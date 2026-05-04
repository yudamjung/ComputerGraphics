using UnityEngine;
using UnityEngine.UI;   // UI를 사용하므로 잊지 않고 추가한다.

public class GameDirector : MonoBehaviour
{
    GameObject hpGauge;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.hpGauge = GameObject.Find("hpGauge");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DecreaseHp()
    {
        this.hpGauge.GetComponent<Image>().fillAmount -= 0.1f;
    }
}
