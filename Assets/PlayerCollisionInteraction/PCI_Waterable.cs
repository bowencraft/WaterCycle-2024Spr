using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PCI_Waterable : PlayerCollisionInteraction
{
    [SerializeField] private GameObject beforeWateringGameObject;
    [SerializeField] private GameObject afterWateringGameObject;
    [SerializeField] private ParticleSystem transitionParticleSystem;
    [SerializeField] private float animationDuration = 2f;
    [SerializeField] private float particleSystemDelayDestroy = 0f;

    protected override void PlayEffect()
    {
        StartCoroutine(DelayTransition(animationDuration));
    }

    IEnumerator DelayTransition(float delayTime)
    {
        transitionParticleSystem.Play();
        beforeWateringGameObject.transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), delayTime / 2);
        
        yield return new WaitForSeconds(delayTime / 2);
        
        beforeWateringGameObject.SetActive(false);
        afterWateringGameObject.SetActive(true);
        Vector3 targetLocalScale = afterWateringGameObject.transform.localScale;
        afterWateringGameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        afterWateringGameObject.transform.DOScale(targetLocalScale, delayTime / 2);
        
        yield return new WaitForSeconds(delayTime / 2 + particleSystemDelayDestroy);
        
        transitionParticleSystem.Stop();
        base.PlayEffect();
    }
    
}
