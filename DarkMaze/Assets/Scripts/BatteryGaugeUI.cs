using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 분할된 배터리 게이지. 0-100% 범위를 segmentCount개 슬라이스로 나눔.
/// 활성 구간은 비례적으로 채워지므로 배터리 소모가 부드러워 보인다.
/// 배터리가 완전히 비면 사각형이 안 보인다.
/// </summary>
public class BatteryGaugeUI : MonoBehaviour
{
    public FlashlightBattery battery;
    public Color fullColor = new Color(0.3f, 0.85f, 0.35f);
    public Color lowColor = new Color(1f, 0.25f, 0.25f);
    public float lowThreshold = 20f;
    public int segmentCount = 10;
    public float segmentGap = 2f;

    Image[] segments;

    void Awake()
    {
        // Player 태그에서 FlashlightBattery 자동 감지
        if (battery == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) battery = player.GetComponent<FlashlightBattery>();
        }

        segments = new Image[segmentCount];

        // 전체 너비에서 간격을 뺀 후 각 세그먼트 너비 계산
        RectTransform rt = (RectTransform)transform;
        float totalWidth = rt.rect.width;
        float segWidth = (totalWidth - segmentGap * (segmentCount - 1)) / segmentCount;

        // 각 세그먼트 UI 생성 (가로로 채워지는 Image)
        for (int i = 0; i < segmentCount; i++)
        {
            GameObject go = new GameObject("Segment" + i, typeof(RectTransform));
            go.transform.SetParent(transform, false);
            Image img = go.AddComponent<Image>();
            img.type = Image.Type.Filled;
            img.fillMethod = Image.FillMethod.Horizontal;

            // 세그먼트 위치 및 크기 설정
            RectTransform segRt = (RectTransform)go.transform;
            segRt.anchorMin = new Vector2(0f, 0f);
            segRt.anchorMax = new Vector2(0f, 1f);
            segRt.pivot = new Vector2(0f, 0.5f);
            segRt.anchoredPosition = new Vector2(i * (segWidth + segmentGap), 0f);
            segRt.sizeDelta = new Vector2(segWidth, 0f);

            segments[i] = img;
        }
    }

    void Update()
    {
        if (battery == null || segments == null) return;

        // 배터리 상태에 따라 색상 결정
        Color c = battery.Level <= lowThreshold ? lowColor : fullColor;
        float perSegment = 100f / segmentCount;

        // 각 세그먼트의 채움 정도 업데이트
        for (int i = 0; i < segments.Length; i++)
        {
            float segStart = i * perSegment;
            // 현재 세그먼트가 배터리 수준에 어느 정도 포함되는지 계산
            float fill = Mathf.Clamp01((battery.Level - segStart) / perSegment);
            segments[i].enabled = fill > 0f;
            segments[i].fillAmount = fill;
            segments[i].color = c;
        }
    }
}
