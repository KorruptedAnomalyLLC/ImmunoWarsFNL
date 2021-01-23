using UnityEngine;


/// <summary>
/// Relays Commands between components attatched to a unit
/// </summary>
/// TODO: Need to place in Null Checks!!!
public class CommandMessenger : MonoBehaviour
{
    private LocalBlackboard _localBlackboard; //req

    #region Setup
    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;

        if (TryGetComponent(out MovementRoot temp))
        {
            _localBlackboard._moveRoot = temp;
            _localBlackboard._moveRoot.Setup(_localBlackboard);
        }

        if (TryGetComponent(out CombatRoot temp2))
        {
            _localBlackboard._combatRoot = temp2;
            _localBlackboard._combatRoot.Setup(_localBlackboard);
        }

        if (TryGetComponent(out VisionRoot temp3))
        {
            _localBlackboard._visionRoot = temp3;
            _localBlackboard._visionRoot.Setup(_localBlackboard);
        }

        if (TryGetComponent(out StatusManager temp4))
        {
            _localBlackboard._statusManager = temp4;
            _localBlackboard._statusManager.Setup(_localBlackboard);
        }
    }
    #endregion


    public void CallSelected()
    {
        //send commands to any components that have a selected function
        _localBlackboard._unitRoot.Selected();

        if (_localBlackboard._moveRoot != null)
            _localBlackboard._moveRoot.Selected();

        if (_localBlackboard._visionRoot != null)
            _localBlackboard._visionRoot.Selected();
    }

    public void CallDropped()
    {
        if (_localBlackboard._moveRoot != null) //this occurs first to make sure patrol speed is applied after standard speed
            _localBlackboard._moveRoot.Dropped();

        //Send commands to any components that have a dropped function
        _localBlackboard._unitRoot.Dropped();

        if(_localBlackboard._visionRoot != null)
            _localBlackboard._visionRoot.Dropped();
    }

    public void CallDead()
    {
        //Send commands to any components that have a dead function
        if (GlobalBlackboard.Instance.selectedUnit == _localBlackboard)
            UIManager.Instance.ButtonClicked(ButtonType.DropUnit);

        //tell UnitRoot to hold off on the updates

        //broadcast unit death to game manager
    }

    public void BehaviorStateChanged()
    {
        if (_localBlackboard._moveRoot != null)
            _localBlackboard._moveRoot.BehaviorStateChanged();
    }

    #region Targeting Commands
    public void AddTarget(LocalBlackboard newTarget, bool enemy)
    {
        if (_localBlackboard._visionRoot != null)
            _localBlackboard._visionRoot.AddTarget(newTarget);

        _localBlackboard._unitRoot.AddTarget(enemy);
    }

    public void DropTarget()
    {
        if (_localBlackboard._visionRoot != null)
            _localBlackboard._visionRoot.DropTarget();

        if (_localBlackboard._moveRoot != null)
            _localBlackboard._moveRoot.TargetDropped();

        _localBlackboard._unitRoot.DropTarget();
    }

    public void MoveToTouchPos(Vector3 touchedPos)
    {
        DropTarget();
        _localBlackboard._unitRoot.PlayerControlledMovement();

        if (_localBlackboard._moveRoot != null)
            _localBlackboard._moveRoot.MoveToTouchPos(touchedPos);
        
    }
    #endregion

    public void AttackButtonChosen(int chosenAttack)
    {
        if(chosenAttack < _localBlackboard.attackCount) //Assures that 'index out of range' error never rears its ugly head
        {
            _localBlackboard._attackManager.AttackSelected(chosenAttack); //should this run through the combatRoot??

            _localBlackboard._moveRoot.AttackStateChanged(); //update targetOffsetMovement distance
        }
    }
}
