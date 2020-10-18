using UnityEngine;
using UnityEngine.AI;

public class MovementRoot : MonoBehaviour
{

    //AI Tree Branch References
    private RandomPointGenerator randMovePoint; //not req

    private LocalBlackboard _localBlackboard; //req

    //Script variables
    [SerializeField]
    private Vector2 priorityRange = new Vector2(50, 60);

    private NavMeshAgent navAI; //req

    private bool rotating = false;

    public void Setup()
    {
        navAI = GetComponent<NavMeshAgent>();
        navAI.avoidancePriority = (int)Random.Range(priorityRange.x, priorityRange.y);
        _localBlackboard = GetComponent<LocalBlackboard>();

        if (TryGetComponent(out RandomPointGenerator temp))
        {
            randMovePoint = temp;
            randMovePoint.Setup();
        }
    }


    public void Selected()
    {
        navAI.speed = _localBlackboard.pSpeed;
        navAI.acceleration = _localBlackboard.pAcceleration;
    }

    public void Dropped()
    {
        navAI.speed = _localBlackboard.sSpeed;
        navAI.acceleration = _localBlackboard.sAcceleration;
    }

    public void TargetDropped()
    {
        navAI.updateRotation = true;
        rotating = false;
    }

    public void _update()
    {

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

        _randMoveUpdate(); //to be moved, likely behavior state will be a called function not checked every tick
    }

    //used for movement stuff/things that have to go every frame to keep game looking smooth
    public void Update()
    {
        if(rotating)
            RotateTo(_localBlackboard.currentTarget.position);
    }


    private void PatrolBranch()
    {
        if (randMovePoint != null)
            RandomMovement();
    }

    private void TargetObtainedBranch()
    {
        MoveWithTarget();
    }


    #region RandomMovement
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
    #endregion


    #region Move To Target
    //Using a fuck ton of inefficient math functions, clean this shit up!
    //maintain optimum attack distance based on the current attack
    //ToDo
    //Distance should be calculated outside of the movement branch, best to make a sight branch for choosing targets, etc?
    //this only accounts for this unit's attack goals, doesn't have anything in place for changing targets or fleeing.... flight or fight will involve a decision of attack or flee, which can mean combat movement or flee movement
    //will have to change the way states are read in the movement script, likely unitRoot will have to control this?
    private void MoveWithTarget()
    {
        //move to optimum distance, if already there don't move
        Vector3 moveTarget = ProcessTargetOffset(_localBlackboard.targetMovementOffset);
        MoveTo(moveTarget);
        //look at target
        if ((transform.position - moveTarget).magnitude < _localBlackboard.lookAtThreshold)
        {
            navAI.updateRotation = false;
            rotating = true;
            //RotateTo(_localBlackboard.currentTarget.position);
        }
        else
        {
            navAI.updateRotation = true;
            rotating = false;
        }

    }

    //rotates towards the passed in position
    private void RotateTo(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * (navAI.angularSpeed/200));
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
    #endregion


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
