using UnityEngine.Events;

public interface IPerformableAction
{
    public void PerformAction();
    public bool IsAssigned();
    public UnityEvent onActionStarts { get;}
    public UnityEvent onActionCompletes { get;}

}