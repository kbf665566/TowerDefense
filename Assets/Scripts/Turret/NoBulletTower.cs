using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class NoBulletTower : NormalTower, IAttackTower, ITowerRange
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LightningEffect lightningEffect;

    private WaitForSeconds wait = new WaitForSeconds(0.15f);
    private Coroutine hideEffectCoroutine;
    public override void SetTower(int uid, TowerData towerData, Vector2Short gridPos)
    {
        useBullet = false;
        
        lineRenderer.enabled = false;
        if (lightningEffect != null)
            lightningEffect.UpdateFromMaterialChange();

        base.SetTower(uid, towerData, gridPos);
    }

    public override void Shoot()
    {
        float amount = 0f;
        float duration = 0f;
        if (debuff == DebuffType.Stun)
        {
            amount = stunProbability;
            duration = stunDuration;

        }
        else if (debuff == DebuffType.Slow)
        {
            amount = slowAmount;
            duration = slowDuration;
        }

        targetEnemy.TakeDamage(final_Damage,amount,duration,debuff);
        EventHelper.EffectShowEvent.Invoke(this,GameEvent.GameEffectShowEvent.CreateEvent(firePoint.transform.position,towerData.AttackParticle));

        if (lightningEffect == null)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, targetEnemy.transform.position);
        }
        else
        {
            lightningEffect.SetPosition(firePoint.position, targetEnemy.transform.position);
            lightningEffect.enabled = true;
        }

        lineRenderer.enabled = true;

        hideEffectCoroutine = StartCoroutine(HideEffect());
    }

    private IEnumerator HideEffect()
    {
        yield return wait;
        lineRenderer.enabled = false;
        if(lightningEffect != null)
            lightningEffect.enabled = false;
    }

    private void OnDisable()
    {
        if (hideEffectCoroutine != null)
            StopCoroutine(hideEffectCoroutine);
        lineRenderer.enabled = false;
        if (lightningEffect != null)
            lightningEffect.enabled = false;
    }
}
