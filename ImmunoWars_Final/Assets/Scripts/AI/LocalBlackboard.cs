using UnityEngine;
using UnityEngine.AI;

public enum BehaviorState
{
    Patrol,
    Combat,
    FlightOrFight,
    Dead,
    PlayerControlled,
    FollowFriendlyUnit
}

public enum Attack 
{ 
    Slot1,
    Slot2,
    Slot3
}


public class LocalBlackboard : MonoBehaviour
{
    #region Component Roots
    [HideInInspector]
    public MovementRoot _moveRoot;
    [HideInInspector]
    public UnitRoot _unitRoot;
    [HideInInspector]
    public CombatRoot _combatRoot;
    [HideInInspector]
    public CommandMessenger _commandMessenger;
    #endregion


    public bool heroUnit = false;
    public BehaviorState _behaviorState = BehaviorState.Patrol;
    public Attack _attack = Attack.Slot1;

    public Transform currentTarget;



    public GameObject selectionGlow;
    public Sprite attack1UI, attack2UI, attack3UI, dropUnitUI;

    public string nameOfChar = "SuperFluper", 
                  charInfo = "A type of Flu";

    #region Movement Variables 
    [Header("Movement Variables")]
    public float optimumAttackDistance = 0.5f;
    public float targetMovementOffset = 1f; //how much distance to keep between unit and it's target
    public float movementSlopAllowance = 0.5f;
    public float personalSpace = 2f;   
    public float lookAtThreshold = 2f;
    [HideInInspector]
    public int originalPriority = 10;
    [HideInInspector]
    public bool hasTarget = false;
    [HideInInspector]
    public NavMeshAgent navAI;
    #endregion


    [Space(20)]
    //Stats when Player Controlled
    public float pSpeed = 1f;
    public float pAcceleration = 1f;

    [Space(10)]
    //Stats when System Controlled
    public float sSpeed = 1f;
    public float sAcceleration = 1f;

    
    #region Attack Variables
    [Header("Attack Variables")]
    public bool inRange = false;
    public int damageAmount = 1;
    #endregion

    [Space(20)]
    public StatusManager _healthManager;
    public int energyLevel = 5;


   // private void Awake()
    //{
        //this is done to avoid using square root functions in Movement scripts... will actually get used in optimization pass
        //optimumAttackDistance *= optimumAttackDistance;
        //movementSlopAllowance *= movementSlopAllowance;
    //}


    //Debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, personalSpace);
    }
}
