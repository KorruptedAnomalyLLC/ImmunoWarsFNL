///
///This script waits for x seconds then destroys whatever object was passed in.
///If there is a spawn function on the attack, this script will fire that off
///before it destroys the attack
///
/// This script also has an option to be used independently of the attack system.
/// This option has it read the Spawn script from the inspector and start the 
/// destroy countdown when spawned
///
using System.Collections;
using UnityEngine;


public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    private float destroyCountdown = 5f;

    [Header("Independent Specific Variables")]
    [SerializeField]
    private Spawn _spawn;
    [SerializeField]
    private bool independent = false;

    #region Independent Functions
    private void Start()
    {
        if (independent)
            StartDestroyCountdown(this.gameObject);
    }
    #endregion


    #region Standard Functions
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
    #endregion
}
