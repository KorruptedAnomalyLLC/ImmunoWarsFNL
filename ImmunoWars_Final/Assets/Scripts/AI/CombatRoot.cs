using System.Collections;
using UnityEngine;

public class CombatRoot : MonoBehaviour
{
    //AI tree branch references
    private LocalBlackboard _localBlackboard;
    
    

    public float attackRechargeTime = 10f;
    private float counter = 0f;

    public bool attackReady = true;

    #region Setup
    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;

        if(TryGetComponent(out AttackManager temp))
        {
            _localBlackboard._attackManager = temp;
            _localBlackboard._attackManager.Setup(_localBlackboard);
        }
    }
    #endregion

    #region Ticks
    public void _update()
    {
        if (_localBlackboard.isStunned)
            return;

        CheckRange();
        CheckTarget();
        _localBlackboard._attackManager._update();
    }
    #endregion


    #region Check if in range to use attack
    public float attackSlopDistance = 2f;
    private void CheckRange()
    {
        Vector3 TempPos = _localBlackboard.currentTarget.transform.position;

        if((TempPos - transform.position).magnitude < attackSlopDistance + _localBlackboard.targetMovementOffset)
        {
            _localBlackboard.inRange = true;
        }
        else
        {
            _localBlackboard.inRange = false;
        }
    }
    #endregion

    //will be moved to vision Root
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
