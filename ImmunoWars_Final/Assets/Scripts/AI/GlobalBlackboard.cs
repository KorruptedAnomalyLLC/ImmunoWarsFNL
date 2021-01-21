using System.Dynamic;
using UnityEngine;

public class GlobalBlackboard : GenericSingletonClass<GlobalBlackboard>
{
    /// <summary>
    /// ...essentially just global variables, will likely change the name soon
    /// </summary>
    public  LocalBlackboard selectedUnit;
    public  bool unitSelected = false;

    public float playfieldHeight = 0f;
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
