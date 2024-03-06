using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PCI_FormChange : PlayerCollisionInteraction
{
    [Header("Select Effect One")] 
    public bool directlyChangeForm = false;
    public PlayerController.PlayerForm changeFormTo = PlayerController.PlayerForm.Drop;

    [Header("Or Select Effect Two")] 
    [SerializeField] private bool changeSize = false;
    [SerializeField] private float sizeChangePerSecond = 1f;
    [SerializeField] private float sizeGrowMax = 5f;

    private void Awake()
    {
        if (directlyChangeForm == false && changeSize == false)
        {
            Debug.LogError("Make sure to select true for one of effect one OR effect two");
        }else if (directlyChangeForm == true && changeSize == true)
        {
            Debug.LogError("Only select true for one of effect one OR effect two");
        }
    }

    protected override void PlayEffect()
    {
        print("play effect");
        if (directlyChangeForm)
        {
            PlayerControllerManager.Instance.ChangePlayerForm(changeFormTo);
        }
        else
        {
            PlayerControllerManager.Instance.AdjustScale(sizeChangePerSecond*Time.deltaTime, sizeGrowMax);
        }
        base.PlayEffect();
    }
}
