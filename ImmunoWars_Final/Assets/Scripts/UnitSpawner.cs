///
///This script provides a simple way for units to instatiate a copy of their own prefab rather than cloning themselves with the same status effects, etc
///

using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] unitsToSpawn;

    public void SpawnAUnit(int unitToSpawn, Vector3 spawnPoint, Quaternion spawnRotation)
    {
        Instantiate(unitsToSpawn[unitToSpawn], spawnPoint, spawnRotation);
    }
}
