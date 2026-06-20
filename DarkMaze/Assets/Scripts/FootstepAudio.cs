using UnityEngine;

/// <summary>
/// 플레이어의 이동 상태에 따라 발소리를 주기적으로 재생한다.
/// 정지 = 무음, 걷기 = 느림 + 조용함, 달리기 = 빠름 + 큼.
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class FootstepAudio : MonoBehaviour
{
    [Header("Clips (assign imported footstep audio)")]
    public AudioClip walkClip;
    [Tooltip("Optional separate clip for running. Falls back to walkClip when empty.")]
    public AudioClip runClip;

    [Header("Cadence (seconds between steps)")]
    public float walkInterval = 0.45f;
    public float runInterval = 0.28f;

    [Header("Volume / Pitch")]
    public float walkVolume = 0.4f;
    public float runVolume = 0.9f;
    public float walkPitch = 1f;
    public float runPitch = 1.35f;

    PlayerController pc;
    AudioSource src;
    float stepTimer;

    void Awake()
    {
        pc = GetComponent<PlayerController>();
        src = gameObject.AddComponent<AudioSource>();
        src.loop = false;
        src.playOnAwake = false;
        src.spatialBlend = 0f;
    }

    void Update()
    {
        // 정지 상태면 즉시 재생 준비 (다음 이동 시 즉시 발소리)
        if (pc.State == PlayerController.MoveState.Idle)
        {
            stepTimer = 0f;
            return;
        }

        // 이동 상태에 따른 발소리 간격 선택
        bool running = pc.State == PlayerController.MoveState.Running;
        float interval = running ? runInterval : walkInterval;

        // 발소리 타이머 감소 및 발동 시 재생
        stepTimer -= Time.deltaTime;
        if (stepTimer <= 0f)
        {
            stepTimer = interval;
            // 달리기 중이면 runClip 선호, 없으면 walkClip 사용
            AudioClip clip = running ? (runClip != null ? runClip : walkClip) : walkClip;
            if (clip != null)
            {
                src.pitch = running ? runPitch : walkPitch;
                src.PlayOneShot(clip, running ? runVolume : walkVolume);
            }
        }
    }
}
