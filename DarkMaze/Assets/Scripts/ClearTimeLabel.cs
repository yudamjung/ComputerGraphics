using UnityEngine;
using UnityEngine.UI;

/// <summary>게임 씬에서 기록한 클리어 시간을 표시한다.</summary>
[RequireComponent(typeof(Text))]
public class ClearTimeLabel : MonoBehaviour
{
    void Start()
    {
        // GameResult에 저장된 클리어 시간을 텍스트로 표시
        GetComponent<Text>().text = "클리어 타임  " + GameResult.ClearTime.ToString("F1") + "초";
    }
}
