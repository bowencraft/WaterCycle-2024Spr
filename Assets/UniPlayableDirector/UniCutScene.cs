using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class UniPlayableDirector : MonoBehaviour, IPerformableAction
{
    private UnityEvent _onActionStarts;
    private UnityEvent _onActionCompletes;
    private PlayableDirector myPD;

    public void PerformAction()
    {
        myPD = GetComponent<PlayableDirector>();
        myPD.stopped += OnPlayableDirectorStopped;
        myPD.Play();
    }

    public bool IsAssigned()
    {
        return true;
    }
    
    void OnPlayableDirectorStopped(PlayableDirector myPlayableDirector)
    {
        if(myPlayableDirector == myPD) onActionCompletes.Invoke();
    }

    public UnityEvent onActionStarts
    {
        get { return _onActionStarts; }
    }

    public UnityEvent onActionCompletes {
        get { return _onActionCompletes; }
    }
}
