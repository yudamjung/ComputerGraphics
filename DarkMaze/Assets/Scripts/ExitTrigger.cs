using UnityEngine;

/// <summary>플레이어가 탈출 트리거에 들어가면 승리 조건을 발동한다.</summary>
[RequireComponent(typeof(Collider2D))]
public class ExitTrigger : MonoBehaviour
{
    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 진입하면 게임 승리 처리
        if (other.CompareTag("Player") && gm != null) gm.Win();
    }
}
