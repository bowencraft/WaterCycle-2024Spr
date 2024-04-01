using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterParticles : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        // Get the ParticleSystem component attached to this GameObject
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Check if the ParticleSystem has stopped and if it has no particles left
        if (!particleSystem.IsAlive())
        {
            // Destroy this GameObject
            Destroy(gameObject);
        }
    }
}
