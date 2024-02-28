using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsController : MonoBehaviour
{
    public float moveSpeed = 5f; // 控制移动速度
    public float floatSpeed = 2f;
    
    private Rigidbody rb;
    [SerializeField]
    public Transform cameraTransform; // 主相机的Transform
    public float hoverHeight = 10.0f; // 悬浮的目标高度
    public LayerMask groundLayer; // 用于射线检测的地面层

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        rb.AddForce(16.4f * Vector3.up);
        CheckHoverHeight();
            
        if (Input.GetKey(KeyCode.Q))
        {
            VerticalMove(floatSpeed); // 上升
        }
        else if (Input.GetKey(KeyCode.E))
        {
            VerticalMove(-floatSpeed); // 下降
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
                rb.AddForce(-Vector3.up * 0.1f * (heightAboveGround - hoverHeight));
            }
            else if (heightAboveGround < hoverHeight - 0.1) // 如果低于指定高度
            {
                // 施加向上的力
                rb.AddForce(Vector3.up * 0.1f * (hoverHeight - heightAboveGround));
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

        Vector3 movement = (camForward * moveVertical + camRight * moveHorizontal).normalized;

        rb.AddForce(movement * moveSpeed);
    }


    void VerticalMove(float direction)
    {
        rb.velocity = new Vector3(rb.velocity.x, direction, rb.velocity.z);
    }
    
}
