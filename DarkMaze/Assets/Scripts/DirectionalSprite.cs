using UnityEngine;

/// <summary>
/// 이동 방향에 따라 SpriteRenderer를 4가지 탑다운 스프라이트로 교체한다.
/// 위치 변화를 추적하므로 다이나믹 및 키네마틱 바디 모두 지원.
/// 정지 상태에서는 마지막 방향을 유지한다.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class DirectionalSprite : MonoBehaviour
{
    public Sprite up, down, left, right;

    // 움직임 감지를 위한 최소 거리 임계값 (너무 작은 움직임은 무시)
    const float MIN_MOVEMENT_THRESHOLD = 0.0000004f;

    SpriteRenderer sr;
    Vector2 lastPos;
    Vector2 face = Vector2.down;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lastPos = transform.position;
        Apply();
    }

    void Update()
    {
        Vector2 pos = transform.position;
        Vector2 delta = pos - lastPos;
        lastPos = pos;

        // 최소 임계값보다 큰 움직임만 감지
        if (delta.sqrMagnitude > MIN_MOVEMENT_THRESHOLD)
        {
            // 수평 또는 수직 중 더 큰 변화량을 기준으로 방향 결정
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                face = delta.x > 0f ? Vector2.right : Vector2.left;
            else
                face = delta.y > 0f ? Vector2.up : Vector2.down;
            Apply();
        }
    }

    void Apply()
    {
        // 현재 방향에 해당하는 스프라이트 선택
        Sprite s;
        if (face == Vector2.up) s = up;
        else if (face == Vector2.left) s = left;
        else if (face == Vector2.right) s = right;
        else s = down;

        // 스프라이트가 할당되어 있으면 적용
        if (s != null) sr.sprite = s;
    }
}
