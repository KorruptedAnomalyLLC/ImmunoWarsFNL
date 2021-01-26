///
///This script applies a pushback force to an object without needing a statusManager.
///This is used on the fake super fluper
///

using UnityEngine;

public class RecievePhysicsPush : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    private Vector3 tempDirection;
    private Transform targetTransform;

    private DestroyOnPhysicsPush _destroyOnPush;

    public void Setup(Transform targetTrans)
    {
        targetTransform = targetTrans;

        if(TryGetComponent(out DestroyOnPhysicsPush temp))
        {
            _destroyOnPush = temp;
        }
    }

    public void RecievePush(Vector3 pushForce)
    {
        rb.AddForce(pushForce);

        _destroyOnPush.KillThisThing();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError(gameObject.name + " read trigger enter.");
        if (other.transform.parent.TryGetComponent(out PushInflictor temp))
        {
            Debug.LogError(gameObject.name + " Soudl be dien now");
            tempDirection = (transform.position - targetTransform.position).normalized;

            RecievePush(temp.ReadPushForce() * tempDirection);
        }
    }
}