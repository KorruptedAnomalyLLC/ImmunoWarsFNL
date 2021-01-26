///
///This ability allows units to spawn something every x seconds
///This script is pretty damn messy.Should be cleaned up at some pont
///

using System.Collections;
using UnityEngine;

public class MultiplyAbility : MonoBehaviour
{

    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;
        StartCoroutine(RepeatAbility());
    }


    #region Spawn
    [SerializeField]
    private bool spawnUnit = true;
    [SerializeField]
    private int numberToSpawn = 1;
    [SerializeField, Tooltip("This number should align with the array position of the wanted unit in the UnitSpawner object under Game in the scene view")]
    private int unitToSpawn = default;
    [SerializeField, Tooltip("This is only used if spawn unit is set to false")]
    private GameObject prefabToSpawn = default;
    [SerializeField]
    private Vector2[] spawnPoints = default;

    private Vector3 tempSpawnPoint;

    public void SpawnSomething()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            tempSpawnPoint = transform.TransformPoint(spawnPoints[i]);
            tempSpawnPoint = new Vector3(tempSpawnPoint.x, GlobalBlackboard.Instance.playfieldHeight, tempSpawnPoint.z);

            if (spawnUnit)
            {
                if(GlobalBlackboard.Instance.unitsInFieldCount >= GlobalBlackboard.Instance.maxUnitsInField)
                {
                    GlobalBlackboard.Instance._unitSpawner.SpawnAUnit(unitToSpawn, tempSpawnPoint, transform.rotation);
                }
            }
            else
            {
                Instantiate(prefabToSpawn, tempSpawnPoint, Quaternion.identity);
            }
        }
    }
    #endregion

    #region Repeater
    [SerializeField]
    private float repeatTime = 2f;

    private LocalBlackboard _localBlackboard;

    private IEnumerator RepeatAbility()
    {
        yield return new WaitForSeconds(repeatTime);

        if(!_localBlackboard.dead)
            SpawnSomething();

        StartCoroutine(RepeatAbility());
    }
    #endregion
}
