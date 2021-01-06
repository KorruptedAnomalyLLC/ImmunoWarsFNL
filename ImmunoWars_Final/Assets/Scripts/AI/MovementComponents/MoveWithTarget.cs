///
///
///

using UnityEngine;

public class MoveWithTarget : MonoBehaviour
{
    LocalBlackboard _localBlackboard;

    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;
    }


    //maintain optimum attack distance based on the current attack
    //ToDo
    //Distance should be calculated outside of the movement branch, best to make a sight branch for choosing targets, etc
    public Vector3 GrabTargetMovePos()
    {
        //move to optimum distance, if already there don't move
        Vector3 moveTarget = ProcessTargetOffset(_localBlackboard.targetMovementOffset);
        return moveTarget;
    }


    //Disable NavAI rotation and tell the MovementRoot to rotate the player manually
    public bool RotationBasedOnTarget(Vector3 moveTarget)
    {
        if ((transform.position - moveTarget).magnitude < _localBlackboard.lookAtThreshold) //magnitude is too expensive to be calculating very often... should use squared numbers if we need more performance
        {
            _localBlackboard.navAI.updateRotation = false;
            return true;
        }
        else
        {
            _localBlackboard.navAI.updateRotation = true;
            return false;
        }
    }


    //Move this shit out of the movement branch you lazy fucker!!!
    public Vector3 ProcessTargetOffset(float offset)
    {
        Vector3 moveTarget = _localBlackboard.currentTarget.position - transform.position; //direction
        moveTarget = moveTarget.normalized * (moveTarget.magnitude - offset) + transform.position; //adjusted distance + direction gives us the Vector pointing at our moveTarget, adding it to our position gives us the moveTarget's world position       

        //this should probably return a move at all bool, don't wanna calc movement when it's not needed
        if ((transform.position - moveTarget).magnitude > _localBlackboard.movementSlopAllowance) //if distance between adjusted MoveTarget is greater than your slop allowance then move
            return moveTarget;
        else
            return transform.position;
    }
}