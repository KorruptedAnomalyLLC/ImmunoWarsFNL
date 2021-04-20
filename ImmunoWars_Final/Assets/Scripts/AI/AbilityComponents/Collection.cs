using UnityEngine;

public class Collection : MonoBehaviour
{
    CollectibleType grabbedObject;
    LocalBlackboard _localBlackboard;

    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;
    }

    private void OnTriggerEnter(Collider other)
    {
        grabbedObject = other.gameObject.GetComponent<CollectibleType>();
        if (grabbedObject != null)
        {
            _localBlackboard._statusManager.UpdateAttackType(grabbedObject.RetrieveType());
            Destroy(other.gameObject);
            Debug.LogError("Grabbed a droplet");
        }
    }
}
