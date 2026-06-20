using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>대상 씬으로 전환하는 버튼 (가능하면 페이드 효과 사용).</summary>
[RequireComponent(typeof(Button))]
public class SceneButton : MonoBehaviour
{
    public string targetScene = "Game";

    void Awake()
    {
        // 버튼 클릭 이벤트 등록
        GetComponent<Button>().onClick.AddListener(Go);
    }

    void Go()
    {
        // ScreenFader가 있으면 페이드 효과로 전환, 없으면 즉시 로드
        if (ScreenFader.Instance != null) ScreenFader.Instance.FadeToScene(targetScene);
        else SceneManager.LoadScene(targetScene);
    }
}
