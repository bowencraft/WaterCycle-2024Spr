using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class SingleQuest : MonoBehaviour
{
    private TMP_Text myTMP;
    [SerializeField] private string requiredInteractionID = "nah";
    [SerializeField] private int currentInteractionCount = 0;
    [SerializeField] private int requiredInteractionCount = 2;
    [SerializeField] private string questDescription = "Do this thing. ";

    private void Start()
    {
        myTMP = GetComponent<TMP_Text>();
        UpdateDisplay();
        QuestManager.i.PCIFinished.AddListener(ReceiveInteraction);
    }

    public void ReceiveInteraction(string interactionID)
    {
        if (interactionID.Equals(requiredInteractionID))
        {
            currentInteractionCount++;
            UpdateDisplay();
        }
    }

    public void UpdateDisplay()
    {

        myTMP.text = questDescription + " (" + currentInteractionCount + "/" + requiredInteractionCount + ")";
        
        if (currentInteractionCount == requiredInteractionCount)
        {
            StartCoroutine(FinishEffect()); //myTMP.color = Color.black;
        }
    }

    IEnumerator FinishEffect()
    {
        float timeLasted = 0f;
        float timeRequired = 0.5f;
        while (timeLasted < timeRequired)
        {
            timeLasted += Time.deltaTime;
            myTMP.color = Color.Lerp(Color.white, Color.black, timeLasted / timeRequired);
            yield return new WaitForSeconds(0);
        }
        myTMP.color = Color.black;

        
    }

}
