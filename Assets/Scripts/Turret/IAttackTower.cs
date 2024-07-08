using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackTower
{
    (float amount, float duration) DebuffProcess();
    void FindEnemy();
    void FireToEnemy();
    void Shoot();
}
