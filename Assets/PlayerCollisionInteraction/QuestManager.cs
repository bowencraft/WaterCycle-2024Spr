using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    static QuestManager instance;
    public static QuestManager i
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<QuestManager>();
            }
            return instance;
        }
    }

    public UnityEvent<string> PCIFinished = new UnityEvent<string>();
    public List<GameObject> StageContainers = new List<GameObject>();
    public int[] stageQuestCount;

    private int currentStage = 0;

    private void Awake()
    {
        stageQuestCount = new int[StageContainers.Count];
        
        int currentAssign = -1;
        foreach (GameObject go in StageContainers)
        {
            currentAssign++;
            stageQuestCount[currentAssign] = 0;
            foreach (SingleQuest sq in go.GetComponentsInChildren<SingleQuest>())
            {
                sq.SetUp(currentAssign);
                stageQuestCount[currentAssign]++;
            }
        }
        
        foreach (GameObject go in StageContainers)
        {
            go.SetActive(false);
        }
        
        StageContainers[currentStage].SetActive(true);
    }

    public void FinishedQuestOfStage(int stage)
    {
        stageQuestCount[stage]--;
        if (stageQuestCount[stage] == 0)
        {
            StageContainers[currentStage].SetActive(false);
            currentStage++;
            if (currentStage < StageContainers.Count)
            {
                StageContainers[currentStage].SetActive(true);
            }
        }
    }
}
