using UnityEngine;


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

    public BehaviorState _behaviorState = BehaviorState.Patrol;
    [HideInInspector]
    public Transform currentTarget;
}
