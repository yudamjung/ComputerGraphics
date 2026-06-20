using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유령 상태 머신: 순찰 -> 추격 (플레이어 소리 감지) -> 순찰로 복귀 (소리 미감지 지속).
/// BFS로 미로를 네비게이션하여 벽을 밀지 않고 복도를 따라간다. 구역 제한 있음.
///
/// 감지 방식은 플레이어 이동 상태에 따른 음성:
///   정지 = 미감지, 걷기 = walkDetect 타일, 달리기 = runDetect 타일.
/// 접촉 중에는 PlayerSlow에 등록하여 플레이어 속도 감소.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class GhostAI : MonoBehaviour
{
    public enum St { Patrol, Chase }

    [Header("Speed")]
    public float patrolSpeed = 1.8f;
    public float chaseSpeed = 3.2f;

    [Header("Hearing (tiles)")]
    public float walkDetect = 3f;
    public float runDetect = 7f;
    public float loseTime = 2.5f;
    public float contactRadius = 0.6f;

    [Header("Zone (cell x range, inclusive)")]
    public int zoneMinCellX = 11;
    public int zoneMaxCellX = 20;

    [Header("Nav")]
    public float repathInterval = 0.25f;

    [Header("Waypoint")]
    public float waypointReachDistance = 0.05f;
    public int patrolAttempts = 40;

    public St State { get; private set; } = St.Patrol;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Transform player;
    PlayerController pc;
    PlayerSlow slow;
    int wallMask;

    float loseTimer;
    float repathTimer;
    Vector2 waypoint;
    Vector2 patrolTarget;
    bool touching;

    // Shared maze walkability grid (all ghosts use the same maze).
    static bool[,] grid;
    static int GW, GH;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        wallMask = 1 << LayerMask.NameToLayer("Wall");
        var p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            pc = p.GetComponent<PlayerController>();
            slow = p.GetComponent<PlayerSlow>();
        }
    }

    void Start()
    {
        // 미로 격자 초기화 및 순찰 시작 위치 설정
        BuildGrid();
        waypoint = transform.position;
        patrolTarget = transform.position;
        PickPatrol();
    }

    // 미로 크기 상수
    const int MazeW = 31;
    const int MazeH = 15;

    void BuildGrid()
    {
        // 모든 유령이 같은 미로 격자 공유 (한 번만 생성)
        if (grid != null) return;
        GW = MazeW; GH = MazeH;
        grid = new bool[GW, GH];

        // 각 셀이 걸을 수 있는 공간인지 확인
        for (int x = 0; x < GW; x++)
            for (int y = 0; y < GH; y++)
                grid[x, y] = Physics2D.OverlapPoint(new Vector2(x + 0.5f, y + 0.5f), wallMask) == null;
    }

    void Update()
    {
        if (player == null) return;

        // 플레이어와의 거리 및 감지 여부 확인
        float dist = Vector2.Distance(transform.position, player.position);
        bool detected = PlayerInZone() && dist <= DetectRadius();

        // 플레이어 감지 시 추격 시작 또는 유지
        if (detected)
        {
            State = St.Chase;
            loseTimer = loseTime;
        }
        else if (State == St.Chase)
        {
            loseTimer -= Time.deltaTime;
            if (loseTimer <= 0f) State = St.Patrol;
        }

        // 플레이어 접촉 상태 업데이트
        bool nowTouch = PlayerInZone() && dist <= contactRadius;
        if (nowTouch != touching)
        {
            touching = nowTouch;
            if (slow != null) slow.SetTouch(GetInstanceID(), touching);
        }

        // 상태에 따라 색상 변경 (추격 = 빨강, 순찰 = 회색)
        if (sr != null)
            sr.color = State == St.Chase ? new Color(1f, 0.22f, 0.22f) : new Color(0.82f, 0.82f, 0.88f);

        // 정기적으로 경로 재계산
        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            repathTimer = repathInterval;
            ComputeMove();
        }
    }

    void FixedUpdate()
    {
        // 상태에 따른 속도 선택
        float spd = State == St.Chase ? chaseSpeed : patrolSpeed;
        Vector2 pos = rb.position;

        // 현재 웨이포인트 도달 시 다음 경로 계산
        if (Vector2.Distance(pos, waypoint) < waypointReachDistance) ComputeMove();

        // 웨이포인트 방향으로 이동
        rb.MovePosition(Vector2.MoveTowards(pos, waypoint, spd * Time.fixedDeltaTime));
    }

    void OnDisable()
    {
        // 유령 비활성화 시 플레이어 접촉 상태 해제
        if (touching && slow != null) slow.SetTouch(GetInstanceID(), false);
        touching = false;
    }

    float DetectRadius()
    {
        // 플레이어 이동 상태에 따른 감지 반경 반환
        if (pc == null) return 0f;
        switch (pc.State)
        {
            case PlayerController.MoveState.Walking: return walkDetect;
            case PlayerController.MoveState.Running: return runDetect;
            default: return 0f;
        }
    }

    bool PlayerInZone()
    {
        if (player == null) return false;
        float px = player.position.x;
        return px >= zoneMinCellX && px <= zoneMaxCellX + 1f;
    }

    Vector2Int CellOf(Vector3 w)
    {
        return new Vector2Int(Mathf.FloorToInt(w.x), Mathf.FloorToInt(w.y));
    }

    // Pathfinding may traverse the whole maze; zone limits only constrain where the
    // ghost patrols and where it can hear the player. Restricting traversal can
    // disconnect the only corridor between two in-zone cells and freeze the ghost.
    bool Walk(int x, int y)
    {
        return x >= 0 && x < GW && y >= 0 && y < GH && grid[x, y];
    }

    Vector2Int ClampZone(Vector2Int c)
    {
        c.x = Mathf.Clamp(c.x, zoneMinCellX, zoneMaxCellX);
        c.y = Mathf.Clamp(c.y, 0, GH - 1);
        return c;
    }

    void ComputeMove()
    {
        Vector2Int s = CellOf(transform.position);
        Vector2Int t = State == St.Chase
            ? ClampZone(CellOf(player.position))
            : ClampZone(CellOf(patrolTarget));

        bool found;
        Vector2Int next = BFSNext(s, t, out found);

        if (State == St.Patrol && (s == t || !found))
        {
            PickPatrol();
            return;
        }

        waypoint = new Vector2(next.x + 0.5f, next.y + 0.5f);
    }

    Vector2Int BFSNext(Vector2Int s, Vector2Int t, out bool found)
    {
        found = false;
        if (s == t) { found = true; return s; }

        var q = new Queue<Vector2Int>();
        var prev = new Dictionary<Vector2Int, Vector2Int>();
        q.Enqueue(s);
        prev[s] = s;
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (q.Count > 0)
        {
            var c = q.Dequeue();
            if (c == t)
            {
                var cur = t;
                while (prev[cur] != s) cur = prev[cur];
                found = true;
                return cur;
            }
            for (int i = 0; i < 4; i++)
            {
                var nc = new Vector2Int(c.x + dx[i], c.y + dy[i]);
                if (Walk(nc.x, nc.y) && !prev.ContainsKey(nc))
                {
                    prev[nc] = c;
                    q.Enqueue(nc);
                }
            }
        }
        return s;
    }

    void PickPatrol()
    {
        // 구역 내에서 걸을 수 있는 순찰 위치를 찾을 때까지 시도
        for (int i = 0; i < patrolAttempts; i++)
        {
            int cx = Random.Range(zoneMinCellX, zoneMaxCellX + 1);
            int cy = Random.Range(0, GH);
            if (Walk(cx, cy))
            {
                patrolTarget = new Vector2(cx + 0.5f, cy + 0.5f);
                return;
            }
        }
    }

    /// <summary>저배터리 경보에 의해 즉시 추격 상태로 전환한다.</summary>
    public void ForceChase()
    {
        State = St.Chase;
        loseTimer = loseTime;
    }

    /// <summary>씬 전환 시 미로 격자를 초기화한다.</summary>
    public static void ResetGrid()
    {
        grid = null;
    }
}
