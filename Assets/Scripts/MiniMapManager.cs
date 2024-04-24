using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    [SerializeField] private GameObject miniMapGO;
    [SerializeField] private KeyCode triggerKeyCode;
    
    static MiniMapManager instance;
    public static MiniMapManager i
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<MiniMapManager>();
            }
            return instance;
        }
    }
    
    public void OpenMiniMap()
    {
        miniMapGO.SetActive(true);
    }

    public void CloseMiniMap()
    {
        miniMapGO.SetActive(false);
    }
}
