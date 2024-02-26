using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerCollisionInteraction : MonoBehaviour
{
    public float playerSpeedRequirement = 0f;
    [Header("READ ONLY DO NOT EDIT")] public bool hasTriggered = false;

    public void TriggerInteraction()
    {
        if (hasTriggered) return;
        if (!CheckPlayerForm()) return;
        PlayEffect();
        hasTriggered = true;
    }

    public bool CheckPlayerForm()
    {
        return true;
    }

    public float GetPlayerSpeed()
    {
        return 1;
    }

    protected virtual void PlayEffect()
    {
        throw new NotImplementedException();
    }
    

    protected void OnCollisionEnter(Collision other)
    {
        print("colliision " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player") && GetPlayerSpeed() >= playerSpeedRequirement)
        {
            TriggerInteraction();
        }
    }

    protected void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TriggerInteraction();
        }
    }

    protected void Update()
    {
        if(Input.GetKey(KeyCode.P))
        {
            TriggerInteraction();
        }
    }
}
