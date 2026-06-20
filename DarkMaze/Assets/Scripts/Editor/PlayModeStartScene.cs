using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// 플레이 모드 시작 시 항상 Intro 씬부터 시작하도록 설정.
/// 메뉴로 토글 가능하며, EditorPrefs로 설정 저장.
/// </summary>
[InitializeOnLoad]
public static class PlayModeStartScene
{
    // Intro 씬의 경로 (프로젝트 내 고정)
    const string IntroPath = "Assets/Scenes/Intro.unity";
    // 기능 활성화 여부를 저장하는 EditorPrefs 키
    const string PrefKey  = "PlayModeStartScene.enabled";

    static PlayModeStartScene()
    {
        // 에디터 플레이 모드 상태 변경 이벤트 등록
        EditorApplication.playModeStateChanged += OnPlayModeState;
    }

    static void OnPlayModeState(PlayModeStateChange state)
    {
        // 플레이 모드 진입 시만 처리
        if (state != PlayModeStateChange.ExitingEditMode) return;
        if (!EditorPrefs.GetBool(PrefKey, true)) return;

        var active = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        // 이미 Intro 씬이면 바꾸지 않음
        if (active.path == IntroPath) return;

        // 저장 여부 확인 후 Intro 씬 열기
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(IntroPath);
    }

    [MenuItem("DarkMaze/항상 Intro부터 시작 %#i")]
    static void Toggle()
    {
        // 현재 설정 토글 및 로그 출력
        bool on = EditorPrefs.GetBool(PrefKey, true);
        EditorPrefs.SetBool(PrefKey, !on);
        UnityEngine.Debug.Log("PlayModeStartScene: " + (!on ? "ON (Intro부터)" : "OFF (현재 씬부터)"));
    }
}
