using UnityEngine;

/// <summary>
/// 전역 BGM 관리. 모든 씬에 걸쳐 배경 음악을 루프 재생.
/// DontDestroyOnLoad를 사용한 싱글톤으로 씬 전환 시에도 유지된다.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip bgmClip;
    [Range(0f, 1f)]
    public float bgmVolume = 0.5f;

    AudioSource bgmSource;

    void Awake()
    {
        // 싱글톤 패턴: 인스턴스가 이미 있으면 새 객체 제거
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // 씬 전환 시에도 이 객체 유지
        DontDestroyOnLoad(gameObject);

        // AudioSource 컴포넌트 가져오기 또는 새로 추가
        bgmSource = GetComponent<AudioSource>();
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }

        // AudioSource 설정
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;
        bgmSource.playOnAwake = true;

        // BGM 재생 시작
        if (!bgmSource.isPlaying && bgmClip != null)
        {
            bgmSource.Play();
        }
    }

    void Update()
    {
        // 인스펙터에서 볼륨 조정 시 실시간 반영
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;
    }
}
