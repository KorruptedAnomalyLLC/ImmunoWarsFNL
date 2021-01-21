///
///This script spawns prefabs, it is used as a component of some attacks
///
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private LocalBlackboard _localBlackboard;

    [SerializeField]
    private GameObject prefabToSpawn;

    [SerializeField, Tooltip("Must have at least four points")]
    private Vector2[] spawnPoints;

    private Vector3 tempSpawnPoint;



    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;
    }

    public void SpawnSomething(int numberToSpawn = 1)
    {
        for(int i = 0; i < numberToSpawn; i++)
        {
            tempSpawnPoint = new Vector3(spawnPoints[i].x, GlobalBlackboard.Instance.playfieldHeight, spawnPoints[i].y);
            Instantiate(prefabToSpawn, tempSpawnPoint, _localBlackboard.transform.rotation);
        }
    }
}
