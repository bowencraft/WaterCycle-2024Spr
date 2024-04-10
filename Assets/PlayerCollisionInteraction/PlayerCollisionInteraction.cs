using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerCollisionInteraction : MonoBehaviour
{
    //SERIALIZED VARIABLES
    [Header("Player Requirement")]
    public List<PlayerController.PlayerForm> playerFormRequirement = new List<PlayerController.PlayerForm>()
    {
        PlayerController.PlayerForm.Cloud,
        PlayerController.PlayerForm.Drop,
        PlayerController.PlayerForm.Ice,
        PlayerController.PlayerForm.Wave
    };
    public float playerSpeedRequirement = 0f;
    
    [Header("Building Health Related")]
    [SerializeField] protected int interactionAmountRequired = 1;
    [SerializeField] private bool unlimitedAmountInteraction = false;
    [SerializeField] private bool onStayInteraction = false;

    [Header("Trigger Related")] 
    public PlayerSoundManager.SoundType soundToPlay = PlayerSoundManager.SoundType.NO_SOUND;
    [SerializeField] float rewardValue = 1f;

    [SerializeField] private string interactionID = "not set";
    //public LevelManager.RewardType interactionReward = LevelManager.RewardType.InteractionPoint;

    [Header("READ ONLY - DO NOT EDIT")] public bool hasTriggered = false;

    // PRIVATE VARIABLES
    public bool canTriggerNewInteractionCount = true;
    
    public virtual void TriggerInteraction()
    {
        if (hasTriggered && !unlimitedAmountInteraction) return;
        PlayEffect();
        if (soundToPlay != PlayerSoundManager.SoundType.NO_SOUND)
        {
            PlayerSoundManager.Instance.PlaySound(soundToPlay);
        }
        hasTriggered = true;
        
        LevelManager.i.GainExperience(rewardValue);
        
        if(!interactionID.Equals("not set")) QuestManager.i.PCIFinished.Invoke(interactionID);
    }

    public PlayerController.PlayerForm CheckPlayerForm()
    {
        return PlayerController.i.GetPlayerForm();
    }

    public float GetPlayerSpeed()
    {
        return PlayerController.i.GetPlayerSpeed();
    }

    /// <summary>
    /// When overriding, call base.PlayEffect() at the end of new method.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void PlayEffect()
    {
        //LevelManager.i.GainReward(interactionReward);
    }
    
    // ON TRIGGER

    private void OnTriggerEnter(Collider other)
    {
        if(!unlimitedAmountInteraction && !canTriggerNewInteractionCount) return;
        if (other.gameObject.CompareTag("Player"))
        {
            CheckTriggerInteraction();
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(!onStayInteraction) return;
        if(!unlimitedAmountInteraction && !canTriggerNewInteractionCount) return;
        if (other.gameObject.CompareTag("Player"))
        {
            CheckTriggerInteraction();
        }
    }

    // ON COLLISION
    
    private void OnCollisionEnter(Collision other)
    {
        if(!unlimitedAmountInteraction && !canTriggerNewInteractionCount) return;
        if (other.gameObject.CompareTag("Player"))
        {
            CheckTriggerInteraction();
        }    
    }

    private void OnCollisionStay(Collision other)
    {
        if(!onStayInteraction) return;
        if(!unlimitedAmountInteraction && !canTriggerNewInteractionCount) return;
        if (other.gameObject.CompareTag("Player"))
        {
            CheckTriggerInteraction();
        }    
    }

    private void OnCollisionExit(Collision other)
    {
        StartCoroutine(DelayBeforeAllowTrigger(0.5f));
    }


    protected void CheckTriggerInteraction()
    {
        print("try 1");
        if (GetPlayerSpeed() >= playerSpeedRequirement && playerFormRequirement.Contains(CheckPlayerForm()))
        {
            interactionAmountRequired--;
            canTriggerNewInteractionCount = false;
            if (interactionAmountRequired == 0 || unlimitedAmountInteraction)
            {
                print("try 2");
                TriggerInteraction();
            }
            else
            {
                print("try 3");

                InteractionButNotTriggered();
            }
        }
    }

    protected virtual void InteractionButNotTriggered()
    {
        
    }

    IEnumerator DelayBeforeAllowTrigger(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);
        canTriggerNewInteractionCount = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        throw new NotImplementedException();
    }

    protected void OnTriggerExit(Collider other)
    {
        StartCoroutine(DelayBeforeAllowTrigger(0.5f));
    }

    /*
    protected void OnCollisionEnter(Collision other)
    {
        print(other.gameObject.name + " ENTERED" );
        if(!canTriggerNewInteractionCount) return;
        if (other.gameObject.CompareTag("Player") && GetPlayerSpeed() >= playerSpeedRequirement && playerFormRequirement.Contains(CheckPlayerForm()))
        {
            interactionAmountRequired--;
            canTriggerNewInteractionCount = false;
            if (playerSpeedRequirement == 0)
            {
                TriggerInteraction();
            }
        }
    }

    protected void OnCollisionStay(Collision other)
    {
        if(!canTriggerNewInteractionCount) return;
        if (other.gameObject.CompareTag("Player") && GetPlayerSpeed() >= playerSpeedRequirement && playerFormRequirement.Contains(CheckPlayerForm()))
        {
            interactionAmountRequired--;
            canTriggerNewInteractionCount = false;
            if (playerSpeedRequirement == 0)
            {
                TriggerInteraction();
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        canTriggerNewInteractionCount = true;
    }*/

    protected void Update()
    {
        if(Input.GetKey(KeyCode.P))
        {
            TriggerInteraction();
        }
    }
}
