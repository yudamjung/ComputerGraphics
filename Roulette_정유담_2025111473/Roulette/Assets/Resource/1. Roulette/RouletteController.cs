using UnityEngine;

public class RouletteController : MonoBehaviour
{
    float rotSpeed = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 휠 클릭 -> 룰렛 돌아감
        // 마우스 좌 클릭 -> 룰렛 서서히 멈춤
        // 마우스 우 클릭 동안 -> 룰렛 돌아감
        // 마우스 손 뗌 -> 룰렛 서서히 멈춤
        // 룰렛의 회전 방향 -> 시계방향
        if (Input.GetMouseButtonDown(2))
        {
            rotSpeed = 10;
        }

        if (Input.GetMouseButton(1))
        {
            rotSpeed = 10;
        }
        else
        {
            rotSpeed *= 0.96f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            rotSpeed *= 0.96f;
        }

        transform.Rotate(0, 0, -this.rotSpeed);
    }
}
