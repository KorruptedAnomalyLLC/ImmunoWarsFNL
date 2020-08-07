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

    public float optimumAttackDistance = 1f;

    public float movementSlopAllowance = 0.5f;
    public float personalSpace = 2f;
    public int originalPriority = 10;

    public GameObject selectionGlow;
    public Sprite attack1UI, attack2UI, attack3UI, dropUnitUI;

    public string nameOfChar = "SuperFluper", 
                  charInfo = "A type of Flu";

    private void Awake()
    {
        optimumAttackDistance *= optimumAttackDistance;
        movementSlopAllowance *= movementSlopAllowance;
    }
}
