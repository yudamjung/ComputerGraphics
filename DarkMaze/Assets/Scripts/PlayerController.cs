using UnityEngine;

/// <summary>
/// 탑다운 플레이어 이동. WASD/화살표로 걷기, Shift로 달리기(~1.8배).
/// 이동 상태를 노출하여 오디오, 안개 전쟁, 유령 AI가 반응할 수 있다.
/// 유령은 SpeedModifier로 속도 감소 적용 (1 = 정상, 0.2 = 80% 느림).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public enum MoveState { Idle, Walking, Running }

    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runMultiplier = 1.8f;

    public MoveState State { get; private set; }
    public Vector2 FacingDir { get; private set; } = Vector2.down;
    public float SpeedModifier { get; set; } = 1f;

    Rigidbody2D rb;
    Vector2 input;
    bool running;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 입력 받기 (8방향 지원)
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        input = new Vector2(x, y);

        // 대각선 입력 정규화 (대각선 속도 = 축 방향 속도)
        if (input.sqrMagnitude > 1f) input = input.normalized;

        // 달리기 키 확인
        running = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // 이동 상태 업데이트
        if (input.sqrMagnitude < 0.01f)
        {
            State = MoveState.Idle;
        }
        else
        {
            State = running ? MoveState.Running : MoveState.Walking;
            FacingDir = input.normalized;
        }
    }

    void FixedUpdate()
    {
        // 이동 상태와 수정자를 반영한 최종 속도 계산
        float speed = walkSpeed * (running ? runMultiplier : 1f) * SpeedModifier;
        rb.linearVelocity = input * speed;
    }
}
