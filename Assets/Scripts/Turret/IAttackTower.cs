using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackTower
{
    Vector3 LockOnPos { get;}
    (float amount, float duration) DebuffProcess();
    void FindEnemy();
    void FireToEnemy();
    void FireToSpecificPoint();
    void Shoot();
    void ChangeLockOnPos(Vector3 targetPos);
}
