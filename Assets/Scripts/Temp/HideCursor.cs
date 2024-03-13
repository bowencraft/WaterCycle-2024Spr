using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState=CursorLockMode.Locked;
        if (_camera == null)
        {
            _camera = GameObject.FindObjectOfType<CinemachineFreeLook>();
        }
    }
    
    public float zoomSpeed = 1f; // 控制缩放速度
    public float minSize = 20f; // 最小视野大小
    public float maxSize = 280f; // 最大视野大小

    public CinemachineFreeLook _camera;

    // Update is called once per frame
    void Update()
    {
        // 获取鼠标滚轮的滚动量
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        // 根据滚动方向调整相机的size属性
        _camera.m_Lens.OrthographicSize -= scroll * zoomSpeed;
        // 限制相机的size在最小值和最大值之间
        _camera.m_Lens.OrthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minSize, maxSize);
    }
}
