using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaterTrail : MonoBehaviour
{
    public GameObject objectToSpawn;

    void Start()
    {
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Terrain")
        {
            Debug.Log("here");
            Vector3 closestPoint = other.ClosestPoint(transform.position);
            Instantiate(objectToSpawn, closestPoint, transform.rotation);
        }
    }
}
