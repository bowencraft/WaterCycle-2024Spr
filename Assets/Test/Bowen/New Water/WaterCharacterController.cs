using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterCharacterController : MonoBehaviour
{
    // 控制参数
    public float jumpForce = 5f; // 控制跳跃力度
    public float maxSpeed = 5f; // 控制最大移动速度
    public float acceleration = 2f; // 控制加速度
    public float maxSlopeAngle = 45f; // 能够移动上的最大斜率角度
    
    private List<Rigidbody> rbs = new List<Rigidbody>();
    private bool isGrounded;
    private bool isJumping = false;
    public float ResetJumpTime = 0.8f; // 控制跳跃后重置跳跃状态的时间
    public Transform cameraTransform; // 主相机的Transform
    public LayerMask groundLayer; // 用于射线检测的地面层
    
    

    void Start()
    {
        // rbs = GetComponentsInChildren<Rigidbody>().ToList();
        rbs.Add(GetComponent<Rigidbody>());
        
        
         // add getcomponent<rigidbody>();
    }
    
    void FixedUpdate()
    {
        Move();
        
        if (!isJumping && Input.GetKeyDown("space"))
        {
            Jump();
        }
    }
    
    
    void Jump()
    {
        isJumping = true;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0,1,0), -Vector3.up, out hit, Mathf.Infinity, groundLayer))
        {
            Debug.Log("Jumped");
            float heightAboveGround = hit.distance;
            if (heightAboveGround < 4f)
            {
                foreach (var rb in rbs)
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
                Invoke("ResetJump", ResetJumpTime);
            }
        }
        //
        // isGrounded = false;
    }
    
    void ResetJump() {
        isJumping = false;
    }

    void Move()
    {
        // if (!isGrounded) return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 targetDirection = (camForward * moveVertical + camRight * moveHorizontal).normalized;

        // Check slope
        if (IsSlopeSteep(targetDirection))
        {
            foreach (var rb in rbs)
            {
                rb.drag = 20; // 增加阻力使其更快停止
                // rb.useGravity = false; // 取消重力影响
            }

            return; // 斜率过陡，不移动
        }

        Vector3 desiredVelocity = targetDirection * maxSpeed;
        Vector3 velocityChange = (desiredVelocity - rbs.First().velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -acceleration, acceleration);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -acceleration, acceleration);
        velocityChange.y = 0;

        foreach (var rb in rbs)
        {
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        if (moveHorizontal != 0f || moveVertical != 0f)
        {
            foreach (var rb in rbs)
            {
                rb.drag = 0; // 低阻力
                // rb.useGravity = true; // 使用重力
            }
        }
        else
        {

            foreach (var rb in rbs)
            {
                rb.drag = 10; // 增加阻力使其更快停止
                // rb.useGravity = false; // 取消重力影响
            }
        }
    }

    bool IsSlopeSteep(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 2f, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return slopeAngle > maxSlopeAngle;
        }
        return false;
    }
    
    // void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //     {
    //         isGrounded = true;
    //     }
    // }
    //
    // void OnCollisionExit(Collision other)
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //     {
    //         isGrounded = false;
    //     }
    // }
}
