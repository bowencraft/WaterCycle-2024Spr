using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTargetFollowing : MonoBehaviour
{
    public Transform target; // 碰撞体的Transform
    public float smoothSpeed = 0.125f; // 平滑速度
    public Vector3 offset; // 与碰撞体之间的偏移

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        // 或者使用SmoothDamp方法
        // Vector3 velocity = Vector3.zero;
        // Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed * Time.deltaTime);
        
        transform.position = smoothedPosition;

        // 如果你还想让摄像机始终朝向碰撞体，可以取消注释下面的代码
        // transform.LookAt(target);
    }
}
