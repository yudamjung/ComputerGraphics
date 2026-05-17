using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;

    float jumpForce = 600f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 2.0f;

    // 플립북 방식의 걷기 애니메이션
    public Sprite[] walkSprites;
    // 점프 중인 모습 추가 
    public Sprite jumpSprite;

    float time = 0;
    int idx = 0;

    SpriteRenderer spriteRenderer;

    bool isGrounded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;       // 60프레임 설정

        this.rigid2D = GetComponent<Rigidbody2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Mathf.Abs(this.rigid2D.linearVelocityY) < 0.01f;

        // 점프한다 (바닥에 있을 때만 가능)
        if (isGrounded && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            // y축 방향으로 jumpForce 600만큼의 힘을 가한다
            this.rigid2D.AddForce(transform.up * this.jumpForce); // AddForce - 매개변수 방향으로 힘을 주겠다
        }

        // 우측으로 이동한다.
        // linearVelocityX - 현재 x축 방향으로 속도가 얼마냐
        if (this.rigid2D.linearVelocityX < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * this.walkForce);
            // 우측으로 walkForce만큼 이동하기
        }

        // 점프 애니메이션
        if (this.rigid2D.linearVelocityY != 0)  // y축으로 속도가 0이 아니면 - 점프가 발생할 때
        {
            this.spriteRenderer.sprite = this.jumpSprite;   // 점프하는 이미지
        }
        else
        {
            // 애니메이션
            this.time += Time.deltaTime;
            if (this.time > 0.1f)       // 0.1초마다 스프라이트를 바꾼다
            {
                this.time = 0;
                this.spriteRenderer.sprite = this.walkSprites[this.idx];
                this.idx = 1 - this.idx;
            }
        }

        // 화면 아래로 떨어지면 게임 재시작
        if (transform.position.y < -10)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        this.isGrounded = true;
    }

    // 깃발에 닿았을 때 충돌 판정
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("골");
        SceneManager.LoadScene("ClearScene");
    }
}
