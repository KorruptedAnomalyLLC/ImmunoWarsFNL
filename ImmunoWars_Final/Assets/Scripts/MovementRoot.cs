using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

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
        }
    }


    public void _update()
    {

        switch (_localBlackboard._behaviorState) 
        {
            case BehaviorState.Patrol:
                PatrolBranch();
                break;

            default:
                Debug.LogError("U Fuck up big time! Very Very Stupid");
                break;
        }

        _randMoveUpdate(); //to be moved, likely behavior state will be a called function not checked every tick
    }


    private void PatrolBranch()
    {
        if (randMovePoint != null)
            RandomMovement();
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

    private void Combat()
    {
        //move to optimum attack distance
    }


    public void MoveTo(Vector3 targetPos)
    {
        navAI.SetDestination(targetPos);
    }

    public void StopMoving()
    {
        navAI.autoBraking = true;
        navAI.SetDestination(transform.position);
    }
}
