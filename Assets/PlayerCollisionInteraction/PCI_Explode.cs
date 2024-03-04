using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PCI_Explode : PlayerCollisionInteraction
{
    [SerializeField] private GameObject regularStateGO  = null;
    [SerializeField] private GameObject explosionStateGO = null;
    [SerializeField] private Transform explosionOrigion;
    [SerializeField] private float explosionForce = 50;
    [SerializeField] private float explosionRadius = 20;
    [SerializeField] private float fragmentMass = 0.1f;
    [SerializeField,Header("Average disappear time")] private float fragmentDisappearTime = 10f;
    [SerializeField,Header("Difference between max and min disappear time")] private float fragmentDisappearTimeVariation = 1f;
    [SerializeField] private List<PCI_Explode> explodeBySequence = new List<PCI_Explode>();
        
    private Dictionary<PCI_Explode, bool> explodeSequenceTrack = new Dictionary<PCI_Explode, bool>();

    private UnityEvent<PCI_Explode> OnExplosionTrigger = new UnityEvent<PCI_Explode>();

    private void Awake()
    {
        if (explodeBySequence.Count != 0)
        {
            foreach (var VARIABLE in explodeBySequence)
            {
                explodeSequenceTrack.Add(VARIABLE,false);
                VARIABLE.OnExplosionTrigger.AddListener(CheckExplosion);
            }
        }
    }

    public void CheckExplosion(PCI_Explode orgExplosion)
    {
        if (explodeSequenceTrack.ContainsKey(orgExplosion))
        {
            explodeSequenceTrack[orgExplosion] = true;
            foreach (var VARIABLE in explodeSequenceTrack)
            {
                if(VARIABLE.Value == false) return;
            }
            PlayEffect();
        }
    }

    protected override void PlayEffect()
    {
        regularStateGO.SetActive(false);
        explosionStateGO.SetActive(true);
        Explode();
        OnExplosionTrigger.Invoke(this);
        base.PlayEffect();
    }

    private void Explode()
    {
        foreach (Transform go in explosionStateGO.transform)
        {
            Rigidbody rb = go.gameObject.AddComponent<Rigidbody>();
            rb.mass = fragmentMass * Random.Range(0.8f,1.2f);
            rb.AddExplosionForce(explosionForce, explosionOrigion.position,explosionRadius);
            go.gameObject.AddComponent<BoxCollider>();
            StartCoroutine(DelayDisappear(go.gameObject));
        }
    }

    IEnumerator DelayDisappear(GameObject targetGO)
    {
        yield return new WaitForSeconds(fragmentDisappearTime += fragmentDisappearTimeVariation* Random.Range(-0.5f,0.5f));
        targetGO.transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 1f);
        yield return new WaitForSeconds(1f);
        targetGO.SetActive(false);
    }
}
