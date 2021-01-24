///
///This script allows attacks to have a recharge/cooldown time before firing off again
///

using System.Collections;
using UnityEngine;

public class Recharge : MonoBehaviour
{
    [SerializeField]
    private float rechargeTime = 2f;

    private AttackRoot _attackRoot;


    public void Setup(AttackRoot attackRoot)
    {
        _attackRoot = attackRoot;
    }


    #region Attack Recharge
    public void RechargeAttack()
    {
        StartCoroutine(RechargeTimer());
    }

    private IEnumerator RechargeTimer()
    {
        yield return new WaitForSeconds(rechargeTime);
        _attackRoot.attackCharged = true;
    }
    #endregion
}
