using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCI_FatalWater : PlayerCollisionInteraction
{
    [SerializeField] private float bufferTime = 2f;
    [SerializeField] private float currentTime = 0f;
    
    protected override void PlayEffect()
    {
        print("player death");
        PlayerControllerManager.Instance.Respawn();
        base.PlayEffect();
    }

}
