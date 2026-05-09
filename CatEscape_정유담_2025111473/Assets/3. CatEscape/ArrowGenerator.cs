using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    public GameObject arrowPrefab;  //public 선언 - 외부에서 접근 가능
    float span = 1.0f;
    float delta = 0;

    // 화살의 낙하 속도 조절을 위한 변수
    public float initialArrowFallSpeed = 0.1f; // 초기 화살 낙하 속도
    public float arrowSpeedIncreaseRate = 0.01f; // 초당 속도 증가량
    public float maxArrowFallSpeed = 0.5f;     // 최대 낙하 속도

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime;   // Time.deltaTime = 이전 프레임부터 현재 프레임까지 화면을 그리는 데 걸린 시간 (초 단위)
        if (this.delta > this.span)     // 1초가 지나면
        {
            this.delta = 0;
            GameObject go = Instantiate(arrowPrefab);
            int px = Random.Range(-6, 7);       // -6 ~ 6 랜덤 숫자 정함
            go.transform.position = new Vector3(px, 7, 0);      // 랜덤한 x 위치에서 오브젝트 생성
        }

        // 현재 게임 시간에 따라 화살의 낙하 속도 계산
        // Time.time - 게임 시작 후 경과한 시간
        float currentFallSpeed = initialArrowFallSpeed + Time.time * arrowSpeedIncreaseRate;
        currentFallSpeed = Mathf.Min(currentFallSpeed, maxArrowFallSpeed); // 최대 속도 제한

        // 새로 생성된 화살의 ArrowController 컴포넌트를 가져와서 낙하 속도 설정
        ArrowController arrowController = GetComponent<ArrowController>();
        if (arrowController != null)
        {
            arrowController.fallSpeed = currentFallSpeed;
        }
    }
}
