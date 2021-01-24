///
///This script allows an attack to play itself rather than rely on an Attack Player/entier AI system.
///This is useful for projectiles, explosions, environment effects, etc that need to be simpler than a standard attack setup
///

using UnityEngine;

public class IndieAttackPlayer : MonoBehaviour
{
    public void Setup(AttackRoot attackRoot)
    {
        attackRoot.RunAttack();
    }
}
