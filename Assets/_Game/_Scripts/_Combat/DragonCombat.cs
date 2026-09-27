using System.Collections.Generic;
using UnityEngine;

public class DragonCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DragonHealth selfHealth;
    [SerializeField] private ParticleSystem fireVFX;

    [Header("Abilities")]
    [SerializeField] private DragonAbilityData fireAttack;
    [SerializeField] private DragonAbilityData tailAttack;
    [SerializeField] private DragonAbilityData flyAttack;

    [Header("Fire Attack")]
    [SerializeField] private Transform fireOrigin;
    [SerializeField] private float fireRadius = 1.5f;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private Color fireGizmoColor = Color.red;

    [Header("Tail Attack")]
    [SerializeField] private Transform tailHitPoint;
    [SerializeField] private float tailRadius = 2f;
    [SerializeField] private Color tailGizmoColor = Color.yellow;

    [Header("fly Attack")]
    [SerializeField] private Transform flyHitPoint;
    [SerializeField] private float flyRadius = 2.5f;
    [SerializeField] private Color flyGizmoColor = Color.cyan;

    public bool IsFireAttackActive { get; private set; }

    public DragonAbilityData FireAttack => fireAttack;
    public DragonAbilityData TailAttack => tailAttack;
    public DragonAbilityData FlyAttack => flyAttack;

    public bool TryUseFire()
    {
        return fireAttack.TryUse();
    }

    public bool TryUseTail()
    {
        return tailAttack.TryUse();
    }

    public bool TryUseFly()
    {
        return flyAttack.TryUse();
    }

    public void PerformFireAttack()
    {
        if (fireOrigin == null)
            return;

        Vector3 start = fireOrigin.position;

        Vector3 end =
            start + transform.forward * fireAttack.Range;

        Collider[] hits = Physics.OverlapCapsule(
            start,
            end,
            fireRadius,
            targetLayers
        );

        HashSet<DragonHealth> damagedTargets =
            new HashSet<DragonHealth>();

        foreach (Collider hit in hits)
        {
            DragonHealth targetHealth =
                hit.GetComponentInParent<DragonHealth>();

            if (targetHealth == null)
                continue;

            if (targetHealth == selfHealth)
                continue;

            if (targetHealth.IsDead)
                continue;

            if (!damagedTargets.Add(targetHealth))
                continue;

            targetHealth.TakeDamage(
                fireAttack.Damage
            );
        }
    }

    public void SetFireAttackActive(bool active)
    {
        IsFireAttackActive = active;

        if (!active)
            StopFireVFX();
    }

    public void StartFireVFX()
    {
        if (fireVFX == null)
            return;

        fireVFX.Play(true);
    }

    public void StopFireVFX()
    {
        if (fireVFX == null)
            return;

        fireVFX.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );
    }

    public void PerformTailAttack()
    {
        if (tailHitPoint == null)
            return;

        Collider[] hits = Physics.OverlapSphere(
            tailHitPoint.position,
            tailRadius,
            targetLayers
        );

        HashSet<DragonHealth> damagedTargets =
            new HashSet<DragonHealth>();

        foreach (Collider hit in hits)
        {
            DragonHealth targetHealth =
                hit.GetComponentInParent<DragonHealth>();

            if (targetHealth == null)
                continue;

            if (targetHealth == selfHealth)
                continue;

            if (targetHealth.IsDead)
                continue;

            if (!damagedTargets.Add(targetHealth))
                continue;

            targetHealth.TakeDamage(
                tailAttack.Damage
            );
        }
    }

    public void PerformFlyAttack()
    {
        if (flyHitPoint == null)
            return;

        Collider[] hits = Physics.OverlapSphere(
            flyHitPoint.position,
            flyRadius,
            targetLayers
        );

        HashSet<DragonHealth> damagedTargets =
            new HashSet<DragonHealth>();

        foreach (Collider hit in hits)
        {
            DragonHealth targetHealth =
                hit.GetComponentInParent<DragonHealth>();

            if (targetHealth == null)
                continue;

            if (targetHealth == selfHealth)
                continue;

            if (targetHealth.IsDead)
                continue;

            if (!damagedTargets.Add(targetHealth))
                continue;

            targetHealth.TakeDamage(
                flyAttack.Damage
            );
        }
    }

    public bool IsTargetInsideFlyRadius(DragonHealth target)
    {
        if (target == null ||
            target.Hurtbox == null ||
            flyHitPoint == null)
            return false;

        Vector3 closestPoint =
            target.Hurtbox.ClosestPoint(
                flyHitPoint.position
            );

        float distance = Vector3.Distance(
            flyHitPoint.position,
            closestPoint
        );

        return distance <= flyRadius;
    }

    public float GetDistanceToTarget(DragonHealth target)
    {
        if (target == null)
            return Mathf.Infinity;

        Vector3 targetPoint;

        if (target.Hurtbox != null)
        {
            targetPoint =
                target.Hurtbox.ClosestPoint(
                    transform.position
                );
        }
        else
        {
            targetPoint = target.transform.position;
        }

        Vector3 selfPosition = transform.position;

        selfPosition.y = 0f;
        targetPoint.y = 0f;

        return Vector3.Distance(
            selfPosition,
            targetPoint
        );
    }

    public bool IsTargetInTailRange(DragonHealth target)
    {
        return GetDistanceToTarget(target)
            <= tailAttack.Range;
    }
    public bool IsTargetInFireRange(DragonHealth target)
    {
        return GetDistanceToTarget(target)
            <= fireAttack.Range;
    }

    

    private void OnDrawGizmosSelected()
    {
        if (fireOrigin != null && fireAttack != null)
        {
            Gizmos.color = fireGizmoColor;

            Vector3 start = fireOrigin.position;
            Vector3 end =
                start + transform.forward * fireAttack.Range;

            Gizmos.DrawWireSphere(start, fireRadius);
            Gizmos.DrawWireSphere(end, fireRadius);

            Gizmos.DrawLine(
                start + transform.right * fireRadius,
                end + transform.right * fireRadius
            );

            Gizmos.DrawLine(
                start - transform.right * fireRadius,
                end - transform.right * fireRadius
            );
        }

        if (tailHitPoint != null)
        {
            Gizmos.color = tailGizmoColor;

            Gizmos.DrawWireSphere(
                tailHitPoint.position,
                tailRadius
            );
        }

        if (flyHitPoint != null)
        {
            Gizmos.color = flyGizmoColor;

            Gizmos.DrawWireSphere(
                flyHitPoint.position,
                flyRadius
            );
        }
    }
}