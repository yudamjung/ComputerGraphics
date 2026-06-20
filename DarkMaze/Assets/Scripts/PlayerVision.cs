using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 시야 제한 시스템: 플레이어의 Light2D 반경을 이동 상태에 따라 조절한다.
/// 걷기 = 작은 반경, 달리기 = 큰 반경. 나머지는 낮은 강도의 전역 조명으로 어둡다.
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class PlayerVision : MonoBehaviour
{
    public float walkRadius = 4.5f;
    public float runRadius = 6f;
    public float lerp = 6f;

    [Header("Battery falloff (depletionStart% -> 0%)")]
    public float depletionStart = 30f;
    public float minDepletedRadius = 0f;


    PlayerController pc;
    FlashlightBattery battery;

    Light2D vision;

    void Awake()
    {
        pc = GetComponent<PlayerController>();
        battery = GetComponent<FlashlightBattery>();
        vision = GetComponentInChildren<Light2D>();
    }

    void Update()
    {
        if (vision == null) return;

        // 이동 상태에 따른 기본 시야 반경 선택
        float target = pc.State == PlayerController.MoveState.Running ? runRadius : walkRadius;

        // 배터리 상태에 따른 시야 감소 적용
        if (battery != null)
        {
            // depletionStart%부터 0%까지 점진적으로 시야 감소
            float t = Mathf.InverseLerp(depletionStart, 0f, battery.Level);
            target = Mathf.Lerp(target, minDepletedRadius, t) * battery.FlickerMultiplier;
        }

        // 부드러운 전환을 위해 지수 감소 보간
        vision.pointLightOuterRadius = Mathf.Lerp(vision.pointLightOuterRadius, target,
            1f - Mathf.Exp(-lerp * Time.deltaTime));
    }
}
