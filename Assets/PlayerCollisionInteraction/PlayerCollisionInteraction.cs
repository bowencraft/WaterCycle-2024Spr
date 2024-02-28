using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerCollisionInteraction : MonoBehaviour
{
    public List<PlayerController.PlayerForm> playerFormRequirement = new List<PlayerController.PlayerForm>()
    {
        PlayerController.PlayerForm.Cloud,
        PlayerController.PlayerForm.Drop,
        PlayerController.PlayerForm.Ice,
        PlayerController.PlayerForm.Wave
    };
    public float playerSpeedRequirement = 0f;
    public PlayerSoundManager.SoundType soundToPlay = PlayerSoundManager.SoundType.NO_SOUND;
    [Header("READ ONLY DO NOT EDIT")] public bool hasTriggered = false;

    public void TriggerInteraction()
    {
        if (hasTriggered) return;
        PlayEffect();
        if (soundToPlay != PlayerSoundManager.SoundType.NO_SOUND)
        {
            PlayerSoundManager.Instance.PlaySound(soundToPlay);
        }
        hasTriggered = true;
    }

    public PlayerController.PlayerForm CheckPlayerForm()
    {
        return PlayerController.i.GetPlayerForm();
    }

    public float GetPlayerSpeed()
    {
        return PlayerController.i.GetPlayerSpeed();
    }

    protected virtual void PlayEffect()
    {
        throw new NotImplementedException();
    }
    

    protected void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && GetPlayerSpeed() >= playerSpeedRequirement && playerFormRequirement.Contains(CheckPlayerForm()))
        {
            TriggerInteraction();
        }
    }

    protected void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && GetPlayerSpeed() >= playerSpeedRequirement && playerFormRequirement.Contains(CheckPlayerForm()))
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
