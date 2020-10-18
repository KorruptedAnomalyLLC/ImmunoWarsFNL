using UnityEngine;
using UnityEngine.UI;

public enum BehaviorState
{
    Patrol,
    Combat,
    FlightOrFight,
    Dead,
    PlayerControlled
}


public class LocalBlackboard : MonoBehaviour
{
    public bool heroUnit = false;
    public BehaviorState _behaviorState = BehaviorState.Patrol;
    [HideInInspector]
    public Transform currentTarget;



    public GameObject selectionGlow;
    public Sprite attack1UI, attack2UI, attack3UI, dropUnitUI;

    public string nameOfChar = "SuperFluper", 
                  charInfo = "A type of Flu";

    //Movement Crap
    public bool hasTarget = false;
    public float optimumAttackDistance = 0.5f;
    public float targetMovementOffset = 1f;
    public float movementSlopAllowance = 0.5f;
    public float personalSpace = 2f;
    public int originalPriority = 10;
    public float lookAtThreshold = 2f;

    [Space(20)]
    //Stats when Player Controlled
    public float pSpeed = 1f;
    public float pAcceleration = 1f;

    [Space(10)]
    //Stats when System Controlled
    public float sSpeed = 1f;
    public float sAcceleration = 1f;

    private void Awake()
    {
        //this is done to avoid using square root functions in Movement scripts... will actually get used in optimization pass
        //optimumAttackDistance *= optimumAttackDistance;
        //movementSlopAllowance *= movementSlopAllowance;
    }


    //Debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, personalSpace);
    }
}
