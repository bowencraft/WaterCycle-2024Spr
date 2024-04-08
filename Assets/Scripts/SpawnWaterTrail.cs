using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaterTrail : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnCooldown = 1f; // Cooldown time in seconds

    private float timeSinceLastSpawn = 0f; // Timer to track cooldown

    public float spawnPositionOffset = 1f;

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
        if (other.tag == "Terrain" && timeSinceLastSpawn >= spawnCooldown)
        {
            Debug.Log("Spawning Object");
            // Reset timer
            timeSinceLastSpawn = 0f;

            // Calculate spawn position
            //Vector3 spawnPosition = transform.position - new Vector3(0, GetComponent<Renderer>().bounds.extents.y / 2, 0);
            Vector3 spawnPosition = transform.position - new Vector3(0, spawnPositionOffset, 0);
            // Instantiate the object
            Instantiate(objectToSpawn, spawnPosition, transform.rotation);
        }
    }
}
