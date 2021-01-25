///
///This script waits for x seconds then destroys whatever object was passed in.
///If there is a spawn function on the attack, this script will fire that off
///before it destroys the attack
///

using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    private float destroyCountdown = 5f;

    private Spawn _spawn;

    public void Setup(Spawn spawn)
    {
        _spawn = spawn;
    }

    public void StartDestroyCountdown(GameObject destroyTarget)
    {
        StartCoroutine(DestroyCountdown(destroyTarget));
    }

    private IEnumerator DestroyCountdown(GameObject destroyTarget)
    {
        yield return new WaitForSeconds(destroyCountdown);

        if(_spawn != null)
            _spawn.SpawnSomething();
        
        Destroy(destroyTarget);
    }
}
