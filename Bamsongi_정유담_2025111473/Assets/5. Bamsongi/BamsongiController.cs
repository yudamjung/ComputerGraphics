using UnityEngine;

public class BamsongiController : MonoBehaviour
{
    public void Shoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }

    // 충돌했을 때 중력 무효화시키는 메서드
    // isKinematic - 중력 무효화 (default값 false)
    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<ParticleSystem>().Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        // Shoot(new Vector3(0, 200, 2000)); // y방향 200 + z방향 2000만큼 힘을 줌
    }

    // Update is called once per frame
    void Update()
    {

    }
}
