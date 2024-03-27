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
    // SERIALIZED VARIABLES
    [Header("Game Object and Mesh")]
    [SerializeField] public GameObject regularStateGO  = null;
    [SerializeField] public GameObject explosionStateGO = null;
    [SerializeField] private Transform explosionOrigion;

    [Header("Explosion Effect")]
    [SerializeField] private float explosionForce = 50;
    [SerializeField] private float explosionRadius = 20;
    [SerializeField] private float fragmentMass = 0.1f;

    [Header("Fragment Effect")]
    [SerializeField] private float fragmentDisappearTime = 10f;
    [SerializeField] private float fragmentDisappearTimeVariation = 1f;
    
    [Header("Sequence Caused Explosion")]
    [SerializeField] private List<PCI_Explode> explodeBySequence = new List<PCI_Explode>();
        
    // PRIVATE VARIABLES
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

    private void Start()
    {
        foreach (Transform go in explosionStateGO.transform)
        {
            go.gameObject.layer = LayerMask.NameToLayer("Explodable"); //"Explodable";
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
        GetComponent<BoxCollider>().enabled = false;
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


/*
 * 1. origional collider still here after playeffect
 * 2. diable fragments collider with player
 */
