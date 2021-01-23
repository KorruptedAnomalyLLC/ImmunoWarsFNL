///
///This script looks for stuff,
///it is disabled when the player is in control of this unit
///
using UnityEngine;

public class VisionRoot : MonoBehaviour
{
    private LocalBlackboard _localBlackboard; //req
    [SerializeField]
    private float visionDistance = 10f;
    private Collider[] objectsHit;

    private int numberOfUnitsSeen;
    private LocalBlackboard targetInfo, tempTargetInfo;
    private float lowestEnergy;

    private bool visionEnabled = true;

    #region Setup
    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;
        objectsHit = new Collider[GlobalBlackboard.Instance.maxUnitsInField];
    }
    #endregion

    #region Selected/Dropped
    public void Selected()
    {
        visionEnabled = false;
    }

    public void Dropped()
    {
        visionEnabled = true;
    }
    #endregion


    #region Update Target Functions
    public void AddTarget(LocalBlackboard newTarget) //possibly move this to status manager
    {
        _localBlackboard.hasTarget = true;
        _localBlackboard.currentTarget = newTarget;
    }

    public void DropTarget()
    {
        _localBlackboard.hasTarget = false;
        _localBlackboard.currentTarget = null;
    }
    #endregion

    #region Update Ticks
    public void _update()
    {
        if (!visionEnabled)
            return;

        if (!_localBlackboard.hasTarget)
            SearchForTarget();
        else
            CheckIfTargetExists();
    }
    #endregion

    #region Seek Target
    private void SearchForTarget()
    {
        numberOfUnitsSeen = Physics.OverlapSphereNonAlloc(_localBlackboard.transform.position, visionDistance, objectsHit, GlobalBlackboard.Instance.unitPhysicsLayer);
        PrioritizeTarget();
    }


    //searches for enemy with the lowest health and makes that the target
    private void PrioritizeTarget()
    {
        lowestEnergy = Mathf.Infinity;
        targetInfo = null;

        for (int i = 0; i < numberOfUnitsSeen; i++)
        {
            if(objectsHit[i].transform.parent != transform) //don't find yourself
            {
                if(objectsHit[i].transform.parent.TryGetComponent<LocalBlackboard>(out tempTargetInfo))
                {
                    if (tempTargetInfo.heroUnit != _localBlackboard.heroUnit && !tempTargetInfo.dead) //if not on same team as this unit and that unit isn't dead
                    {
                        if (tempTargetInfo.energyLevel < lowestEnergy) //check to find the enemy with the lowest health, target that one
                        {
                            lowestEnergy = tempTargetInfo.energyLevel;
                            targetInfo = tempTargetInfo;
                        }
                    }
                }
            }
        }

        if (targetInfo != null)
            _localBlackboard._commandMessenger.AddTarget(targetInfo, true);
    }
    #endregion

    #region Monitor Target
    private void CheckIfTargetExists()
    {
        //when a target dies, drop it and search for a new target, possibly switch out of combat mode if a new one isn't found
        if(_localBlackboard.currentTarget == null)
        {
            _localBlackboard._commandMessenger.DropTarget();
            SearchForTarget();
        }
    }
    #endregion

    #region Debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, visionDistance);
    }
    #endregion
}
