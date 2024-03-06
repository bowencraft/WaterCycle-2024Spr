using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] float animationMinY, animationMaxY;
    [SerializeField] private RectTransform animationRect;
}
