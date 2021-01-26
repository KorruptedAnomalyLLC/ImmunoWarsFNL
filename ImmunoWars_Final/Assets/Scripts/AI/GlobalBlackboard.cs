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

    public Transform movementRangeBox;

    public float typeDamageMultiplier = 4f;

    public int maxUnitsInField = 50;
    public int unitsInFieldCount = 0;

    public LayerMask unitPhysicsLayer;
    public UnitSpawner _unitSpawner;

    public override void Awake()
    {
        base.Awake(); //calls instance setup function

        //failsafe for having at least one rangebox for movement
        if (movementRangeBox == null)
            movementRangeBox = GameObject.FindGameObjectWithTag("GenericMovementTerritory").transform;

        playField = GameObject.FindGameObjectWithTag("PlayField").transform;
        playfieldHeight = playField.position.y;
    }
}
