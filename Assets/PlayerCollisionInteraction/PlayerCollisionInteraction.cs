using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerCollisionInteraction : MonoBehaviour
{
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

    protected virtual void PlayEffect()
    {
        throw new NotImplementedException();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            TriggerInteraction();
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            TriggerInteraction();
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.P))
        {
            TriggerInteraction();
        }
    }
}
