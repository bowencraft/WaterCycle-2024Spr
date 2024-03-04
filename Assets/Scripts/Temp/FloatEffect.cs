using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float amplitude = 0.5f; // �����ķ���
    public float frequency = 1f; // ������Ƶ��

    // ԭʼλ��
    private Vector3 startPosition;

    void Start()
    {
        // ��¼��ʼλ��
        startPosition = transform.position;
    }

    void Update()
    {
        // ����ʱ��ı������λ�ã��������¸�����Ч��
        float tempPos = amplitude * Mathf.Sin(Time.time * frequency);
        transform.position = startPosition + new Vector3(0, tempPos, 0);
    }
}
