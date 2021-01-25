﻿///
///This script spawns prefabs, it is used as a component of some attacks
///If the object it spawns is an attack, this script will initialize it.
///
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public bool spawnOnPlayAttack = true; //should be read only
    public bool spawnOnLandAttack = false;

    [SerializeField]
    private int numberToSpawn = 1;
    [SerializeField]
    private GameObject prefabToSpawn = default;
    [SerializeField]
    private Vector2[] spawnPoints = default;

    private Vector3 tempSpawnPoint;
    private LocalBlackboard _localBlackboard;
    private GameObject spawnedObject;


    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;
    }


    public void SpawnSomething()
    {
        for(int i = 0; i < numberToSpawn; i++)
        {
            tempSpawnPoint = transform.TransformPoint(spawnPoints[i]);
            tempSpawnPoint = new Vector3(tempSpawnPoint.x, GlobalBlackboard.Instance.playfieldHeight, tempSpawnPoint.z);
            //tempSpawnPoint = _localBlackboard.transform.TransformPoint(tempSpawnPoint);

            spawnedObject = Instantiate(prefabToSpawn, tempSpawnPoint, transform.rotation);

            if(spawnedObject.TryGetComponent(out AttackRoot temp))
            {
                temp.Setup(_localBlackboard);
            }
        }
    }
}
