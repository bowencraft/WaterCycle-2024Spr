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
    
}
