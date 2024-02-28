using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public abstract class SystemsManager : MonoBehaviour
{
    [SerializeField] protected CinemachineFreeLook cam;

    public virtual float CurrentSize { get { return 0f; } }

    public virtual void Activate(Vector3 position, Vector3 velocity) { }
    public virtual void Deactivate() { }
    public virtual Vector3 GetPosition() { return Vector3.zero; }
    public virtual void SetPosition(Vector3 position) { }

    public virtual Vector2 GetCamRotation()
    {
        Vector2 rotation;
        rotation.x = cam.m_XAxis.Value;
        rotation.y = cam.m_YAxis.Value;

        return rotation;
    }

    public virtual void SetCamRotation(Vector2 rotation)
    {
        cam.m_XAxis.Value = rotation.x;
        cam.m_YAxis.Value = rotation.y;
    }
}
