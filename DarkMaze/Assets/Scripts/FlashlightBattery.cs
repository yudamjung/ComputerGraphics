using UnityEngine;

/// <summary>
/// 손전등 배터리: 시간에 따라 소모되고, 낮아지면 깜빡이면서 근처 유령을 깨운다.
/// BatteryPickup으로 충전된다. PlayerVision이 Level과 FlickerMultiplier를 읽어
/// 실제 시야 반경을 조절한다.
/// </summary>
public class FlashlightBattery : MonoBehaviour
{
    [Header("Battery")]
    public float startPercent = 15f;
    public float drainPerSecond = 1f;
    public float flickerAt = 10f;

    [Header("Low-battery ghost alert")]
    public float alertRadius = 5f;
    public float alertInterval = 1.5f;

    [Header("Flicker")]
    public float flickerSpeed = 14f;
    [Range(0f, 1f)] public float flickerMinMultiplier = 0.3f;

    public float Level { get; private set; }
    public bool Flickering { get { return Level <= flickerAt; } }
    public bool Depleted { get { return Level <= 0f; } }
    public float FlickerMultiplier { get; private set; } = 1f;

    float alertTimer;

    void Awake()
    {
        Level = startPercent;
    }

    void Update()
    {
        // 배터리 소모
        if (Level > 0f)
            Level = Mathf.Max(0f, Level - drainPerSecond * Time.deltaTime);

        // 낮은 배터리 상태에서 깜빡이는 시각 효과
        FlickerMultiplier = Flickering
            ? Mathf.Lerp(flickerMinMultiplier, 1f, (Mathf.Sin(Time.time * flickerSpeed) + 1f) * 0.5f)
            : 1f;

        // 낮은 배터리 때 주기적으로 근처 유령 깨우기
        if (Flickering)
        {
            alertTimer -= Time.deltaTime;
            if (alertTimer <= 0f)
            {
                alertTimer = alertInterval;
                AlertNearbyGhosts();
            }
        }

        Narrate();
    }

    void AlertNearbyGhosts()
    {
        // 알림 반경 내의 모든 유령에게 즉시 추격 신호
        GhostAI[] ghosts = FindObjectsOfType<GhostAI>();
        for (int i = 0; i < ghosts.Length; i++)
        {
            if (Vector2.Distance(ghosts[i].transform.position, transform.position) <= alertRadius)
                ghosts[i].ForceChase();
        }
    }

    void Narrate()
    {
        // 배터리 상태에 따른 스토리 텍스트 표시
        if (NarrativeText.Instance == null) return;
        if (Level <= 25f) NarrativeText.Instance.ShowOnce("battery25", "배터리가 얼마 남지 않았다...");
        if (Flickering) NarrativeText.Instance.ShowOnce("flicker", "오 이런, 손전등이 꺼질 것 같아..!");
        if (Depleted) NarrativeText.Instance.ShowOnce("depleted", "아무것도 보이지 않는다...!");
    }

    /// <summary>BatteryPickup에 의해 호출되어 배터리를 충전한다.</summary>
    public void Restore(float percent)
    {
        Level = Mathf.Clamp(percent, 0f, 100f);
    }
}
