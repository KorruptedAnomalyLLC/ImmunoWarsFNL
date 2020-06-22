using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitBase : MonoBehaviour
{

    /// <summary>
    /// Temp, shit code... will get updated later this week 6/22/20
    /// </summary>
    
    [SerializeField]
    private GameObject ui;

    private NavMeshAgent navAI;

    private void Start()
    {
        navAI = GetComponent<NavMeshAgent>();
    }


    public void Selected()
    {
        ui.SetActive(true);
    }

    public void Drop()
    {
        ui.SetActive(false);
    }

    public void MoveTo(Vector3 targetPos)
    {
        navAI.SetDestination(targetPos);
    }
}
