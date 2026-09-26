using UnityEngine;

public class DragonAnimationEvents : MonoBehaviour
{
    [SerializeField] private DragonCombat combat;

    public void FireAttackHit()
    {
        if (!combat.IsFireAttackActive)
            return;

        combat.PerformFireAttack();
        combat.StartFireVFX();
    }

    public void FireAttackStop()
    {
        combat.StopFireVFX();
    }

    public void TailAttackHit()
    {
        combat.PerformTailAttack();
    }
}