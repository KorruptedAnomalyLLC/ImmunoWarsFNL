///
///Pretty much a lightweight version of AttackRoot
///
using System.Collections;
using UnityEngine;

public class ProjectileRoot : MonoBehaviour
{
    #region Damage Variables
    public Type colorType = Type.None;
    public float damageAmount = 1;
    public float hitTime = 0.6f;
    [SerializeField]
    private int numberToSpawn = 1;
    private float damageToDeal;
    #endregion

    #region Component Variables
    [SerializeField]
    private ProjectileColliderController _projectileColliderController;
    [SerializeField]
    private TypeInflictor _typeInflictor;
    private LocalBlackboard _localBlackboard;
    private PushInflictor _pushInflictor;
    private StatusManager hitUnitsStatus;
    private StunInflictor _stunInflictor;
    #endregion

    #region Collision Variables
    [SerializeField]
    private Collider myCollider;
    private Collider parentCollider;
    [SerializeField]
    private GameObject projectileFX;
    [SerializeField]
    private GameObject explosionFX;
    #endregion

    [SerializeField]
    private float lifeTimeAfterHit = 3f;
    private bool targetHeroes = false;


    #region Setup
    public void Setup(LocalBlackboard localBlackboard, Type newType)
    {
        _localBlackboard = localBlackboard;
        colorType = newType;
        parentCollider = _localBlackboard.healthCollider;

        if (TryGetComponent(out TypeInflictor temp))
        {
            _typeInflictor = temp;
        }
        if (TryGetComponent(out PushInflictor temp3))
        {
            _pushInflictor = temp3;
        }
        if (TryGetComponent(out ProjectileColliderController temp4))
        {
            _projectileColliderController = temp4;
            _projectileColliderController.Setup(myCollider, this, targetHeroes);
        }
        if (TryGetComponent(out StunInflictor temp5))
        {
            _stunInflictor = temp5;
        }
    }
    #endregion


    //kinda hard to keep track of, maybe split this into an enemy interface manager that takes in damage/heal/other messages from outside sources
    //and sends them out as needed?
    public void DealDamage(StatusManager unitHit)
    {
        myCollider.enabled = false;
        projectileFX.SetActive(false);
        explosionFX.SetActive(true);

        //Damage
        damageToDeal = damageAmount;
        if (damageToDeal < 0)
        {
            unitHit.AdjustEnergy(damageToDeal); //heal units instead of damaging them
            //heal yourself too? _localBlackboard._healthManager.AdjustEnergy(damageToDeal);
        }
        else
        {
            //if this attack has a type, check if it matches the hit unit's type. If so multiply damage by the typeDamageMultiplier
            if (_typeInflictor != null && _typeInflictor.CompareTypes(_localBlackboard._statusManager.FindType(), unitHit.FindType()))
                damageToDeal *= GlobalBlackboard.Instance.typeDamageMultiplier;


            unitHit.TakeDamage(damageToDeal, _localBlackboard);
        }

        //Status Effects
        //Apply Push back effect if one is added
        if (_pushInflictor != null)
        {
            _pushInflictor.ApplyPushBack(unitHit._localBlackboard.navAI, _localBlackboard.transform.position);
        }
        //apply stun effect if component was added
        if (_stunInflictor != null)
        {
            _stunInflictor.InflictStun(unitHit);
        }

        //start countdown to destroy this projectile
        StartCoroutine(KillProjectile());
    }


    private IEnumerator KillProjectile()
    {
        yield return new WaitForSeconds(lifeTimeAfterHit);
        Destroy(this);
    }
}