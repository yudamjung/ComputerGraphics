using UnityEngine;
using UnityEngine.Rendering.Universal;


/// <summary>
/// 플레이어와 접촉하면 손전등 배터리를 100%로 복구하고 사라진다.
/// 어두움 속에서도 눈에 띄도록 Light2D 자식으로 희미하게 발광한다.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class BatteryPickup : MonoBehaviour
{
    [Header("Twinkle (stays visible even in total blackout)")]
    public float twinkleSpeed = 3f;
    public float minIntensity = 0.6f;
    public float maxIntensity = 1.6f;

    Light2D glow;

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        glow = GetComponent<Light2D>();
    }

    void Update()
    {
        if (glow == null) return;

        // 사인파를 이용한 깜빡이는 발광 효과
        float t = (Mathf.Sin(Time.time * twinkleSpeed) + 1f) * 0.5f;
        glow.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 플레이어의 배터리를 100%로 복구
        FlashlightBattery battery = other.GetComponent<FlashlightBattery>();
        if (battery != null) battery.Restore(100f);

        // 배터리 획득 메시지 표시
        if (NarrativeText.Instance != null)
            NarrativeText.Instance.ShowOnce("batteryPickup", "다행이다...!");

        // 아이템 비활성화
        gameObject.SetActive(false);
    }
}
