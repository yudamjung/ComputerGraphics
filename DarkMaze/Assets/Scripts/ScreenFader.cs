using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 전체 화면 검은색 오버레이. 씬 시작 시 페이드 인하고, 다음 씬 로드 전 페이드 아웃.
/// 씬마다 하나씩 배치하며, ScreenFader.Instance로 접근.
/// </summary>
[RequireComponent(typeof(Image))]
public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;
    public float fadeDuration = 0.6f;

    Image img;

    void Awake()
    {
        Instance = this;
        img = GetComponent<Image>();
    }

    void Start()
    {
        // 검은 화면에서 페이드 인
        StartCoroutine(Fade(1f, 0f));
    }

    public void FadeToScene(string scene)
    {
        // 페이드 아웃 후 씬 로드
        StartCoroutine(FadeOutLoad(scene));
    }

    IEnumerator FadeOutLoad(string scene)
    {
        // 페이드 아웃 완료 후 씬 로드
        yield return Fade(0f, 1f);
        SceneManager.LoadScene(scene);
    }

    IEnumerator Fade(float from, float to)
    {
        // 투명도를 graduallyTime에 걸쳐 변환
        float t = 0f;
        Color c = img.color;
        c.a = from; img.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / fadeDuration);
            img.color = c;
            yield return null;
        }

        // 최종 투명도 보정
        c.a = to; img.color = c;
    }
}
