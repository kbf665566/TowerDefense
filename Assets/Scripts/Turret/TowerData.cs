using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class TowerData 
{
    public int Id;
    public string Name;
    public List<TowerLevelData> TowerLevelData;
    public Sprite TowerIcon;
    public TowerType towerType;
    /// <summary> �O�_�ϥεo�g�� </summary>
    public bool IsUseBullet;
    public bool CanSlowEnemy;
    public bool CanStunEnemy;
    public string TowerInformation;
}

[Serializable]
public enum TowerType
{
    /// <summary> �@�몺������ </summary>
    Normal,
    /// <summary> ���U�� </summary>
    Support,
    /// <summary> �d������� </summary>
    AOE,
    /// <summary> �W�[�귽���� </summary>
    Money
}
[Serializable]
public class TowerLevelData
{
    /// <summary> ���� </summary>
    public float Damage;
    /// <summary> �g�{ </summary>
    public float ShootRange;
    /// <summary> ��t </summary>
    public float FireRate;
    /// <summary> �ϥΪ��o�g�� </summary>
    public Bullet TowerBullet;
    /// <summary> �o�g���t�� </summary>
    public float BulletSpeed;
    /// <summary> �o�g���z���d�� </summary>
    public float BulletExplosionRadius;

    //Support��
    /// <summary> Buff�W�[������ </summary>
    public float BuffAddDamage;
    /// <summary> Buff�W�[���d�� </summary>
    public float BuffAddRange;
    /// <summary> Buff�W�[����t </summary>
    public float BuffAddFireRate;

    //Money��
    /// <summary> �C��Ĳ�o����o�쪺�귽 </summary>
    public int GetMoney;

    //Debuff
    /// <summary> �w�t���{�� </summary>
    public float SlowAmount;
    /// <summary> �w�t������ɶ� </summary>
    public float SlowDuration;

    /// <summary> ���w�����v </summary>
    public float StunProbability;
    /// <summary> ���w������ɶ� </summary>
    public float StunDuration;

    //����
    public int BuildUpgradeCost;
    public int SoldPrice;

    public GameObject towerPrefab;
}