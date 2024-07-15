using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
/// <summary>
/// 一般子彈
/// </summary>
public class NormalBullet : Bullet
{
    protected override void Move()
    {
        if (TargetIsDied())
            NoTargetMove();
        else
            HaveTargetMove();
    }

    private void NoTargetMove()
    {
        if (OutRangeToHide())
            return;    

        Vector3 dir = transform.forward;
        float distanceThisFrame = speed * Time.fixedDeltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void HaveTargetMove()
    {
        Vector3 dir = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.fixedDeltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target.transform);
    }
}
