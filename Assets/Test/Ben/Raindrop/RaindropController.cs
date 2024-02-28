using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaindropController : MonoBehaviour
{
    private static RaindropController instance;

    [Header("References")]
    [SerializeField] public RaindropAppearance appearance;
    [SerializeField] public Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float accelerationRate;
    [SerializeField] private float maxSpeed;

    [Header("Jump")]
    [SerializeField] private float castDistance;
    [SerializeField] private float jumpSpeed;

    private bool canJump = false;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(rb.position, Vector3.down * castDistance);
    }
#endif

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        ProcessMovement();
        ProcessJump();
    }

    private void ProcessMovement()
    {
        Vector3 flatCamForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 flatCamRight = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;

        Vector3 moveInput = Vector3.zero;
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = (flatCamForward * moveInput.y + flatCamRight * moveInput.x).normalized;

        Vector3 newVelocity = rb.velocity + moveDirection * accelerationRate * Time.fixedDeltaTime;
        newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);

        rb.velocity = newVelocity;
    }

    private void ProcessJump()
    {
        bool grounded = Physics.Raycast(rb.position, Vector3.down, castDistance);

        if (grounded && rb.velocity.y < 0) canJump = true;

        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            canJump = false;

            Vector3 newVelocity = rb.velocity;
            newVelocity.y = jumpSpeed;
            rb.velocity = newVelocity;

            ResetSpring();
        }
    }

    public void ResetSpring()
    {
        appearance.springRb.transform.localPosition = Vector3.zero;
        appearance.springRb.velocity = rb.velocity;
    }

    public static void IncreaseSize(float amount)
    {
        instance.appearance.dropSize += amount;

        instance.appearance.dropSize = Mathf.Clamp(instance.appearance.dropSize, 0f, 50f);
    }

    public static void DecreaseSize(float amount)
    {
        instance.appearance.dropSize -= amount;

        if (instance.appearance.dropSize <= 0)
            TransitionToCloud();
    }

    public static void TransitionToCloud()
    {
        PhaseManager.UpdatePhase(PhaseManager.Phase.Cloud, instance.rb.position, instance.rb.velocity);
    }

    public static void TransitionToWave()
    {
        PhaseManager.UpdatePhase(PhaseManager.Phase.Wave, instance.rb.position, instance.rb.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ocean"))
            TransitionToWave();
    }
}
