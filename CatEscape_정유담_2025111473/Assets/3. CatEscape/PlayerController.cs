using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 플레이어 이동량
    public float moveAmount = 3f;


    public float minX = -6.5f; // 플레이어 중심이 도달할 수 있는 최소 X 좌표
    public float maxX = 6.5f;  // 플레이어 중심이 도달할 수 있는 최대 X 좌표

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void LButtonDown()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x -= moveAmount; // 왼쪽으로 이동

        // 경계 내로 제한
        if (currentPosition.x < minX)
        {
            currentPosition.x = minX;
        }
        transform.position = currentPosition;
    }

    public void RButtonDown()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x += moveAmount; // 오른쪽으로 이동
        if (currentPosition.x > maxX)
        {
            currentPosition.x = maxX;
        }
        transform.position = currentPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position; // 현재 위치를 기준으로 새로운 위치 계산

        // 왼쪽 화살표가 눌렸을 때
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newPosition.x -= moveAmount;
        }

        // 오른쪽 화살표가 눌렸을 때
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            newPosition.x += moveAmount;
        }

        // 계산된 새로운 X 위치를 화면 경계 내로 제한
        if (newPosition.x < minX)
        {
            newPosition.x = minX;
        }
        else if (newPosition.x > maxX)
        {
            newPosition.x = maxX;
        }

        // 최종 위치를 플레이어 오브젝트에 적용
        transform.position = newPosition;
    }

}
