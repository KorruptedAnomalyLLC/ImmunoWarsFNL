///
///This script spawns prefabs, it is used as a component of some attacks
///
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    private int numberToSpawn = 1;
    [SerializeField]
    private GameObject prefabToSpawn = default;
    [SerializeField]
    private Vector2[] spawnPoints = default;

    private Vector3 tempSpawnPoint;
    private LocalBlackboard _localBlackboard;



    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;
    }

    public void SpawnSomething()
    {
        for(int i = 0; i < numberToSpawn; i++)
        {
            tempSpawnPoint = new Vector3(spawnPoints[i].x, GlobalBlackboard.Instance.playfieldHeight, spawnPoints[i].y);
            tempSpawnPoint = _localBlackboard.transform.TransformPoint(tempSpawnPoint);

            Instantiate(prefabToSpawn, tempSpawnPoint, _localBlackboard.transform.rotation);
        }
    }
}
