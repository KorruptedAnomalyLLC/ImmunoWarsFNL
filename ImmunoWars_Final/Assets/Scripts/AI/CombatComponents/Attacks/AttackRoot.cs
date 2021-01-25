///
///Base Attack Script, effect components are attatched to this to make up the game's different attacks.
///Attacks should be made into prefabs
///...this script got waaay too big, need to chop it up some
///
using UnityEngine;



public class AttackRoot : MonoBehaviour
{
    #region Standard Attack Variables
    //the following variables should be read only... if I get time I will fix this
    [Tooltip("This is the overall combat value of the attack. Higher rating = more powerful attack.\n\nThe AI uses this value to pick the best attack when in combat.\n\nThis should be based on things such as damage amount, heal amount, special effects, etc. \n\nExclude cost from value as AI looks at that seperately.")]
    public float attackValue = 0f;
    [Tooltip("This is how long(in seconds) an attack is active for before being shut off by the AttackPlayer.\n\nIf this attack is played by IndieAttackPlayer instead of the standard, this value is ignored.")]
    public float hitTime = 0.6f;
    [Tooltip("This is how much energy the attack costs to use.\n\nIf an attack costs more energy than the unit has left it will die.\nThe AI will not use an attack that will kill it if there is another option.")]
    public float energyCost = 0f;

    [SerializeField, Tooltip("This is the distance the AI will try to be from it's target before it can attack.\n\nIt will maintain this distance while this attack is active.")]
    private float optimumAttackDistance = 1.6f;
    #endregion

    #region Effect Variables
    [HideInInspector]
    public Type attackType = Type.None; //this should stay in the typeInfuser script or the collector script
    [HideInInspector]
    public bool attackCharged = true;
    #endregion

    #region Target Variables
    [SerializeField]
    private bool onlyHitTarget = true;
    public bool targetHeroes = false;
    #endregion

    #region Components
    private AttackColliderController _attackCollider;
    private TypeInflictor _typeInflictor;
    private LocalBlackboard _localBlackboard;
    private DoDamage _doDamage;
    private Heal _heal;
    private Spawn _spawn;
    private PushInflictor _pushInflictor;
    private Collection _collection;
    private StunInflictor _stunInflictor;
    private DestroyAfterTime _destroyAfterTime;
    private Recharge _recharge;
    private IndieAttackPlayer _indieAttackPlayer;
    private AttackFXPlayer _attackFXPlayer;
    private DestroyOnHit _destroyOnHit;
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
        if (TryGetComponent(out AttackColliderController temp6))
        {
            _attackCollider = temp6;
            _attackCollider.Setup(_localBlackboard, this);
        }
        if (TryGetComponent(out DestroyAfterTime temp7))
        {
            _destroyAfterTime = temp7;
            _destroyAfterTime.Setup(_spawn);
        }
        if (TryGetComponent(out DoDamage temp8))
        {
            _doDamage = temp8;
        }
        if (TryGetComponent(out Heal temp9))
        {
            _heal = temp9;
        }
        if (TryGetComponent(out Recharge temp10))
        {
            _recharge = temp10;
            _recharge.Setup(this);
        }
        if (TryGetComponent(out IndieAttackPlayer temp11))
        {
            _indieAttackPlayer = temp11;
            _indieAttackPlayer.Setup(this);
        }
        if(TryGetComponent(out AttackFXPlayer temp12))
        {
            _attackFXPlayer = temp12;
        }
        if(TryGetComponent(out DestroyOnHit temp13))
        {
            _destroyOnHit = temp13;
        }
    }
    #endregion


    #region Run/End Attack
    public void RunAttack()
    {
        if(_spawn != null)
        {
            if(_spawn.spawnOnPlayAttack)
                _spawn.SpawnSomething();
        }

        if(_attackCollider != null)
        {
            _attackCollider.ActivateCollider(_localBlackboard.currentTarget._statusManager, onlyHitTarget, targetHeroes);
        }

        if(_recharge != null)
        {
            attackCharged = false;
            _recharge.RechargeAttack();
        }
        if(_attackFXPlayer != null)
        {
            _attackFXPlayer.PlayAttackFX();
        }

        _localBlackboard._statusManager.AdjustEnergy(-energyCost);
    }

    public void EndAttack()
    {
        if(_attackFXPlayer != null)
        {
            _attackFXPlayer.StopAttackFX();
        }

        if (_attackCollider != null)
        {
            _attackCollider.DeactivateCollider();
        }

        //choose next attack
        //only do this if unit isn't selected
        if (GlobalBlackboard.Instance.selectedUnit != _localBlackboard)
            _localBlackboard._attackManager.SelectAttack();
    }
    #endregion


    #region Hit Unit
    //apply all effects to the unit that was hit... unless its dead
    public void HitUnit(StatusManager unitHit)
    {
        if (unitHit._localBlackboard.dead) //don't want error messages popping up from beating a dead unit
            return;

        if (_spawn != null)
        {
            if (_spawn.spawnOnLandAttack)
                _spawn.SpawnSomething();
        }

        if (_doDamage != null)
        {
            //if this attack has a type, check if it matches the hit unit's type. If so multiply damage by the typeDamageMultiplier
            if (_typeInflictor != null && _typeInflictor.CompareTypes(_localBlackboard._statusManager.FindType(), unitHit.FindType()))
                _doDamage.DealDamage(unitHit, _localBlackboard, GlobalBlackboard.Instance.typeDamageMultiplier);
            else
                _doDamage.DealDamage(unitHit, _localBlackboard);
        }
        
        if (_heal != null)
        {
            _heal.HealUnit(unitHit, _localBlackboard);
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

        //just used for projectiles. Starts a countdown to kill this attack object
        if(_destroyAfterTime != null)
        {
            _destroyAfterTime.StartDestroyCountdown(this.gameObject);
        }
        if(_destroyOnHit != null)
        {
            _destroyOnHit.DestroyAttack(this.gameObject);
        }
    }
    #endregion



    //used by collection attack... seems kinda weird to have the function here tho
    //also collection should be an ability, not an attack
    public void UpdateAttackType(Type newType)
    {
        attackType = newType;
    }


    public void OnAttackSelected()
    {
        _localBlackboard.optimumAttackDistance = optimumAttackDistance;
    }
}
