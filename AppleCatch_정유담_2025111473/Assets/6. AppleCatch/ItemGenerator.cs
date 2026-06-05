using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject bombPrefab;
    float span = 1.0f;
    float delta = 0;
    int ratio = 2;      // bomb가 나올 확률 - 변수 선언으로 따로 빼야 레벨 디자인이 편함!
    float speed = -0.03f;


    public void SetParameter(float span, float speed, int ratio)
    {
        this.span = span;
        this.speed = speed;
        this.ratio = ratio;
    }

    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime;
        if (this.delta > this.span)
        {
            this.delta = 0;
            GameObject item; /*= Instantiate(applePrefab);   // 1초당 사과 하나씩 생성 */
            int dice = Random.Range(1, 11);         // 1 ~ 10 정수 랜덤
            if (dice <= this.ratio)
            {
                item = Instantiate(bombPrefab);     // 1 or 2 일 때 (10까지 있는 주사위의 눈이 1 혹은 2가 나올때나 마찬가지)
            }
            else
            {
                item = Instantiate(applePrefab);    // 그 외
            }

            float x = Random.Range(-1, 2);                // -1 이상 2 미만 정수
            float z = Random.Range(-1, 2);                // -1 이상 2 미만 정수
            item.transform.position = new Vector3(x, 4, z);
            item.GetComponent<ItemController>().dropSpeed = this.speed;
        }
    }
}
