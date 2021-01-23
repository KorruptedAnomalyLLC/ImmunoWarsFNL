///
///Initializes Movement Components
///Handles Ticks
///Sends Commands to Nav AI System
///Handles selected/dropped commands
///Determines when new movement needs to be figured out and when unit should stay put
///

using UnityEngine;
using UnityEngine.AI;

public class MovementRoot : MonoBehaviour
{

    //AI Tree Branch References
    private MoveWithTarget _moveTarget; //not req
    private RandomFloatyMovement _randMove; //not req
    private NavAIPrioritySetter _navAIPriority; //not req
    private LocalBlackboard _localBlackboard; //req

    private bool rotating = false;


    #region Setup
    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;
        _localBlackboard.navAI = GetComponent<NavMeshAgent>();


        if (TryGetComponent(out NavAIPrioritySetter temp))
        {
            _navAIPriority = temp;
            _navAIPriority.SetUpAvoidancePriority(_localBlackboard.navAI);
        }
        if (TryGetComponent(out RandomFloatyMovement temp2))
        {
            _randMove = temp2;
            _randMove.Setup(_localBlackboard);
        }
        if (TryGetComponent(out MoveWithTarget temp3))
        {
            _moveTarget = temp3;
            _moveTarget.Setup(_localBlackboard);
        }

        BehaviorStateChanged();
    }
    #endregion



    #region Ticks
    public void _update()
    {
        if (_localBlackboard.isStunned)
            return;

        switch (_localBlackboard._behaviorState) 
        {
            case BehaviorState.Patrol:
                PatrolBranch();
                break;
         
            default:
                if (_localBlackboard.hasTarget)
                    TargetObtainedBranch();
                break;
        }
    }

    //used for movement stuff/things that have to go every frame to keep game looking smooth
    public void _smoothUpdate()
    {
        if (_localBlackboard.isStunned)
            return;

        if (rotating && _localBlackboard.currentTarget != null)
            RotateTo(_localBlackboard.currentTarget.transform.position);
    }
    #endregion

    #region Actions related to other scripts
    //Update Movement stuff when the unit's behaviour state changes
    public void BehaviorStateChanged()
    {
        switch (_localBlackboard._previousState)
        {
            case BehaviorState.Patrol:
                ExitPatrol();
                break;
            default:
                break;
        }

        switch (_localBlackboard._behaviorState)
        {
            case BehaviorState.Patrol:
                EnterPatrol();
                break;
            case BehaviorState.Combat:
                _moveTarget.UpdateTargetOffset(_localBlackboard.optimumAttackDistance);
                break;
            case BehaviorState.FollowFriendlyUnit:
                _moveTarget.UpdateTargetOffset(_localBlackboard.personalSpace);
                break;
            case BehaviorState.PlayerControlled:
                break;
            default:
                break;
        }
    }


    public void AttackStateChanged()
    {
        _moveTarget.UpdateTargetOffset(_localBlackboard.optimumAttackDistance);
    }

    public void Selected()
    {
        _localBlackboard.navAI.speed = _localBlackboard.pSpeed;
        _localBlackboard.navAI.acceleration = _localBlackboard.pAcceleration;
        StopMoving();
    }

    public void Dropped()
    {
        _localBlackboard.navAI.speed = _localBlackboard.sSpeed;
        _localBlackboard.navAI.acceleration = _localBlackboard.sAcceleration;
    }

    public void TargetDropped()
    {
        _localBlackboard.navAI.updateRotation = true;
        rotating = false;
    }
    #endregion

    #region Patrol Branch
    private void EnterPatrol()
    {
        if (_randMove != null)
            MoveTo(_randMove.EnterRandomMovement(_localBlackboard.navAI));

        //Assures that unit will return to rotating based on it's path
        rotating = false;
        _localBlackboard.navAI.updateRotation = true;
    }

    private void PatrolBranch()
    {
        if (_randMove != null)
        {
            Vector3 temp = _randMove.RandomMovement(_localBlackboard.navAI); //allocating mem here, can we get rid of this for a permanent Vector3?
            if (temp != Vector3.positiveInfinity)
                MoveTo(temp);
        }
    }

    private void ExitPatrol()
    {
        _localBlackboard.navAI.speed = _localBlackboard.sSpeed;
        _localBlackboard.navAI.acceleration = _localBlackboard.sAcceleration;
    }
    #endregion

    #region Target Obtained Branch
    Vector3 tempMoveTarget;
    private void TargetObtainedBranch()
    {
        if (_moveTarget == null)
            return;

        tempMoveTarget = _moveTarget.GrabTargetMovePos();
        MoveTo(tempMoveTarget);
        rotating = _moveTarget.RotationBasedOnTarget(tempMoveTarget);
    }
    #endregion


    #region Move Commands
    public void MoveTo(Vector3 targetPos)
    {

        targetPos = new Vector3(targetPos.x, GlobalBlackboard.Instance.playfieldHeight, targetPos.z);
        _localBlackboard.navAI.SetDestination(targetPos);
    }

    public void StopMoving()
    {
        _localBlackboard.navAI.autoBraking = true;
        _localBlackboard.navAI.SetDestination(transform.position);
    }

    //rotates towards the passed in position
    private void RotateTo(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * (_localBlackboard.navAI.angularSpeed / 200));
    }

    public void MoveToTouchPos(Vector3 target)
    {
            MoveTo(target);
    }
    #endregion
}
