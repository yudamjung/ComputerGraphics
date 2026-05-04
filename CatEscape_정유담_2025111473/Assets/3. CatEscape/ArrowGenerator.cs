using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    public GameObject arrowPrefab;  //public 선언 - 외부에서 접근 가능
    float span = 1.0f;
    float delta = 0;

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
    }
}
