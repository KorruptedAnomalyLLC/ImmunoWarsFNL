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
            //combatRoot.Setup();
        }
    }


    public void Selected()
    {
        _localBlackboard._behaviorState = BehaviorState.PlayerControlled;
        _localBlackboard.selectionGlow.SetActive(true);
        moveRoot.StopMoving();
    }

    public void Drop()
    {
        _localBlackboard._behaviorState = BehaviorState.Patrol;
        _localBlackboard.selectionGlow.SetActive(false);
    }


    public void UpdateTarget(Transform newTarget)
    {
        _localBlackboard.currentTarget = newTarget;
        
        //use an attack
    }

    public void EnterCombat()
    {
        _localBlackboard._behaviorState = BehaviorState.Combat;
    }

    public void MoveToTouchPos(Vector3 target)
    {
        //Vector3 target = new Vector3(target2D.x, GlobalBlackboard.Instance.playfieldHeight, target2D.y);
        moveRoot.MoveTo(target);
    }

    private void Update()
    {
        currentTick += Time.deltaTime;

        if(currentTick > tickTime)
        {
            if(moveRoot != null)
                moveRoot._update();

            currentTick = 0;
        }
    }
}
