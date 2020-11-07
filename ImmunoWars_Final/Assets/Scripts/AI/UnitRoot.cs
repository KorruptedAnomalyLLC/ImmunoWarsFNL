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
    public MovementRoot moveRoot; //not req
    [HideInInspector]
    public CombatRoot combatRoot; //not req

    [HideInInspector]
    public LocalBlackboard _localBlackboard; //req


    private void Start()
    {
        _localBlackboard = GetComponent<LocalBlackboard>();


        if (TryGetComponent(out MovementRoot temp))
        {
            moveRoot = temp;
            moveRoot.Setup();
        }

        if(TryGetComponent(out CombatRoot temp2))
        {
            combatRoot = temp2;
            combatRoot.Setup();
        }
    }


    public void Selected()
    {
        _localBlackboard._behaviorState = BehaviorState.PlayerControlled;
        _localBlackboard.selectionGlow.SetActive(true);
        moveRoot.Selected();
        moveRoot.StopMoving();
    }

    public void Drop()
    {
        _localBlackboard._behaviorState = BehaviorState.Patrol;
        _localBlackboard.selectionGlow.SetActive(false);
        moveRoot.Dropped();
    }


    public void UpdateTarget(Transform newTarget, bool enemy)
    {
        _localBlackboard.currentTarget = newTarget;
        _localBlackboard.hasTarget = true;

        if (enemy) //if enemy is target, follow at optimumm attack distance(this will be changing so it needs to update somehow...???)
        {
            _localBlackboard.targetMovementOffset = _localBlackboard.optimumAttackDistance;
        }
        else //if friendly unit is the target, follow at personal space distance
        {
            _localBlackboard.targetMovementOffset = _localBlackboard.personalSpace;
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
        _localBlackboard.hasTarget = false;
        moveRoot.TargetDropped();
        moveRoot.MoveTo(target);
    }

    private void Update()
    {
        currentTick += Time.deltaTime;

        if(currentTick > tickTime)
        {
            if(moveRoot != null)
                moveRoot._update();

            if (combatRoot != null && _localBlackboard._behaviorState == BehaviorState.Combat)
                combatRoot._update();

            currentTick = 0;
        }
    }
}
