using UnityEngine;

/// <summary>대상을 XY 평면에서 부드럽게 따라다니며 자신의 Z는 유지한다.</summary>
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 8f;

    void LateUpdate()
    {
        if (target == null) return;

        // 대상 위치에서 카메라의 Z값 유지
        Vector3 p = target.position;
        p.z = transform.position.z;

        // 지수 감소를 이용한 부드러운 추적
        transform.position = Vector3.Lerp(transform.position, p, 1f - Mathf.Exp(-smooth * Time.deltaTime));
    }
}
