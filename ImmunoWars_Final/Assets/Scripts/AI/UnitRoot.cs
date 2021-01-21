using UnityEngine;

public class UnitRoot : MonoBehaviour
{
    /// <summary>
    /// Base of the AI, all roads start from here
    /// </summary>

    [SerializeField]
    private float tickTime = 0.5f; //how often to run updates, not done everyframe to save on processing power
    private float currentTick = 0;

    [HideInInspector]
    public LocalBlackboard _localBlackboard; //req

    #region Setup
    //Initialize Components/Check if they exist
    private void Start()
    {
        if (TryGetComponent(out LocalBlackboard temp))
        {
            _localBlackboard = temp;
            _localBlackboard._unitRoot = this;
        }
        else
        {
            Debug.LogError(gameObject.name + "  is missing a LocalBlackboard Component. Please add it or the AI won't do anything.");
        }

        if (TryGetComponent(out CommandMessenger temp2))
        {
            _localBlackboard._commandMessenger = temp2;
            _localBlackboard._commandMessenger.Setup(_localBlackboard);
        }
        else
        {
            Debug.LogError(gameObject.name + "  is missing a CommandMessenger Component. Please add it or the AI won't do anything.");
        }
    }
    #endregion

    #region Player Selection Commands
    public void Selected()
    {
        SwapBehaviorState(BehaviorState.PlayerControlled);
        ToggleSelectionGlow(true); 
    }

    public void Dropped()
    {
        SwapBehaviorState(BehaviorState.Patrol);
        ToggleSelectionGlow(false);
    }

    public void PlayerControlledMovement()
    {
        SwapBehaviorState(BehaviorState.PlayerControlled);
    }
    #endregion


    #region UI Manager
    private void ToggleSelectionGlow(bool onOff)
    {
        _localBlackboard.selectionGlow.SetActive(onOff);
    }
    #endregion

    #region Behavior State Changes
    private void SwapBehaviorState(BehaviorState newState)
    {
        _localBlackboard._previousState = _localBlackboard._behaviorState;
        _localBlackboard._behaviorState = newState;
        _localBlackboard._commandMessenger.BehaviorStateChanged();
    }
    #endregion


    //need to sync this up with vissionRoot's update target functions
    public void AddTarget(bool enemy)
    {
        if (enemy)
        {
            SwapBehaviorState(BehaviorState.Combat);
        }
        else
        {
            SwapBehaviorState(BehaviorState.FollowFriendlyUnit);
        }
    }

    public void DropTarget()
    {
        SwapBehaviorState(BehaviorState.Patrol);
    }


    #region Ticks
    private void Update()
    {
        if (_localBlackboard.dead)
            return;

        //General Updates
        currentTick += Time.deltaTime;

        if(currentTick > tickTime)
        {
            if (_localBlackboard._visionRoot != null)
                _localBlackboard._visionRoot._update();

            if(_localBlackboard._moveRoot != null)
                _localBlackboard._moveRoot._update();

            if (_localBlackboard._combatRoot != null && _localBlackboard._behaviorState == BehaviorState.Combat) //should the behavior check be done in combat root??
                _localBlackboard._combatRoot._update();

            currentTick = 0;
        }

        //Smooth Updates
        if (_localBlackboard._moveRoot != null)
            _localBlackboard._moveRoot._smoothUpdate();
    }
    #endregion
}
