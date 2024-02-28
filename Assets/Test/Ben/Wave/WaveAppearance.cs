using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAppearance : MonoBehaviour
{
    private static readonly int shaderParamCenter = Shader.PropertyToID("_Center");

    private static readonly int shaderParamHeight = Shader.PropertyToID("_Height");
    private static readonly int shaderParamSpread = Shader.PropertyToID("_Spread");
    private static readonly int shaderParamFalloff = Shader.PropertyToID("_Falloff");

    private static readonly int shaderParamDirection = Shader.PropertyToID("_Direction");
    private static readonly int shaderParamBiasMin = Shader.PropertyToID("_Bias_Min");
    private static readonly int shaderParamBiasMax = Shader.PropertyToID("_Bias_Max");

    private static WaveAppearance instance;

    [Header("References")]
    [SerializeField] public WaveController controller;
    [SerializeField] private MeshRenderer[] meshRenderers;

    [Header("Phase Size")]
    [SerializeField] public float waveSize;
    [SerializeField] public float maxWaveSize;

    [Header("Wave Height")]
    [SerializeField] private float heightScaleMin;
    [SerializeField] private float heightScaleMax;
    [SerializeField] private AnimationCurve heightSpeedScaling;
    [SerializeField] private AnimationCurve heightSizeScaling;

    public float height;

    [Header("Wave Spread")]
    [SerializeField] private float spreadScaleMin;
    [SerializeField] private float spreadScaleMax;
    [SerializeField] private AnimationCurve spreadSpeedScaling;
    [SerializeField] private AnimationCurve spreadSizeScaling;

    public float spread;

    [Header("Wave Falloff")]
    [SerializeField] [Range(0f, 3f)] private float falloffScaleMin;
    [SerializeField] [Range(0f, 3f)] private float falloffScaleMax;
    [SerializeField] private AnimationCurve falloffSpeedScaling;
    [SerializeField] private AnimationCurve falloffSizeScaling;

    public float falloff;

    [Header("Wave Bias")]
    [SerializeField] public Vector2 waveDirection;
    [SerializeField] private float biasScaleMin;
    [SerializeField] private float biasScaleMax;

    private Material material;

    private void Awake()
    {
        instance = this;

        material = meshRenderers[0].material;

        foreach (MeshRenderer meshRender in meshRenderers)
            meshRender.material = material;
    }

    private void Update()
    {
        Vector2 waveCenter;
        waveCenter.x = controller.rb.position.x;
        waveCenter.y = controller.rb.position.z;

        material.SetVector(shaderParamCenter, waveCenter);


        float speedRatio = Mathf.InverseLerp(0f, controller.maxSpeed, controller.rb.velocity.magnitude);
        float sizeRatio = Mathf.InverseLerp(0f, maxWaveSize, waveSize);

        float heightRatio = heightSpeedScaling.Evaluate(speedRatio) * heightSizeScaling.Evaluate(sizeRatio);
        height = Mathf.Lerp(heightScaleMin, heightScaleMax, heightRatio);

        float spreadRatio = spreadSpeedScaling.Evaluate(speedRatio) * spreadSizeScaling.Evaluate(sizeRatio);
        spread = Mathf.Lerp(spreadScaleMin, spreadScaleMax, spreadRatio);

        float falloffRatio = falloffSpeedScaling.Evaluate(speedRatio) * falloffSizeScaling.Evaluate(sizeRatio);
        falloff = Mathf.Lerp(falloffScaleMin, falloffScaleMax, 1 - falloffRatio);

        material.SetFloat(shaderParamHeight, height);
        material.SetFloat(shaderParamSpread, spread);
        material.SetFloat(shaderParamFalloff, falloff);


        waveDirection.x = controller.rb.velocity.x;
        waveDirection.y = controller.rb.velocity.z;

        material.SetVector(shaderParamDirection, waveDirection.normalized);
        material.SetFloat(shaderParamBiasMin, biasScaleMin);
        material.SetFloat(shaderParamBiasMax, biasScaleMax);
    }

    public static float GetOceanHeight(Vector3 position)
    {
        Vector2 pos = new Vector2(position.x, position.z);
        
        Vector2 center = instance.material.GetVector(shaderParamCenter);
        Vector2 direction = instance.material.GetVector(shaderParamDirection);
        float height = instance.material.GetFloat(shaderParamHeight);
        float spread = instance.material.GetFloat(shaderParamSpread);
        float falloff = instance.material.GetFloat(shaderParamFalloff);
        float biasMin = instance.material.GetFloat(shaderParamBiasMin);
        float biasMax = instance.material.GetFloat(shaderParamBiasMax);

        float distance = Vector2.Distance(pos, center);

        float biasedSpread = Vector2.Dot(direction, (pos - center).normalized);
        biasedSpread = spread * Mathf.Lerp(biasMin, biasMax, -Mathf.InverseLerp(-1f, 1f, biasedSpread));

        float spreadDistance = distance / biasedSpread;

        return height * Mathf.Cos(spreadDistance) * Mathf.Pow(0.5f, falloff * spreadDistance);
    }
}
