using UnityEngine;

public class AttackColliderController : MonoBehaviour
{

    private StatusManager hitUnitsStatus, targetStatus;
    private LocalBlackboard hitUnitInfo;
    private Collider myCollider;
    private LocalBlackboard _localBlackboard;
    private AttackRoot _attackRoot;
    private DamageOverTime _damageOverTime;

    bool onlyHitTarget = true;
    bool targetHeroes = false;

    public void Setup(LocalBlackboard localBlackboard, AttackRoot attackRoot)
    {
        _localBlackboard = localBlackboard;
        myCollider = _localBlackboard.healthCollider;
        _attackRoot = attackRoot;
        if (TryGetComponent(out DamageOverTime temp))
        {
            _damageOverTime = temp;
            _damageOverTime.Setup(_attackRoot);
        }
    }

    public void ActivateCollider(StatusManager whoToTarget, bool whoToHit, bool aimAtAllies)
    {
        targetStatus = whoToTarget;
        onlyHitTarget = whoToHit;
        targetHeroes = aimAtAllies;
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider == myCollider)
            return;

        if(collider.transform.parent.TryGetComponent<LocalBlackboard>(out hitUnitInfo))
        {
            if(hitUnitInfo.heroUnit == targetHeroes)
            {
                if (onlyHitTarget)
                {
                    if (hitUnitInfo._statusManager == targetStatus)
                        _attackRoot.DealDamage(targetStatus);
                }
                else
                {
                    _attackRoot.DealDamage(hitUnitInfo._statusManager);
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.transform.parent.TryGetComponent<LocalBlackboard>(out hitUnitInfo))
        {
            if(hitUnitInfo.heroUnit == targetHeroes)
            {
                //Call damage over time remove.... gotta do some digging here, thought I had finished this one
            }
        }
    }
}
