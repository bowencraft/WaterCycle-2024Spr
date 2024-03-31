using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 玩家角色的Transform
    public float distance = 5.0f; // 摄像机与角色的默认距离
    public Vector2 distanceMinMax = new Vector2(1f, 10f); // 摄像机与角色的最小和最大距离
    public Vector2 yAngleMinMax = new Vector2(-40, 80); // Y轴旋转的最小和最大值

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensitivityX = 4.0f; // X轴旋转灵敏度
    private float sensitivityY = 1.0f; // Y轴旋转灵敏度

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
        currentY = Mathf.Clamp(currentY, yAngleMinMax.x, yAngleMinMax.y);

        // 检测鼠标滚轮输入，调整摄像机与角色之间的距离
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance = Mathf.Clamp(distance - scroll * 5, distanceMinMax.x, distanceMinMax.y);
    }

    private void FixedUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * dir;

        AdjustCameraIfObstructed();

        transform.LookAt(target); // 使摄像机始终看向玩家角色
    }

    // 生成一个LayerMask，表示所有除了"IgnoredLayer"之外的Layers
    public LayerMask ignoreLayerMask;
    
    private void AdjustCameraIfObstructed()
    {

        RaycastHit hit;
        if (Physics.Linecast(target.position,transform.position,out hit, ~ignoreLayerMask))
        {
            transform.position = hit.point;
            // transform.position = Vector3.Lerp(transform.position, hit.point, 0.5f); // 当检测到遮挡时，将摄像机移向角色
        }
    }
}