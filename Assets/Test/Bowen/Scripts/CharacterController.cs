using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // 控制移动速度
    public float jumpForce = 5f; // 控制跳跃力度
    public float verticalSpeed = 2f; // 控制上升和下降的速度
    private Rigidbody rb;
    private bool isGrounded;
    private bool isGravityEnabled = true; // 是否受重力影响的开关
    [SerializeField]
    public Transform cameraTransform; // 主相机的Transform

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();

        if (isGravityEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Q))
            {
                VerticalMove(verticalSpeed); // 上升
            }
            else if (Input.GetKey(KeyCode.E))
            {
                VerticalMove(-verticalSpeed); // 下降
            }
        }

        // 按G键切换重力影响
        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGravity();
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 使用相机的方向来确定移动方向
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // 忽略相机的Y轴，防止角色向上或向下移动
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = (camForward * moveVertical + camRight * moveHorizontal).normalized;

        rb.AddForce(movement * moveSpeed);
    }

    void Jump()
    {
        Debug.Log("Jumped");
        
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void VerticalMove(float direction)
    {
        rb.velocity = new Vector3(rb.velocity.x, direction, rb.velocity.z);
    }

    void ToggleGravity()
    {
        isGravityEnabled = !isGravityEnabled;
        rb.useGravity = isGravityEnabled; // 启用或禁用 Rigidbody 的重力
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Dirt"))
        {
            isGrounded = true;
        }
    }
}
