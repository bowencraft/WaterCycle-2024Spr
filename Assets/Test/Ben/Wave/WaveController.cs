using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    private static WaveController instance;

    [Header("References")]
    [SerializeField] public WaveAppearance appearance;
    [SerializeField] public Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float accelerationRate;
    [SerializeField] private float decelerationRate;
    [SerializeField] public float maxSpeed;

    [Space]
    [SerializeField] private float dashAccelerationRate;
    [SerializeField] private float dashDecelerationRate;
    [SerializeField] private float dashMaxSpeed;

    [Space]
    [SerializeField] private AnimationCurve decelerationSizeScaling;

    [Space]
    [SerializeField] private float massGainRate;
    [SerializeField] private float massLossRate;

    [Header("Land Detection")]
    [SerializeField] private float massLossRateOnLand;
    [SerializeField] private float collisionHeightThreshold;
    [SerializeField] private float checkHeight;

    private Vector3 prevPos;
    private Vector3 currentPos;

    [HideInInspector] public bool active = false;

    private void Awake()
    {
        instance = this;
        active = true;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        prevPos = currentPos;

        ProcessMovement();
        ProcessMassChange();

        currentPos = transform.position;
        currentPos.y = appearance.height - 1;
        transform.position = currentPos;
    }

    private void ProcessMovement()
    {
        Vector3 flatCamForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 flatCamRight = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;

        Vector3 horizontalInput = Vector3.zero;

        if (active)
        {
            horizontalInput.x = Input.GetAxisRaw("Horizontal");
            horizontalInput.y = Input.GetAxisRaw("Vertical");
        }

        Vector3 moveDirection = (flatCamForward * horizontalInput.y + flatCamRight * horizontalInput.x).normalized;

        bool dashing = active && Input.GetKey(KeyCode.Space);

        float decelRate = rb.velocity.magnitude > maxSpeed ? dashDecelerationRate : decelerationRate;

        Vector3 deceleration = -rb.velocity.normalized * decelRate * Time.fixedDeltaTime;
        deceleration *= decelerationSizeScaling.Evaluate(Mathf.InverseLerp(0f, appearance.maxWaveSize, appearance.waveSize));
        deceleration.y = 0;
        deceleration = Vector3.ClampMagnitude(deceleration, rb.velocity.magnitude);

        if (horizontalInput == Vector3.zero || (!dashing && rb.velocity.magnitude > maxSpeed))
            rb.velocity += deceleration;


        float accelRate = dashing ? dashAccelerationRate : accelerationRate;
        Vector3 acceleration = moveDirection * accelRate * Time.fixedDeltaTime;
        acceleration.y = 0;

        float speedCap = dashing ? dashMaxSpeed : maxSpeed;
        Vector3 clampedVelocity = rb.velocity + acceleration;
        clampedVelocity = Vector3.ClampMagnitude(clampedVelocity, speedCap);

        if (!dashing && rb.velocity.magnitude > maxSpeed)
            rb.velocity += acceleration;
        else
            rb.velocity = clampedVelocity;
    }

    private void ProcessMassChange()
    {
        if (rb.velocity.magnitude >= maxSpeed)
            appearance.waveSize += massGainRate * Time.fixedDeltaTime;
        else if (rb.velocity.magnitude < maxSpeed * 0.5f)
            appearance.waveSize -= massLossRate * Time.fixedDeltaTime;

        if (active)
        {
            Vector3 checkOrigin = rb.position + Vector3.up * checkHeight;
            bool hit = Physics.Raycast(checkOrigin, Vector3.down, out RaycastHit hitInfo);

            if (hit && !hitInfo.collider.CompareTag("Ocean"))
            {
                appearance.waveSize -= massLossRateOnLand * Time.fixedDeltaTime;

                float heightDelta = hitInfo.point.y - rb.position.y;

                if (heightDelta > 0 && heightDelta <= collisionHeightThreshold)
                    TransitionToRaindrop(prevPos, Vector3.ProjectOnPlane(rb.velocity, Vector3.up));
            }
        }

        appearance.waveSize = Mathf.Clamp(appearance.waveSize, 0f, appearance.maxWaveSize);
    }

    public static void IncreaseSize(float amount)
    {
        Debug.Log("Increase by " + amount);
    }

    public static void DecreaseSize(float amount)
    {
        Debug.Log("Decrease by " + amount);

        // if amount goes below 0, transition to cloud
    }

    public static void TransitionToRaindrop(Vector3 position, Vector3 velocity)
    {
        PhaseManager.UpdatePhase(PhaseManager.Phase.Raindrop, position, velocity);
    }
}
