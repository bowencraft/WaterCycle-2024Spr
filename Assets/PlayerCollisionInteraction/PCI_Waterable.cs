using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCI_Waterable : PlayerCollisionInteraction
{
    [SerializeField] private GameObject beforeWateringGameObject;
    [SerializeField] private GameObject afterWateringGameObject;
    [SerializeField] private ParticleSystem transitionParticleSystem;
    [SerializeField] private float animationDelay = 1f;

    protected override void PlayEffect()
    {
        transitionParticleSystem.Play();
    }

    IEnumerator DelayTransition(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        beforeWateringGameObject.SetActive(false);
        afterWateringGameObject.SetActive(true);
    }
    
}
