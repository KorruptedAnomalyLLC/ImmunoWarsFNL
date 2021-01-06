using System.Collections;
using UnityEngine;

public class CombatRoot : MonoBehaviour
{

    private LocalBlackboard _localBlackboard;
    public float attackRechargeTime = 10f;
    private float counter = 0f;

    public bool attackReady = true;

    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;
    }


    private void Update()
    {
        Recharge();
    }

    public void _update()
    {
        CheckRange();
        CheckTarget();
        switch (_localBlackboard._attack)
        {
            case Attack.Slot1:
                BasicTestAttack();
                break;
            case Attack.Slot2:
                break;
            case Attack.Slot3:
                break;
            default:
                break;
        }
    }


    public GameObject basicHitbox;
    private Coroutine tempCoroutine;
    private void BasicTestAttack()
    {
        if(_localBlackboard.inRange && attackReady)
        {
            attackReady = false;
            basicHitbox.SetActive(true);

            if(tempCoroutine == null)
                tempCoroutine = StartCoroutine(ResetAttack());
        }
    }

    public float hitTime = 0.6f;
    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(hitTime);
        basicHitbox.SetActive(false);

    }

    


    private void Recharge()
    {
        if (attackReady)
            return;

        counter += Time.deltaTime;
        if (counter >= attackRechargeTime)
        {
            attackReady = true;
            tempCoroutine = null;
            counter = 0;
        }

    }


    public float attackSlopDistance = 2f;
    private void CheckRange()
    {
        Vector3 TempPos = _localBlackboard.currentTarget.position;

        if((TempPos - transform.position).magnitude < attackSlopDistance + _localBlackboard.targetMovementOffset)
        {
            _localBlackboard.inRange = true;
        }
        else
        {
            _localBlackboard.inRange = false;
        }
    }


    private float sightRange = 10f;
    private void CheckTarget()
    {
        if (!_localBlackboard.hasTarget)
        {
            //find a target
            //Physics.SphereCast(transform.position, sightRange)
        }
    }
}
