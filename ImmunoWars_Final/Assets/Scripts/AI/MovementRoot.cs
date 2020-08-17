using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class MovementRoot : MonoBehaviour
{
    private NavMeshAgent navAI; //req


    private RandomPointGenerator randMovePoint;

    //private UnitBase unitRoot; //req
    private LocalBlackboard _localBlackboard; //req

    [SerializeField]
    private Vector2 priorityRange = new Vector2(50, 60);

    public void Setup()
    {
        navAI = GetComponent<NavMeshAgent>();
        navAI.avoidancePriority = (int)Random.Range(priorityRange.x, priorityRange.y);
        //unitRoot = GetComponent<UnitBase>();
        _localBlackboard = GetComponent<LocalBlackboard>();

        if (TryGetComponent(out RandomPointGenerator temp))
        {
            randMovePoint = temp;
            randMovePoint.Setup();
        }
    }


    public void _update()
    {

        switch (_localBlackboard._behaviorState) 
        {
            case BehaviorState.Patrol:
                PatrolBranch();
                break;

            case BehaviorState.Combat:
                CombatBranch();
                break;

            case BehaviorState.PlayerControlled:
                PlayerMoveBranch();
                break;
         
            default:
                
                break;
        }

        _randMoveUpdate(); //to be moved, likely behavior state will be a called function not checked every tick
    }


    private void PatrolBranch()
    {
        if (randMovePoint != null)
            RandomMovement();
    }

    private void CombatBranch()
    {
        CombatMovement();
    }

    private void PlayerMoveBranch()
    {
        return;
    }

    //ToDo:
    //Turn into seperate behavior script, combine with RandPointGenerator,
    //make enter and exit events work
    [SerializeField]
    private float changeDist = 0.5f;
    private bool moving = false;
    [SerializeField]
    private float randMoveAcceleration = 1f;
    private void RandomMovement()
    {
        if (!moving)
        {
            navAI.speed = randMoveAcceleration;
            navAI.autoBraking = false;
            MoveTo(randMovePoint.GeneratePoint());
            moving = true;
        }
        else
        {
            if (navAI.remainingDistance < changeDist)
            {
                MoveTo(randMovePoint.GeneratePoint());
            }
        }
    }

    private void _randMoveUpdate()
    {
        if (_localBlackboard._behaviorState != BehaviorState.Patrol)
        {
            moving = false;
        }
    }

    //maintain optimum attack distance based on the current attack
    //ToDo
    //Distance should be calculated outside of the movement branch, best to make a sight branch for choosing targets, etc?
    //this only accounts for this unit's attack goals, doesn't have anything in place for changing targets or fleeing.... flight or fight will involve a decision of attack or flee, which can mean combat movement or flee movement
    //will have to change the way states are read in the movement script, likely unitRoot will have to control this?
    private void CombatMovement()
    {
        //move to optimum attack distance, if already there don't move
        Vector3 moveTarget = ProcessTargetOffset(_localBlackboard.optimumAttackDistance);
        MoveTo(moveTarget);
    }

    public Vector3 ProcessTargetOffset(float offset)
    {
        Vector3 moveTarget = transform.position - _localBlackboard.currentTarget.position; //direction
        moveTarget = moveTarget * (moveTarget.sqrMagnitude - offset); //adjusted distance + direction

        if ((transform.position - moveTarget).sqrMagnitude > _localBlackboard.movementSlopAllowance) //if distance between adjusted MoveTarget is greater than your slop allowance then move
        {
            return moveTarget;
        }
        else
            return transform.position;
    }

    public void MoveTo(Vector3 targetPos)
    {

        targetPos = new Vector3(targetPos.x, GlobalBlackboard.Instance.playfieldHeight, targetPos.z);
        navAI.SetDestination(targetPos);
    }

    public void StopMoving()
    {
        navAI.autoBraking = true;
        navAI.SetDestination(transform.position);
    }
}
