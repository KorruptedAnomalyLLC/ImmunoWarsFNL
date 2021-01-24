///
///This script allows an attack to heal units or heal itself or both
///

using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField]
    private bool healOthers = true;
    [SerializeField]
    private bool healSelf = false;


    public void HealUnit(StatusManager unitHit, LocalBlackboard _localBlackboard)
    {
        if(healOthers)
            unitHit.FullRecovery(); //heal units instead of damaging them

        if (healSelf)
            _localBlackboard._statusManager.FullRecovery();
    }
}
