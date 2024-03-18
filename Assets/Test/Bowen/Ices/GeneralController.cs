using UnityEngine;

public class GeneralController : MonoBehaviour
{
    public float moveSpeed = 5f; // 控制移动速度
    public float jumpForce = 5f; // 控制跳跃力度
    public float verticalSpeed = 2f; // 控制上升和下降的速度
    public float floatSpeed = 10f;
    
    private Rigidbody[] rbs;
    private bool isGrounded;
    [SerializeField]
    private bool isGravityEnabled = true; // 是否受重力影响的开关
    [SerializeField]
    public Transform cameraTransform; // 主相机的Transform
    public float hoverHeight = 10.0f; // 悬浮的目标高度
    public LayerMask groundLayer; // 用于射线检测的地面层

    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
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

        foreach (Rigidbody rb in rbs)
        {
            rb.AddForce(movement * moveSpeed, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0,1,0), -Vector3.up, out hit, Mathf.Infinity, groundLayer))
        {
            Debug.Log("Jumped");
            float heightAboveGround = hit.distance;
            if (heightAboveGround < 2f)
            {
                foreach (Rigidbody rb in rbs)
                {
                    rb.AddForce(Vector3.up * jumpForce * Time.deltaTime * 60, ForceMode.Impulse);
                }
            }
        }
        //
        // isGrounded = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Dirt"))
        {
            isGrounded = true;
        }
    }
}
