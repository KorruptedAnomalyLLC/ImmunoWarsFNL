using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenDamage = 2f;

    private List<StatusManager> infectedUnits = new List<StatusManager>();
    private AttackRoot _attackRoot;


    public void Setup(AttackRoot attackRoot)
    {
        _attackRoot = attackRoot;
        StartCoroutine(DealDamageOverTime());
    }



    public void AddToList(StatusManager affectedUnit)
    {
        infectedUnits.Add(affectedUnit);
    }

    public void RemoveFromList(StatusManager affectedUnit)
    {
        if(infectedUnits.Contains(affectedUnit))
            infectedUnits.Remove(affectedUnit);
    }



    private IEnumerator DealDamageOverTime()
    {
        yield return new WaitForSeconds(timeBetweenDamage);
        //foreach(StatusManager unit in infectedUnits)
        //{
        //    if(unit != null)
        //        _attackRoot.HitUnit(unit);
        //}

        _attackRoot.RunAttack();
        yield return new WaitForSeconds(_attackRoot.hitTime);
        _attackRoot.EndAttack();

        StartCoroutine(DealDamageOverTime());
    }
}
