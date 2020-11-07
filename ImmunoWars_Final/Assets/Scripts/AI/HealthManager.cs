using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    LocalBlackboard _localBlackboard;

    public void Start()
    {
        _localBlackboard = GetComponent<LocalBlackboard>();
    }


    public void TakeDamage(int damageTaken)
    {
        _localBlackboard.energyLevel -= damageTaken;

        if(_localBlackboard.energyLevel <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
