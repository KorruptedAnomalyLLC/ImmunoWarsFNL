
using UnityEngine;
using UnityEngine.AI;

public class RandomFloatyMovement : MonoBehaviour
{
    private RandomPointGenerator _randMovePoint;
    [SerializeField]
    private float changeDist = 0.5f;
    [SerializeField]
    private float randMoveSpeed = 1f;



    public void Setup(LocalBlackboard localBlackboard)
    {
        if (TryGetComponent(out RandomPointGenerator temp))
        {
            _randMovePoint = temp;
            _randMovePoint.Setup(localBlackboard);
        }
    }



    public Vector3 EnterRandomMovement(NavMeshAgent navAI)
    {
        navAI.speed = randMoveSpeed;
        navAI.autoBraking = false;
        return _randMovePoint.GeneratePoint();
    }

    public Vector3 RandomMovement(NavMeshAgent navAI)
    {
        if (navAI.remainingDistance < changeDist)
            return _randMovePoint.GeneratePoint();
        else
            return Vector3.positiveInfinity;
    }
}
