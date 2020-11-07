using UnityEngine;

public class AttackColliderController : MonoBehaviour
{
    private int damageAmount = 2;
    private HealthManager healthScript;
    private Collider myCollider;
    private LocalBlackboard _localBlackboard;

    private void Start()
    {
        _localBlackboard = GetComponentInParent<LocalBlackboard>();
        myCollider = _localBlackboard._healthManager.GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider == myCollider)
            return;


        healthScript = collider.gameObject.GetComponent<HealthManager>();

        if (healthScript != null)
        {
            healthScript.TakeDamage(damageAmount, transform.parent.transform);
        }
    }
}
