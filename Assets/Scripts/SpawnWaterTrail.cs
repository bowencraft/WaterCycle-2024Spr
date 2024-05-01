using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnWaterTrail : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnCooldown = 1f; // Cooldown time in seconds

    private float timeSinceLastSpawn = 0f; // Timer to track cooldown

    public float spawnPositionOffset = 1f;

    public UnityEvent playSoundAction;

    private bool grounded = false;

    void Update()
    {
        // Update the timer every frame
        if (timeSinceLastSpawn < spawnCooldown)
        {
            timeSinceLastSpawn += Time.deltaTime;
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
        // Check if cooldown has completed and if the collider is the Terrain
        if (other.tag == "Terrain" )
        {
            if (timeSinceLastSpawn >= spawnCooldown)
            {
                // Reset timer
                timeSinceLastSpawn = 0f;

                // Calculate spawn position
                //Vector3 spawnPosition = transform.position - new Vector3(0, GetComponent<Renderer>().bounds.extents.y / 2, 0);
                Vector3 spawnPosition = transform.position + new Vector3(0, spawnPositionOffset, 0);
                // Instantiate the object
                Instantiate(objectToSpawn, spawnPosition, transform.rotation);

            }
            
            
            if (!grounded)
            {
                print("Grounded!");
                grounded = true;
                playSoundAction.Invoke();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Terrain")
        {
            grounded = false;
        }
    }
}
