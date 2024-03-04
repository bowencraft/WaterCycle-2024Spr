using UnityEngine;

public class GeneralController : MonoBehaviour
{
    public float moveSpeed = 5f; // 控制移动速度
    public float jumpForce = 5f; // 控制跳跃力度
    public float verticalSpeed = 2f; // 控制上升和下降的速度
    public float floatSpeed = 10f;
    
    private Rigidbody rb;
    private bool isGrounded;
    [SerializeField]
    private bool isGravityEnabled = true; // 是否受重力影响的开关
    [SerializeField]
    public Transform cameraTransform; // 主相机的Transform
    public float hoverHeight = 10.0f; // 悬浮的目标高度
    public LayerMask groundLayer; // 用于射线检测的地面层

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();

        if (isGravityEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space pressed");
                Jump();
            }
        }
        else
        {
            rb.AddForce(16.4f * Vector3.up);
            CheckHoverHeight();
            
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

    void CheckHoverHeight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, groundLayer))
        {
            float heightAboveGround = hit.distance;
            if (heightAboveGround > hoverHeight + 0.1) // 如果高于指定高度
            {
                // 施加向下的力
                rb.AddForce(-Vector3.up * floatSpeed * (heightAboveGround - hoverHeight)  * Time.deltaTime* 60);
            }
            else if (heightAboveGround < hoverHeight - 0.1) // 如果低于指定高度
            {
                // 施加向上的力
                rb.AddForce(Vector3.up * floatSpeed * (hoverHeight - heightAboveGround)  * Time.deltaTime* 60);
            }
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = (camForward * moveVertical + camRight * moveHorizontal).normalized  * Time.deltaTime* 60;

        rb.AddForce(movement * moveSpeed);
    }

    void Jump()
    {
        Debug.Log("Jumped");
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0,1,0), -Vector3.up, out hit, Mathf.Infinity, groundLayer))
        {
            float heightAboveGround = hit.distance;
            if (heightAboveGround < 2f)
            {
                rb.AddForce(Vector3.up * jumpForce * Time.deltaTime * 60, ForceMode.Impulse);
            }
        }
        //
        // isGrounded = false;
    }

    void VerticalMove(float direction)
    {
        rb.velocity = new Vector3(rb.velocity.x, direction, rb.velocity.z)  * Time.deltaTime * 60;
    }

    void ToggleGravity()
    {
        isGravityEnabled = !isGravityEnabled;
        // 注意：我们在这里不直接使用rb.useGravity来切换重力，
        // 因为我们通过CheckHoverHeight来控制悬浮逻辑
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Dirt"))
        {
            isGrounded = true;
        }
    }
}
