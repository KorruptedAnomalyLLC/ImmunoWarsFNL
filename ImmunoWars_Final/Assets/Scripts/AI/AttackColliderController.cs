
using UnityEngine;

public class AttackColliderController : MonoBehaviour
{
    private int damageAmount = 2;
    private HealthManager healthScript;
    private Collider myCollider;

    private void Start()
    {
        myCollider = GetComponentInParent<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider == myCollider)
            return;



        healthScript = collider.gameObject.GetComponentInParent<HealthManager>();

        Debug.Log("Made it into Trigger " + healthScript);

        if (healthScript != null)
        {
            healthScript.TakeDamage(damageAmount);
        }
    }
}
