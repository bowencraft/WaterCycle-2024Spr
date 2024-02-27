using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    private static CloudController instance;

    [Header("References")]
    [SerializeField] public CloudAppearance appearance;
    [SerializeField] public Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float accelerationRateHorizontal;
    [SerializeField] private float maxSpeedHorizontal;

    [Space]
    [SerializeField] private float accelerationRateVertical;
    [SerializeField] private float maxSpeedVertical;

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
    }

    private void ProcessMovement()
    {
        // horizontal
        Vector3 flatCamForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 flatCamRight = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;

        Vector3 horizontalInput = Vector3.zero;
        horizontalInput.x = Input.GetAxisRaw("Horizontal");
        horizontalInput.y = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = (flatCamForward * horizontalInput.y + flatCamRight * horizontalInput.x).normalized;

        Vector3 horizontalVelocity = rb.velocity + moveDirection * accelerationRateHorizontal * Time.fixedDeltaTime;
        horizontalVelocity.y = 0;
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedHorizontal);

        // vertical
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.E)) verticalInput += 1;
        if (Input.GetKey(KeyCode.Q)) verticalInput -= 1;

        Vector3 verticalVelocity = new Vector3(0, rb.velocity.y + verticalInput * accelerationRateVertical * Time.fixedDeltaTime);
        verticalVelocity = Vector3.ClampMagnitude(verticalVelocity, maxSpeedVertical);


        rb.velocity = horizontalVelocity + verticalVelocity;
    }

    public static void IncreaseSize(float amount)
    {
        Debug.Log("Increase by " + amount);
    }

    public static void DecreaseSize(float amount)
    {
        Debug.Log("Decrease by " + amount);

        // if amount goes below 0, transition to raindrop
    }

    public static void TransitionToRaindrop(Vector3 position, Vector3 velocity)
    {
        PhaseManager.UpdatePhase(PhaseManager.Phase.Raindrop, position, velocity);
    }
}
