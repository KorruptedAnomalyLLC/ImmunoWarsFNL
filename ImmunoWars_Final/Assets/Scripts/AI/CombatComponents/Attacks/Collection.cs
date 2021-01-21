using UnityEngine;

public class Collection : MonoBehaviour
{
    CollectibleType grabbedObject;
    AttackRoot _attackRoot;

    public void Setup(AttackRoot attackRoot)
    {
        _attackRoot = attackRoot;
    }

    private void OnTriggerEnter(Collider other)
    {
        grabbedObject = other.gameObject.GetComponent<CollectibleType>();
        if (grabbedObject != null)
        {
            _attackRoot.UpdateAttackType(grabbedObject.RetrieveType());
        }
    }
}
