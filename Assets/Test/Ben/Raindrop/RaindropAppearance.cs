using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaindropAppearance : MonoBehaviour
{
    private static readonly int sSpringDirID = Shader.PropertyToID("_SpringDir");
    private static readonly int sWeightID = Shader.PropertyToID("_Weight");

    [Header("References")]
    [SerializeField] public RaindropController controller;
    [SerializeField] public Rigidbody springRb;
    [SerializeField] public SpringJoint springJoint;

    [Header("Config")]
    [SerializeField] public GameObject splashPrefab;
    [SerializeField] public float splashVelocityThreshold;

    [Header("Size")]
    [SerializeField] public float dropSize;

    [Space]
    [SerializeField] private float dropScaleMin;
    [SerializeField] private float dropScaleMax;

    [Space]
    [SerializeField] private float dropWeightMin;
    [SerializeField] private float dropWeightMax;

    [Space]
    [SerializeField] private float dropSpringMin;
    [SerializeField] private float dropSpringMax;

    private Material mMaterial;

    private void Awake()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        mMaterial = renderer.material;
        renderer.material = mMaterial;
    }

    private void Update()
    {
        Vector3 springDir = springRb.position - controller.rb.position;
        mMaterial.SetVector(sSpringDirID, springDir);

        float sizeRatio = Mathf.InverseLerp(0f, 50f, dropSize);

        float scale = Mathf.Lerp(dropScaleMin, dropScaleMax, sizeRatio);
        transform.localScale = Vector3.one * scale;

        float weight = Mathf.Lerp(dropWeightMin, dropWeightMax, sizeRatio);
        mMaterial.SetFloat(sWeightID, weight);

        springJoint.spring = Mathf.Lerp(dropSpringMin, dropSpringMax, 1 - sizeRatio);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (splashPrefab != null && collision.relativeVelocity.magnitude > splashVelocityThreshold)
        {
            Instantiate(splashPrefab, collision.contacts[0].point, Quaternion.identity);
        }
    }
}
