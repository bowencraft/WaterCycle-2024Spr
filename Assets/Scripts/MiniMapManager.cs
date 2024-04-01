using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    [SerializeField] private GameObject miniMapGO;
    [SerializeField] private KeyCode triggerKeyCode;

    private void Update()
    {
        
        if (Input.GetKeyDown(triggerKeyCode))
        {
            miniMapGO.SetActive(true);
        }else if (Input.GetKeyUp(triggerKeyCode))
        {
            miniMapGO.SetActive(false);
        }
    }
}
