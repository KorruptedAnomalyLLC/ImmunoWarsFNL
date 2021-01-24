///
///Use this script to handle any attack fx (VFX, sounds, sprites, animations, ect)
///It will need to be updated to handle whatever fx are created unless they 
///fit into the current on/off setup.
///**If you want constant VFX to play for the attack(such as with missiles) then don't link those FX up to this script,
///just parent them to the attack object and leave them enabled. This script is only for FX that are toggling FX on and off
///when the attack is ran or stopped
///

using UnityEngine;

public class AttackFXPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject attackFX = default;


    public void PlayAttackFX()
    {
        attackFX.SetActive(true);
    }

    public void StopAttackFX()
    {
        attackFX.SetActive(false);
    }
}
