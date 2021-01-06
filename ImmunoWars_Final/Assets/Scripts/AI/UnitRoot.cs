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

    //Initialize Components/Check if they exist
    private void Start()
    {
        _localBlackboard = GetComponent<LocalBlackboard>();
        _localBlackboard._unitRoot = this;

        _localBlackboard._commandMessenger = GetComponent<CommandMessenger>();
        _localBlackboard._commandMessenger.Setup(_localBlackboard);

        if (TryGetComponent(out MovementRoot temp))
        {
            _localBlackboard._moveRoot = temp;
            _localBlackboard._moveRoot.Setup(_localBlackboard);
        }

        if(TryGetComponent(out CombatRoot temp2))
        {
            _localBlackboard._combatRoot = temp2;
            _localBlackboard._combatRoot.Setup(_localBlackboard);
        }
    }


    public void Selected()
    {
        _localBlackboard._behaviorState = BehaviorState.PlayerControlled; //move to status manager once it is built
        _localBlackboard.selectionGlow.SetActive(true); //to be replaced with something less hacky  
    }

    public void Dropped()
    {
        _localBlackboard._behaviorState = BehaviorState.Patrol;
        _localBlackboard.selectionGlow.SetActive(false);
    }


    public void UpdateTarget(Transform newTarget, bool enemy)
    {
        _localBlackboard.currentTarget = newTarget;
        _localBlackboard.hasTarget = true;

        if (enemy) //if enemy is target, follow at optimumm attack distance(this will be changing so it needs to update somehow...???)
        {
            _localBlackboard.targetMovementOffset = _localBlackboard.optimumAttackDistance;
            EnterCombat();
        }
        else //if friendly unit is the target, follow at personal space distance
        {
            _localBlackboard.targetMovementOffset = _localBlackboard.personalSpace;
            _localBlackboard._behaviorState = BehaviorState.FollowFriendlyUnit;
        }
        //use an attack
    }

    public void EnterCombat()
    {
        _localBlackboard._behaviorState = BehaviorState.Combat;
    }

    public void MoveToTouchPos(Vector3 target)
    {
        //Vector3 target = new Vector3(target2D.x, GlobalBlackboard.Instance.playfieldHeight, target2D.y);
        _localBlackboard._behaviorState = BehaviorState.PlayerControlled;
        _localBlackboard.hasTarget = false;
        _localBlackboard._moveRoot.TargetDropped();
        _localBlackboard._moveRoot.MoveTo(target);
    }


    public void TakeDamage()
    {
        
    }

    #region Ticks
    private void Update()
    {
        //General Updates
        currentTick += Time.deltaTime;

        if(currentTick > tickTime)
        {
            if(_localBlackboard._moveRoot != null)
                _localBlackboard._moveRoot._update();

            if (_localBlackboard._combatRoot != null && _localBlackboard._behaviorState == BehaviorState.Combat)
                _localBlackboard._combatRoot._update();

            currentTick = 0;
        }

        //Smooth Updates
        if (_localBlackboard._moveRoot != null)
            _localBlackboard._moveRoot._smoothUpdate();
    }
    #endregion
}
