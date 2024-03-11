using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    static UI_PlayerStats instance;
    public static UI_PlayerStats i
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UI_PlayerStats>();
            }
            return instance;
        }
    }

    [Header("Experience UI")] 
    [SerializeField] private float animationMinY;
    [SerializeField] private float animationMaxY;
    [SerializeField] private RectTransform animationRect;

    [Header("Collectibles Count")]
    [SerializeField] private TMP_Text collectibleCountTMPText;

    public void UpdateExperienceDisplay(float ratio)
    {
        print("ratio is " + ratio);
        
        print("min y is " + animationMinY );
        
        animationRect.localPosition = new Vector3(0,
            (animationMaxY-animationMinY) * ratio + animationMinY,
            0);
    }
    
    public void UpdateCollectibleCount(int gainedAmount, int totalAmount)
    {
        collectibleCountTMPText.text = "Collectibles: " + gainedAmount + "/" + totalAmount;
    }
}
