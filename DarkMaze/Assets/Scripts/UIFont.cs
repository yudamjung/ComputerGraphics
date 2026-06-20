using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 런타임에 갈무리 픽셀 폰트(한글 지원)를 UI Text에 적용한다.
/// 갈무리를 찾을 수 없으면 한글을 지원하는 OS 폰트로 대체한다.
/// </summary>
[RequireComponent(typeof(Text))]
public class UIFont : MonoBehaviour
{
    static readonly string[] Candidates =
    {
        "Apple SD Gothic Neo", "AppleGothic", "Arial Unicode MS", "Malgun Gothic", "Noto Sans CJK KR"
    };

    void Awake()
    {
        var text = GetComponent<Text>();

        // 먼저 갈무리 픽셀 폰트 로드 시도
        Font pixelFont = Resources.Load<Font>("Fonts/Galmuri11");
        if (pixelFont != null)
        {
            text.font = pixelFont;
            return;
        }

        // 갈무리 없으면 OS 폰트 중 한글 지원하는 폰트 사용
        int size = text.fontSize > 0 ? text.fontSize : 40;
        var font = Font.CreateDynamicFontFromOSFont(Candidates, size);
        if (font != null) text.font = font;
    }
}
