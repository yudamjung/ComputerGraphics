using UnityEngine;
using TMPro;

public class GameDirector : MonoBehaviour
{
    GameObject car;
    GameObject flag;
    GameObject distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 주의 : Hierarchy 창에 있는 오브젝트의 이름과 동일하게 작성해야함!
        this.car = GameObject.Find("car");
        this.flag = GameObject.Find("flag_0");
        this.distance = GameObject.Find("Distance_kr");
    }

    // Update is called once per frame
    void Update()
    {
        float length = this.flag.transform.position.x - this.car.transform.position.x;
        if (length >= 0)
        {
            this.distance.GetComponent<TextMeshProUGUI>().text = "깃발까지 남은 거리: " + length.ToString("F2") + "m";
        }
        else
            this.distance.GetComponent<TextMeshProUGUI>().text = "게임 오버!";
    }
}
