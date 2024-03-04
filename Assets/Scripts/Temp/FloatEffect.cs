using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float amplitude = 0.5f; // 浮动的幅度
    public float frequency = 1f; // 浮动的频率

    // 原始位置
    private Vector3 startPosition;

    void Start()
    {
        // 记录初始位置
        startPosition = transform.position;
    }

    void Update()
    {
        // 根据时间改变物体的位置，创建上下浮动的效果
        float tempPos = amplitude * Mathf.Sin(Time.time * frequency);
        transform.position = startPosition + new Vector3(0, tempPos, 0);
    }
}
