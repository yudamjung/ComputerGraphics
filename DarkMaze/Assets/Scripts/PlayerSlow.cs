using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유령과 접촉 시 플레이어의 속도를 감소시킨다.
/// 접촉 중인 유령의 수에 따라 속도 수정자를 조절한다.
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class PlayerSlow : MonoBehaviour
{
    [Header("유령 수에 따른 속도 수정자 (1 = 정상 속도)")]
    [SerializeField] float speedWith1Ghost  = 0.7f;
    [SerializeField] float speedWith2Ghosts = 0.4f;
    [SerializeField] float speedWith3Ghosts = 0.2f;

    PlayerController pc;
    // 현재 접촉 중인 유령의 ID 저장
    readonly HashSet<int> touching = new HashSet<int>();

    void Awake() { pc = GetComponent<PlayerController>(); }

    public void SetTouch(int ghostId, bool on)
    {
        // 유령 접촉 상태 업데이트
        if (on) touching.Add(ghostId);
        else touching.Remove(ghostId);

        // 첫 접촉 시 메시지 표시
        if (on && NarrativeText.Instance != null)
            NarrativeText.Instance.ShowOnce("ghostContact", "뭔가 스치고 지나갔다... 발이 무거워진다");
    }

    void Update()
    {
        // 접촉 중인 유령 수에 따른 속도 수정자 적용
        float[] mod = { 1f, speedWith1Ghost, speedWith2Ghosts, speedWith3Ghosts };
        pc.SpeedModifier = mod[Mathf.Min(touching.Count, 3)];
    }
}
