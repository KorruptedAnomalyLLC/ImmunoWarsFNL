///
///Base Attack Script, effect components are attatched to this to make up the game's different attacks.
///Attacks should be made into prefabs
///...this script got waaay too big, need to chop it up some
///
using System.Collections;
using UnityEngine;



public class AttackRoot : MonoBehaviour
{
    #region Standard Attack Variables
    public GameObject hitBox;
    public GameObject attackFX;
    public float damageAmount = 1;
    public float hitTime = 0.6f;
    public float energyCost = 0f;
    public bool attackCharged = true;
    public float rechargeTime = 1f;
    public float optimumAttackDistance = 1.6f;
    #endregion

    #region Effect Variables
    public Type colorType = Type.None;
    [SerializeField]
    private int numberToSpawn = 1;
    #endregion

    #region Target Variables
    [SerializeField]
    private bool onlyHitTarget = true;
    [SerializeField]
    private bool targetHeroes = false;
    #endregion

    #region Components
    [SerializeField]
    private AttackColliderController _attackCollider;
    [SerializeField]
    private TypeInflictor _typeInflictor;
    private LocalBlackboard _localBlackboard;
    private Spawn _spawn;
    private PushInflictor _pushInflictor;
    private Collection _collection;
    private StunInflictor _stunInflictor;
    #endregion

    #region Setup
    public void Setup(LocalBlackboard localBlackboard) 
    {
        _localBlackboard = localBlackboard;

        if (TryGetComponent(out TypeInflictor temp))
        {
            _typeInflictor = temp;
        }
        if (TryGetComponent(out Spawn temp2))
        {
            _spawn = temp2;
            _spawn.Setup(_localBlackboard);
        }
        if (TryGetComponent(out PushInflictor temp3))
        {
            _pushInflictor = temp3;
        }
        if (TryGetComponent(out Collection temp4))
        {
            _collection = temp4;
            _collection.Setup(this);
        }
        if (TryGetComponent(out StunInflictor temp5))
        {
            _stunInflictor = temp5;
        }
        if (hitBox.gameObject.TryGetComponent(out AttackColliderController temp6))
        {
            _attackCollider = temp6;
            _attackCollider.Setup(_localBlackboard, this);
        }
        else
            Debug.LogError(gameObject.name + " is Missing AttackColliderController script, please attatch reference to AttackRoot in inspector.");
    }
    #endregion


    //used by collection attack... seems kinda weird to have the function here tho
    public void UpdateAttackType(Type newType)
    {
        colorType = newType;
    }

    private StatusManager targetStatus;
    //private List<UnitRoot> unitsHit = new List<UnitRoot>();
    public void RunAttack(LocalBlackboard currentTarget)
    {
        if(_spawn != null) //for spawn attacks
        {
            _spawn.SpawnSomething(numberToSpawn);
        }
        else //for standard attacks
        {
            hitBox.SetActive(true);
            targetStatus = currentTarget._statusManager;
            _attackCollider.ActivateCollider(_localBlackboard.currentTarget._statusManager, onlyHitTarget, targetHeroes);
        }


        attackFX.SetActive(true);
        attackCharged = false;
        _localBlackboard._statusManager.AdjustEnergy(-energyCost);
        StartCoroutine(Recharge());
    }

    float damageToDeal;
    public void DealDamage(StatusManager unitHit)
    {
        if (unitHit._localBlackboard.dead) //don't want error messages popping up by beating a dead unit
            return;

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

        //Apply Push back effect if one is added
        if (_pushInflictor != null)
        {
            _pushInflictor.ApplyPushBack(unitHit._localBlackboard.navAI, _localBlackboard.transform.position);
        }

        if(_stunInflictor != null)
        {
            _stunInflictor.InflictStun(unitHit);
        }
    }

    public void EndAttack()
    {
        attackFX.SetActive(false);
        hitBox.SetActive(false);

        //choose next attack
        //only do this if unit isn't selected
        if (GlobalBlackboard.Instance.selectedUnit != _localBlackboard)
            _localBlackboard._attackManager.SelectAttack();
    }

    #region Attack Recharge
    private IEnumerator Recharge()
    {
        yield return new WaitForSeconds(rechargeTime);
        attackCharged = true;
    }
    #endregion
}
