using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsSwitch : MonoBehaviour
{
    [SerializeField] private GameObject iceFormTip;
    [SerializeField] private GameObject waterFormTip;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!SkillManager.i.GetSkillUnlocked(0) || PlayerControllerManager.Instance.PlayerForm == PlayerController.PlayerForm.Ice)
        {
            iceFormTip.SetActive(true);
            waterFormTip.SetActive(false);
        }
        else
        {
            iceFormTip.SetActive(false);
            waterFormTip.SetActive(true);
        }
    }
}
