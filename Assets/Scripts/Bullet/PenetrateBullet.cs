using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
/// <summary>
/// 穿透子彈
/// </summary>
public class PenetrateBullet : Bullet
{
    private bool penetrateHitTarget = false;

    public override void SetBullet(float speed, float explosionRadius, float damage, float amount, float duration, DebuffType debuff, float existTime)
    {
        base.SetBullet(speed, explosionRadius, damage, amount, duration, debuff, existTime);

        if (rangeHitTargets == null)
            rangeHitTargets = new Collider[10];
    }

    protected override void Move()
    {
        if (OutRangeToHide())
            return;

        if (!TargetIsDied())
        {
            targetPos = target.transform.position;
            targetPos.y = transform.position.y;
        }
        Vector3 dir = penetrateHitTarget || TargetIsDied() || isSpecificPoint ? transform.forward : targetPos - transform.position;
        float distanceThisFrame = speed * Time.fixedDeltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void OnDisable()
    {
        penetrateHitTarget = false;
    }

    protected override void HitTarget(Transform target)
    {
        Damage(target);
    }

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.activeSelf && col.CompareTag("Enemy"))
        {
            HitTarget(col.transform);
            penetrateHitTarget = true;
        }
    }
}
