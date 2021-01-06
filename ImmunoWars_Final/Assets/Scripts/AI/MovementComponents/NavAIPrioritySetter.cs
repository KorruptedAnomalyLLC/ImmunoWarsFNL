///
///This script Sets the rank a navAI has in pushing others out of it's way
///We need to assure that no two AI have the same rank else they could get stuck on eachother for a time which looks buggy.
///

using UnityEngine;
using UnityEngine.AI;

public class NavAIPrioritySetter : MonoBehaviour
{
    [SerializeField, Tooltip("Avoidance Priority range that this type of unit can have. \nThis determines which unit gets out of the way when two are going to collide and whcih pushes through.")]
    private Vector2 priorityRange = new Vector2(50, 60);

    public void SetUpAvoidancePriority(NavMeshAgent navAI)
    {
        navAI.avoidancePriority = (int)Random.Range(priorityRange.x, priorityRange.y); //weak implementation, doesn't assure no two AI have the same priority... worth fixing?
    }
}
