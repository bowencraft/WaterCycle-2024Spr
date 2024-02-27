using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // 控制移动速度
    public float jumpForce = 5f; // 控制跳跃力度
    public float verticalSpeed = 2f; // 控制上升和下降的速度
    private Rigidbody rb;
    private bool isGrounded;
    [SerializeField]
    private bool isGravityEnabled = true; // 控制是否受重力影响的开关

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();

        if (isGravityEnabled)
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
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

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * moveSpeed);
    }

    void Jump()
    {
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
        rb.useGravity = isGravityEnabled; // 启用或禁用Rigidbody的重力
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}