using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 追蹤型子彈
/// </summary>
public class FollowBullet : Bullet
{
    private float findRange = 100f;
    private const float startFollowTime = .5f;
    private bool startFollow = false;

    public override void SetBullet(float speed, float explosionRadius, float damage, float amount, float duration, DebuffType debuff, float existTime)
    {
        base.SetBullet(speed, explosionRadius, damage, amount, duration, debuff, existTime);

        Invoke(nameof(StartFollow), startFollowTime);
    }

    protected override void Move()
    {
        if (!startFollow)
            HaveYetToStartFollowMove();
        else
        {
            if(TargetIsDied())
            {
                target = enemyManager.FindNearestEnemy(transform.position, findRange);
                if (TargetIsDied())
                {
                    Hide();
                    return;
                }
            }

            StartFollowMove();
        }
    }

    private void HaveYetToStartFollowMove()
    {
        Vector3 dir = transform.forward;
        float distanceThisFrame = speed * Time.fixedDeltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void StartFollowMove()
    {
        Vector3 dir = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.fixedDeltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target.transform);
    }

    protected override void Hide()
    {
        CancelInvoke(nameof(StartFollow));
        startFollow = false;
        base.Hide();
    }

    private void StartFollow()
    {
        startFollow = true;
    }
}
