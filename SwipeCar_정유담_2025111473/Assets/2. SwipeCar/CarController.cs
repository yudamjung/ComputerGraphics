using UnityEngine;

public class CarController : MonoBehaviour
{
    float speed = 0;
    Vector2 startPos;                        // 시작 위치

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        // 스와이프의 길이를 구한다.
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스를 클릭한 좌표
            this.startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))     // 자동차 출발
        {
            // 마우스 버튼에서 손가락을 떼었을 때 좌표
            Vector2 endPos = Input.mousePosition;
            float swipelength = endPos.x - this.startPos.x;

            // 스와이프 길이를 처음 속도로 변경한다
            this.speed = swipelength / 1000.0f;

            // 효과음을 재생한다
            GetComponent<AudioSource>().Play();
        }
        // if (Input.GetMouseButtonDown(0))        // 마우스를 클릭하면
        // {
        //     this.speed = 0.2f;                  // 처음 속도를 설정한다
        // }

        transform.Translate(this.speed, 0, 0);  // 이동 - 축의 방향으로 n만큼 이동 O , (좌표)로 이동 X
        this.speed *= 0.98f;                    // 감속
    }
}
