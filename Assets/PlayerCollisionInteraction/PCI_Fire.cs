using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCI_Fire : PlayerCollisionInteraction
{
    [SerializeField] private ParticleSystem fireParticleSystem;

    protected override void PlayEffect()
    {
        fireParticleSystem.Stop();
        base.PlayEffect();
    }
}
