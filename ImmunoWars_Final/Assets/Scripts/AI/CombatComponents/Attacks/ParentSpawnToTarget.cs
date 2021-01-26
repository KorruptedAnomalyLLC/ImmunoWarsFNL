///
///This script parents an object to a target.
///Name is a bit dumb but it is currently being used as a subset of the spawn component for fluper and corona
///

using UnityEngine;

public class ParentSpawnToTarget : MonoBehaviour
{
    public void ParentObjectToTarget(Transform childObj, Transform parentObj)
    {
        childObj.parent = parentObj;
    }
}
