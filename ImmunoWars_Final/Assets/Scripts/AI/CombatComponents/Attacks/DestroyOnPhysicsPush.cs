///
///This script destroys it's owner when called by the physics push component
///

using System.Collections;
using UnityEngine;

public class DestroyOnPhysicsPush : MonoBehaviour
{
    [SerializeField]
    private GameObject deathFX;
    [SerializeField]
    private float killTime = 1f;

    public void KillThisThing()
    {
        Instantiate(deathFX, transform.position, transform.rotation);
        StartCoroutine(KillTimer());
    }

    private IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(killTime);
        Destroy(this.gameObject);
    }
}
