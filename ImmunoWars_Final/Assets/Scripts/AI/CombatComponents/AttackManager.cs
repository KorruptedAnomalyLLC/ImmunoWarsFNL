///Chooses which attack to use
///
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    //Available Attacks
    public GameObject[] attackPrefabs;
    [HideInInspector]
    public List<AttackRoot> attackRoots = new List<AttackRoot>();

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

        _localBlackboard.attackCount = attackRoots.Count;

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


    float highestValue;
    int currentChosenAtck = 0;
    //Chooses the attack with the highest attackValue(number to represent how powerful/useful this attack is) and filters out those that would deplete this unit's energy completely.
    //default attack if all have too high an energy cost is the first attack
    //if need be, could throw in code to have default be the highest damaging attack but I don't think it'd add much
    public void SelectAttack()
    {
        highestValue = -1;
        activeAttack = attackRoots[0];

        for(int i = 0; i < attackRoots.Count; i++)
        {
            if(attackRoots[i].attackValue > highestValue && attackRoots[i].energyCost >= _localBlackboard.energyLevel)
            {
                highestValue = attackRoots[i].attackValue;
                currentChosenAtck = i;
            }
        }

        _localBlackboard._commandMessenger.AttackButtonChosen(currentChosenAtck);
    }
}
