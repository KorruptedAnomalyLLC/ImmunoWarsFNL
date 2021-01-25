///
///This script reads info from attacks and runs appropriate functions to make the attack do stuff
///...might get a bit complicated
///
using System.Collections;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    LocalBlackboard _localBlackboard;
    [SerializeField]
    private bool attackReady = true;

    #region Setup
    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;
    }
    #endregion

    #region Attack Functions
    public void EvaluateAttacking(AttackRoot activeAttack)
    {
        //I still hate this confusing 'targetHeroes' logic but it'll do for now
        if (_localBlackboard.currentTarget.heroUnit != activeAttack.targetHeroes)
            return;

        if (_localBlackboard.inRange && attackReady && activeAttack.attackCharged)
        {
            attackReady = false;
            PlayAttack(activeAttack);
        }
    }


    private void PlayAttack(AttackRoot activeAttack)
    {
        activeAttack.RunAttack();

        StartCoroutine(EndAttack(activeAttack));
    }

    
    //this is here instead of on the attackRoot becuase it keeps a unit from firing off an attack before the last one has finished
    private IEnumerator EndAttack(AttackRoot activeAttack)
    {
        yield return new WaitForSeconds(activeAttack.hitTime);
        attackReady = true;
        activeAttack.EndAttack();
    }
    #endregion
}
