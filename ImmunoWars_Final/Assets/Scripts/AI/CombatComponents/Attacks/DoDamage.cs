///
///This component allows attacks to hurt units
///

using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 1;
    private float damageToDeal;


    public void DealDamage(StatusManager unitHit, LocalBlackboard _localBlackboard, float damageMultiplier = 1)
    {
        damageToDeal = damageAmount * damageMultiplier;

        unitHit.TakeDamage(damageToDeal, _localBlackboard);
    }
}
