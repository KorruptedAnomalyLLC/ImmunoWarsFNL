///
///This script manages the unit's energy/health and any status effects
///

using System.Collections;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    [HideInInspector]
    public LocalBlackboard _localBlackboard; //this should be private
    private TypeInfuser _typeInfuser;
    [HideInInspector]
    public bool hasType = false; //should be in local blackboard, attacks shouldn't be accessing vars from root scripts
    [SerializeField]
    private GameObject deathFX = default;

    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;

        if (TryGetComponent(out TypeInfuser temp))
        {
            _typeInfuser = temp;
            hasType = true;
        }
    }

    public void FullRecovery()
    {
        _localBlackboard.energyLevel = _localBlackboard.fullEnergyLevel;
    }

    public void AdjustEnergy(float adjustAmount)
    {
        _localBlackboard.energyLevel += adjustAmount;
        CheckEnergy();
    }

    #region Stun Effect
    Coroutine tempCoroutine;
    public void ApplyStunEffect(float stunTime)
    {
        _localBlackboard.isStunned = true;

        if(tempCoroutine != null)
        {
            StopCoroutine(tempCoroutine);
        }

        _localBlackboard._commandMessenger.ApplyStunEffect();

        tempCoroutine = StartCoroutine(StunTimer(stunTime));
    }

    private IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        _localBlackboard.isStunned = false;
        _localBlackboard._commandMessenger.EndStunEffect();
    }
    #endregion

    private void CheckEnergy()
    {
        if (_localBlackboard.energyLevel <= 0 && !_localBlackboard.dead)
        {
            _localBlackboard.dead = true;
            Die();
        }
    }

    public Type FindType()
    {
        if (_typeInfuser != null)
            return _typeInfuser.myType;
        else
            return Type.None;
    }

    //how's this handle healing? Doesn't seem like it currently does
    public void TakeDamage(float damageTaken, LocalBlackboard attacker)
    {
        _localBlackboard.energyLevel -= damageTaken;

        if (!_localBlackboard.hasTarget)
        {
            _localBlackboard._commandMessenger.AddTarget(attacker, true);
        }

        CheckEnergy();
    }

    public void Die()
    {
        _localBlackboard._commandMessenger.CallDead();
        _localBlackboard.dead = true;

        if (deathFX != null)
        {
            deathFX.SetActive(true);
        }

        //start destroy countdown
        StartCoroutine(UnitDeathDelay());
    }

    private float deathDelay = 1f;
    private IEnumerator UnitDeathDelay()
    {
        yield return new WaitForSeconds(deathDelay);

        GlobalBlackboard.Instance.unitsInFieldCount--;
        Destroy(this.transform.gameObject);
    }
}
