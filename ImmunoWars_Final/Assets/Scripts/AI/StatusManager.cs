///
///This script manages the unit's energy/health and any status effects
///

using System.Collections;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public LocalBlackboard _localBlackboard; //this should be private
    private TypeInfuser _typeInfuser;
    public bool hasType = false; //should be in local blackboard, attacks shouldn't be accessing vars from root scripts
    [SerializeField]
    private GameObject deathFX;

    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;

        if (TryGetComponent(out TypeInfuser temp))
        {
            _typeInfuser = temp;
            hasType = true;
        }
    }

    public void AdjustEnergy(float energyChange)
    {
        _localBlackboard.energyLevel += energyChange;
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
            tempCoroutine = null;
            tempCoroutine = StartCoroutine(StunTimer(stunTime));
        }
    }

    private IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        _localBlackboard.isStunned = false;
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
        //play death FX
        deathFX.SetActive(true);
        _localBlackboard.dead = true;

        //start destroy countdown
        StartCoroutine(UnitDeathDelay());
    }

    private float deathDelay = 1f;
    private IEnumerator UnitDeathDelay()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(this.transform.gameObject);
    }
}
