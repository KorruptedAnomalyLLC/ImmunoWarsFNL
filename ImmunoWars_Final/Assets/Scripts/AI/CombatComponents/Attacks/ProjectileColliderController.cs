
using UnityEngine;

public class ProjectileColliderController : MonoBehaviour
{
    private ProjectileRoot _projectileRoot;

    private StatusManager hitUnitsStatus;
    private LocalBlackboard hitUnitInfo;
    private Collider myCollider;
    private bool targetHeroes = false;


    public void Setup(Collider unitCollider, ProjectileRoot projectileRoot, bool aimAtAllies)
    {
        myCollider = unitCollider;
        _projectileRoot = projectileRoot;
        targetHeroes = aimAtAllies;
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider == myCollider)
            return;

        if(collider.transform.parent.TryGetComponent<LocalBlackboard>(out hitUnitInfo))
        {
            if(hitUnitInfo.heroUnit == targetHeroes)
            {
                _projectileRoot.DealDamage(hitUnitInfo._statusManager);
            }
        }
    }
}
