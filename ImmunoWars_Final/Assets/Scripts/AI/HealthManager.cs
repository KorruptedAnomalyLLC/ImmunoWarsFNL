using UnityEngine;

public class HealthManager : MonoBehaviour
{
    LocalBlackboard _localBlackboard;
    UnitRoot _unitRoot;

    public void Start()
    {
        _unitRoot = GetComponentInParent<UnitRoot>();
        _localBlackboard = GetComponentInParent<LocalBlackboard>();
    }


    public void TakeDamage(int damageTaken, Transform attacker)
    {
        _localBlackboard.energyLevel -= damageTaken;

        if (!_localBlackboard.hasTarget)
        {
            _unitRoot.UpdateTarget(attacker, true);
        }

        if(_localBlackboard.energyLevel <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(GlobalBlackboard.Instance.selectedUnit == _unitRoot)
            UIManager.Instance.ButtonClicked(ButtonType.DropUnit);

        Destroy(this.transform.parent.gameObject);
    }
}
