using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCI_Explode : PlayerCollisionInteraction
{
    [SerializeField] private GameObject regularStateGO  = null;
    [SerializeField] private GameObject explosionStateGO = null;
    [SerializeField] private Transform explosionOrigion;
    [SerializeField] private float explosionForce = 50;
    [SerializeField] private float explosionRadius = 20;
    [SerializeField] private float fragmentMass = 0.1f;

    protected override void PlayEffect()
    {
        regularStateGO.SetActive(false);
        explosionStateGO.SetActive(true);
        Explode();
    }

    private void Explode()
    {
        
        foreach (Transform go in explosionStateGO.transform)
        {
            Rigidbody rb = go.gameObject.AddComponent<Rigidbody>();
            rb.mass = fragmentMass * Random.Range(0.8f,1.2f);
            rb.AddExplosionForce(explosionForce, explosionOrigion.position,explosionRadius);
            go.gameObject.AddComponent<BoxCollider>();
        }

        //StartCoroutine(DelayAddCollider());

    }

    /*
    IEnumerator DelayAddCollider()
    {
        yield return new WaitForSeconds(.7f);

        foreach (Transform go in explosionStateGO.transform)
        {
            go.gameObject.AddComponent<BoxCollider>();
        }
    }*/
}
