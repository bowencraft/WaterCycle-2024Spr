using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudElement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CloudAppearance appearance;
    [SerializeField] private Rigidbody target;
    [SerializeField] public Rigidbody self;

    [Header("Movement")]
    [SerializeField] private Vector3 targetOffset;
    [SerializeField] private float moveAcceleration;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float breakDistance;

    [Header("Cloud Particles")]
    [SerializeField] private ParticleSystem cloudParticles;
    [SerializeField] private float cloudEmissionSpeedFactor;

    [Space]
    [SerializeField] private float cloudLifetimeMin;
    [SerializeField] private float cloudLifetimeMax;

    [Space]
    [SerializeField] private float cloudEmissionRateMin;
    [SerializeField] private float cloudEmissionRateMax;

    [Header("Rain Particles")]
    [SerializeField] private ParticleSystem rainParticles;

    public void Setup(CloudAppearance appearance, Rigidbody target)
    {
        this.appearance = appearance;
        this.target = target;
    }

    public void Setup(CloudAppearance appearance, Rigidbody target, Vector3 targetOffset)
    {
        this.appearance = appearance;
        this.target = target;
        this.targetOffset = targetOffset;

        self.position = target.position + targetOffset;
    }

    public void SetOffset(Vector3 targetOffset)
    {
        this.targetOffset = targetOffset;
    }

    private void Update()
    {
        UpdateMovement();
        UpdateParticles();

        // if (appearance != null && Vector3.Distance(target.position, self.position) > breakDistance)
        //     appearance.RemoveElement(this);
    }

    private void UpdateMovement()
    {
        Vector3 moveDirection = (target.position + targetOffset - self.position).normalized;

        self.velocity += moveDirection * moveAcceleration * Time.deltaTime;
        self.velocity = Vector3.ClampMagnitude(self.velocity, moveSpeed);
    }

    private void UpdateParticles()
    {
        ParticleSystem.MainModule main = cloudParticles.main;
        ParticleSystem.EmissionModule emission = cloudParticles.emission;

        if (target.velocity.magnitude > 0)
            cloudParticles.transform.forward = target.velocity;

        main.startSpeedMultiplier = Mathf.Lerp(target.velocity.magnitude, self.velocity.magnitude, 0.5f) * cloudEmissionSpeedFactor;

        float elementSpeedRatio = Mathf.InverseLerp(0f, moveSpeed, self.velocity.magnitude);

        main.startLifetimeMultiplier = Mathf.Lerp(cloudLifetimeMin, cloudLifetimeMax, 1 - elementSpeedRatio);
        emission.rateOverTimeMultiplier = Mathf.Lerp(cloudEmissionRateMin, cloudEmissionRateMax, elementSpeedRatio);
    }

    public void StartRaining()
    {
        rainParticles.Play();
    }

    public void StopRaining()
    {
        rainParticles.Stop();
    }

    public void Death()
    {
        cloudParticles.Stop();
        rainParticles.Stop();

        enabled = false;
        Destroy(gameObject, 5f);
    }
}
