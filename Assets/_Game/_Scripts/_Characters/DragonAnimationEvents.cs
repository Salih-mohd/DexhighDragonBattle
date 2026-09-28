using UnityEngine;

public class DragonAnimationEvents : MonoBehaviour
{
    [SerializeField] private DragonCombat combat;
    [SerializeField] private DragonAudio dragonAudio;

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
    public void PlayFireSFX()
    {
        dragonAudio.PlayFire();
    }

    public void TailAttackHit()
    {
        combat.PerformTailAttack();
    }
}