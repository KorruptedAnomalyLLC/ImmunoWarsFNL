///
///Handles all things related to an attacks collider such as calling to deal damage, etc
///

using UnityEngine;

public class AttackColliderController : MonoBehaviour
{
    [SerializeField]
    private GameObject hitBox = default;

    private LocalBlackboard _localBlackboard;
    private AttackRoot _attackRoot;
    private DamageOverTime _damageOverTime;
    private TriggerSensor _triggerSensor;

    private StatusManager targetStatus;
    private LocalBlackboard hitUnitInfo;
    private Collider myCollider;


    bool onlyHitTarget = true;
    bool targetHeroes = false;

    #region Setup
    public void Setup(LocalBlackboard localBlackboard, AttackRoot attackRoot)
    {
        _localBlackboard = localBlackboard;
        myCollider = _localBlackboard.healthCollider;
        _attackRoot = attackRoot;

        if(TryGetComponent(out TriggerSensor temp))
        {
            _triggerSensor = temp;
            _triggerSensor.Setup(this);
        }
        else
        {
            Debug.LogError(gameObject.name + " is missing a trigger sensor on it's hitbox. Add one or suffer the consequences O_O! ...x_x");
        }

        if (TryGetComponent(out DamageOverTime temp2))
        {
            _damageOverTime = temp2;
            _damageOverTime.Setup(_attackRoot);
        }
    }
    #endregion

    public void ActivateCollider(StatusManager whoToTarget, bool whoToHit, bool aimAtAllies)
    {
        targetStatus = whoToTarget;
        onlyHitTarget = whoToHit;
        targetHeroes = aimAtAllies;
        hitBox.SetActive(true);
    }

    public void DeactivateCollider()
    {
        hitBox.SetActive(false);
    }

    #region Collision Checks
    public void CheckCollision(Collider collider)
    {
        if (collider == myCollider)
            return;

        if (collider.transform.parent.TryGetComponent<LocalBlackboard>(out hitUnitInfo))
        {
            if (hitUnitInfo.heroUnit == targetHeroes) //gotta change this to be attack friendly units
            {
                if (onlyHitTarget)
                {
                    if (hitUnitInfo._statusManager == targetStatus)
                    {
                        _attackRoot.HitUnit(targetStatus);
                        DealWithDamageOverTime(hitUnitInfo._statusManager, true);
                    }
                }
                else
                {
                    _attackRoot.HitUnit(hitUnitInfo._statusManager);
                    DealWithDamageOverTime(hitUnitInfo._statusManager, true);
                }
            }
        }
    }

    public void CheckCollisionExit(Collider collider)
    {
        if (collider == myCollider)
            return;

        if (collider.transform.parent.TryGetComponent<LocalBlackboard>(out hitUnitInfo))
        {
            if (hitUnitInfo.heroUnit == targetHeroes)
            {
                DealWithDamageOverTime(hitUnitInfo._statusManager, false);
            }
        }
    }
    #endregion


    private void DealWithDamageOverTime(StatusManager hitUnit, bool addToList)
    {
        if (_damageOverTime != null)
        {
            if (addToList)
                _damageOverTime.AddToList(hitUnit);
            else
                _damageOverTime.RemoveFromList(hitUnit);
        }
    }
}
