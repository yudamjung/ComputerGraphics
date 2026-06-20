using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 짧은 한글 나레이션을 UI Text에 페이드 인/아웃 시킨다.
/// ShowOnce 키는 특정 이벤트(배터리 상태, 유령 접촉 등)가
/// 게임 한 번에 한 줄만 표시되도록 보장한다.
/// </summary>
[RequireComponent(typeof(Text))]
public class NarrativeText : MonoBehaviour
{
    public static NarrativeText Instance;

    public float fadeIn = 0.3f;
    public float hold = 2.2f;
    public float fadeOut = 0.6f;

    Text label;
    Coroutine running;
    readonly HashSet<string> shown = new HashSet<string>();

    void Awake()
    {
        Instance = this;
        label = GetComponent<Text>();
        SetAlpha(0f);
    }

    public void ShowOnce(string key, string message)
    {
        // 해당 키가 이미 표시되었으면 무시
        if (shown.Contains(key)) return;
        shown.Add(key);
        Show(message);
    }

    public void Show(string message)
    {
        // 진행 중인 코루틴이 있으면 중지 후 새 메시지 시작
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(Run(message));
    }

    IEnumerator Run(string message)
    {
        // 메시지 설정 후 순차적으로 페이드인 → 유지 → 페이드아웃
        label.text = message;
        yield return Fade(0f, 1f, fadeIn);
        yield return new WaitForSeconds(hold);
        yield return Fade(1f, 0f, fadeOut);
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        // 투명도를 시간에 따라 선형 보간
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(from, to, t / duration));
            yield return null;
        }
        SetAlpha(to);
    }

    void SetAlpha(float a)
    {
        Color c = label.color;
        c.a = a;
        label.color = c;
    }
}
