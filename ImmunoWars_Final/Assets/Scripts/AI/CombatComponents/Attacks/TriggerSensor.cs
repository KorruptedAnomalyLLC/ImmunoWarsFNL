///
///This script listens for Trigger Events from the attatched collider and relays them to the AttackColliderController
///
using UnityEngine;

public class TriggerSensor : MonoBehaviour
{
    private AttackColliderController _atkController;


    public void Setup(AttackColliderController atkController)
    {
        _atkController = atkController;
    }



    private void OnTriggerEnter(Collider collider)
    {
        if (_atkController != null)
            _atkController.CheckCollision(collider);
        else
            Debug.LogError(gameObject.name + " Is missing the reference to it's attackColliderController...");
    }

    private void OnTriggerExit(Collider collider)
    {
        if (_atkController != null)
            _atkController.CheckCollisionExit(collider);
        else
            Debug.LogError(gameObject.name + " Is missing the reference to it's attackColliderController...");
    }
}
