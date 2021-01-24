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
        _atkController.CheckCollision(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        _atkController.CheckCollisionExit(collider);
    }
}
