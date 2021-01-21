///Chooses which attack to use
///
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    //Available Attacks
    public GameObject[] attackPrefabs;
    public List<AttackRoot> attackRoots = new List<AttackRoot>();
    [SerializeField]
    private AttackRoot activeAttack;
    private AttackPlayer _attackPlayer;
    private LocalBlackboard _localBlackboard;

    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;

        if(TryGetComponent(out AttackPlayer temp))
        {
            _attackPlayer = temp;
            _attackPlayer.Setup(localBlackboard);
        }

        foreach(GameObject atk in attackPrefabs)
        {
            attackRoots.Add(atk.GetComponent<AttackRoot>());
        }

        foreach(AttackRoot atkRt in attackRoots)
        {
            atkRt.Setup(_localBlackboard);
        }

        SelectAttack();
    }

    #region Manage Ticks
    public void _update()
    {
        //if in combat
        _attackPlayer.EvaluateAttacking(activeAttack);
    }
    #endregion

    //called by input manager, sets the activeAttack to whatever the player selected
    public void AttackSelected(int chosenAttack)
    {
        activeAttack = attackRoots[chosenAttack];
        activeAttack.OnAttackSelected();
    }


    float mostDamage = 0;
    int currentChosenAtck = 0;
    //Chooses the most damaging(or highest healing) attack and filter out those that would deplete this unit's energy completely.
    //default attack if all have too high an energy cost is the first attack
    //if need be, could throw in code to have default be the highest damaging attack but I don't think it'd add much
    public void SelectAttack()
    {
        activeAttack = attackRoots[0];

        for(int i = 0; i < attackRoots.Count; i++)
        {
            if(Mathf.Abs(attackRoots[i].damageAmount) > mostDamage && attackRoots[i].energyCost >= _localBlackboard.energyLevel)
            {
                mostDamage = attackRoots[i].damageAmount;
                currentChosenAtck = i;
            }
        }

        _localBlackboard._commandMessenger.AttackButtonChosen(currentChosenAtck);
    }
}
