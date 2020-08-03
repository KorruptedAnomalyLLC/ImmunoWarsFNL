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

    public float playfieldHeight = 0f;
    public Transform playField;

    private void Awake()
    {
        playField = GameObject.FindGameObjectWithTag("PlayField").transform;
        playfieldHeight = playField.position.y;
    }
}
