using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCI_Collectible : PlayerCollisionInteraction
{
    [SerializeField] private Transform collectibleGameObject;
    protected override void PlayEffect()
    {
        collectibleGameObject.gameObject.SetActive(false);
    }
}
