using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveInteractor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.tag == "Interactable")
        {
            // if (other.GetComponent<Interactable>() != null)
            // {
            //     other.GetComponent<Interactable>().Collide("Wave");
            // }
        }
    }
}