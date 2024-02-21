using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCI_Waterable : PlayerCollisionInteraction
{
    [SerializeField] private GameObject beforeWateringGameObject;
    [SerializeField] private GameObject afterWateringGameObject;
    [SerializeField] private ParticleSystem transitionParticleSystem;
    [SerializeField] private float animationDuration = 2f;

    protected override void PlayEffect()
    {
        StartCoroutine(DelayTransition(animationDuration));
    }

    IEnumerator DelayTransition(float delayTime)
    {
        transitionParticleSystem.Play();
        yield return new WaitForSeconds(delayTime);
        beforeWateringGameObject.SetActive(false);
        afterWateringGameObject.SetActive(true);
        transitionParticleSystem.Stop();
    }
    
}
