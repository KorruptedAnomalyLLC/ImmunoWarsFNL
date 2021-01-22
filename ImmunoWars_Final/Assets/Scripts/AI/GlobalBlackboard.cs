/// <summary>
/// ...essentially just global variables, will likely change the name soon
/// </summary>

using UnityEngine;

public class GlobalBlackboard : GenericSingletonClass<GlobalBlackboard>
{
    [HideInInspector]
    public  LocalBlackboard selectedUnit;
    [HideInInspector]
    public  bool unitSelected = false;

    [HideInInspector]
    public float playfieldHeight = 0f;
    [HideInInspector]
    public Transform playField;

    public float typeDamageMultiplier = 4f;

    public int maxUnitsInField = 50;

    public LayerMask unitPhysicsLayer;

    public override void Awake()
    {
        base.Awake();
        playField = GameObject.FindGameObjectWithTag("PlayField").transform;
        playfieldHeight = playField.position.y;
    }
}
