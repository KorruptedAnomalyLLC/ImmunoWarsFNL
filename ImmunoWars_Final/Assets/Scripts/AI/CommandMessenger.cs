using UnityEngine;


/// <summary>
/// Relays Commands between components attatched to a unit
/// </summary>
public class CommandMessenger : MonoBehaviour
{
    private LocalBlackboard _localBlackboard; //req

    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;
    }

    public void CallSelected()
    {
        //send commands to any components that have a selected function
        _localBlackboard._unitRoot.Selected();
        _localBlackboard._moveRoot.Selected();
    }

    public void CallDropped()
    {
        //Send commands to any components that have a dropped function
        _localBlackboard._unitRoot.Dropped();
        _localBlackboard._moveRoot.Dropped();
    }

    public void CallDead()
    {
        //Send commands to any components that have a dead function
    }

    public void CallActivate()
    {
        //Send commands to any /// that have an activate function
    }

    public void CallDeactivate()
    {
        //Send // to any // that have a deactivate function
    }
}
