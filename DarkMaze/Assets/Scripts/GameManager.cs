using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 씬 관리자: 시간 제한을 카운트다운하고 화면 상단 중앙에 표시한다.
/// 시간 초과 -> Intro로 (게임 오버), 탈출 -> 성공.
/// </summary>
public class GameManager : MonoBehaviour
{
    public float timeLimit = 45f;
    public Text timerText;
    [Tooltip("Seconds remaining at/below which the timer turns red.")]
    public float warnAt = 10f;

    float remaining;
    bool ended;

    void Start()
    {
        remaining = timeLimit;
        UpdateText();
    }

    void Update()
    {
        if (ended) return;

        // 남은 시간 카운트다운
        remaining -= Time.deltaTime;
        if (remaining <= 0f)
        {
            remaining = 0f;
            UpdateText();
            GameOver();
            return;
        }
        UpdateText();
    }

    void UpdateText()
    {
        if (timerText == null) return;

        // 타이머 텍스트와 색상 업데이트
        timerText.text = Mathf.CeilToInt(remaining).ToString();
        // 경고 시간 이하면 빨간색으로 표시
        timerText.color = remaining <= warnAt ? new Color(1f, 0.25f, 0.25f) : Color.white;
    }

    void GameOver()
    {
        // 시간 초과 시 패배 처리
        ended = true;
        Transition("Fail");
    }

    /// <summary>플레이어가 탈출구에 도달하면 호출된다.</summary>
    public void Win()
    {
        if (ended) return;

        ended = true;
        // 클리어 시간 기록
        GameResult.ClearTime = timeLimit - remaining;
        Transition("Success");
    }

    void Transition(string scene)
    {
        // ScreenFader가 있으면 페이드 효과로 씬 전환, 없으면 즉시 로드
        if (ScreenFader.Instance != null) ScreenFader.Instance.FadeToScene(scene);
        else SceneManager.LoadScene(scene);
    }
}
