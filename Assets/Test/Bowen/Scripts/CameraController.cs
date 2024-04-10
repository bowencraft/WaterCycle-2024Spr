using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public Vector2 distanceMinMax = new Vector2(1f, 10f);
    public Vector2 yAngleMinMax = new Vector2(-40, 80);

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    [SerializeField]
    private float sensitivityX = 2.0f;
    [SerializeField]
    private float sensitivityY = 2.0f;

    // 用于平滑旋转的变量
    [SerializeField]
    private float smoothTime = 0.2f; // 调整这个值以改变平滑的速度
    private float velocityX = 0.0f; // X轴旋转的速度（内部使用）
    private float velocityY = 0.0f; // Y轴旋转的速度（内部使用）
    
    public LayerMask ignoreLayerMask;
    
    
    static CameraController instance;
    public static CameraController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CameraController>();
            }
            return instance;
        }
    }

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
        currentY = Mathf.Clamp(currentY, yAngleMinMax.x, yAngleMinMax.y);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance = Mathf.Clamp(distance - scroll * 5, distanceMinMax.x, distanceMinMax.y);
    }

    private void FixedUpdate()
    {
        float targetX = Mathf.SmoothDampAngle(transform.eulerAngles.y, currentX, ref velocityX, smoothTime);
        float targetY = Mathf.SmoothDampAngle(transform.eulerAngles.x, currentY, ref velocityY, smoothTime);

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(targetY, targetX, 0);
        transform.position = target.position + rotation * dir;

        AdjustCameraIfObstructed();

        transform.LookAt(target);
    }

    private void AdjustCameraIfObstructed()
    {
        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit, ~ignoreLayerMask))
        {
            transform.position = Vector3.Lerp(target.position, hit.point, 0.9f);
        }
    }
}