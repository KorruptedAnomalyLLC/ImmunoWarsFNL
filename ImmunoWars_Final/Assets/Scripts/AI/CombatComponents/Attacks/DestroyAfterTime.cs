///
///This script waits for x seconds then destroys whatever object was passed in.
///

using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    private float destroyCountdown = 5f;

    public void StartDestroyCountdown(GameObject destroyTarget)
    {
        StartCoroutine(DestroyCountdown(destroyTarget));
    }

    private IEnumerator DestroyCountdown(GameObject destroyTarget)
    {
        yield return new WaitForSeconds(destroyCountdown);
        Destroy(destroyTarget);
    }
}
