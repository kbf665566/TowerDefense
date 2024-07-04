using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class GameEffectManager : MonoBehaviour
{

    [Header("Tower")]
    [SerializeField] private GameEffect buildEffect;
    [SerializeField] private GameEffect upgradeEffect;
    [SerializeField] private GameEffect removeEffect;
    [Header("Attack")]
    [SerializeField] private GameEffect machineGunAttackEffect;
    [SerializeField] private GameEffect rocketTowerAttackEffect;
    [SerializeField] private GameEffect laserTowerAttackEffect;
    [SerializeField] private GameEffect bulletEffect;
    [SerializeField] private GameEffect missileExplosionEffect;
    [SerializeField] private GameEffect windAttackEffect;
    [SerializeField] private GameEffectWithText makeMoneyEffect;
    [Header("Enemy")]
    [SerializeField] private GameEffect enemyDeathEffect;


    private GameEffectType nowShowEffectType;
    private Dictionary<GameEffectType, IObjectPool<IGameEffect>> effectObjectPools;
    private int defaultCapacity = 20;
    private int maxCapacity = 100;
    private bool collectionCheck = true;
    // Start is called before the first frame update
    void Start()
    {
        effectObjectPools = new Dictionary<GameEffectType, IObjectPool<IGameEffect>>();
        for (int i = 0; i < Enum.GetValues(typeof(GameEffectType)).Length; i++)
            effectObjectPools.Add((GameEffectType)Enum.GetValues(typeof(GameEffectType)).GetValue(i), new ObjectPool<IGameEffect>(CreateEffect, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxCapacity));
    }

    public void ShowEffect(object s, GameEvent.GameEffectShowEvent e)
    {
        nowShowEffectType = e.GameEffectType;
        if(effectObjectPools.TryGetValue(e.GameEffectType,out var pool))
        {
            var newEffect = pool.Get();
            newEffect.GetTransform().SetPositionAndRotation(e.Pos, newEffect.GetTransform().rotation);
            newEffect.GetTransform().SetParent(transform);
            if (e.Size > 1)
                newEffect.GetTransform().localScale = new Vector3(e.Size,e.Size,1);
        }
    }

    public void ShowTextEffect(object s, GameEvent.GameEffectShowWithTextEvent e)
    {
        nowShowEffectType = e.GameEffectType;
        if (effectObjectPools.TryGetValue(e.GameEffectType, out var pool))
        {
            var newEffect = (GameEffectWithText)(pool.Get());
            newEffect.StartEffect(e.ShowText);
            newEffect.GetTransform().SetPositionAndRotation(e.Pos, newEffect.GetTransform().rotation);
            newEffect.GetTransform().SetParent(transform);
        }
    }

    private IGameEffect GetEffect(GameEffectType effectType)
    {
        switch(effectType)
        {
            case GameEffectType.Remove:
                return removeEffect;
            case GameEffectType.Build:
                return buildEffect;
            case GameEffectType.Upgrade:
                return upgradeEffect;

            case GameEffectType.BulletHit:
                return bulletEffect;
            case GameEffectType.MachineGunAttack:
                return machineGunAttackEffect;
            case GameEffectType.LaserTowerAttack:
                return laserTowerAttackEffect;
            case GameEffectType.RocketTowerAttack:
                return rocketTowerAttackEffect;
            case GameEffectType.MissileExplosion:
                return missileExplosionEffect;
            case GameEffectType.WindTowerAttack:
                return windAttackEffect;
            case GameEffectType.MakeMoney:
                return makeMoneyEffect;

            case GameEffectType.EnemyDeath:
                return bulletEffect;

            default:
                return null;
        }
    }

    private void OnEnable()
    {
        EventHelper.EffectShowEvent += ShowEffect;
        EventHelper.EffectShowTextEvent += ShowTextEffect;
    }

    private void OnDisable()
    {
        EventHelper.EffectShowEvent -= ShowEffect;
        EventHelper.EffectShowTextEvent -= ShowTextEffect;
    }

    #region  物件池
    // 物件池中的物件不夠時，建立新的物件去填充物件池
    private IGameEffect CreateEffect()
    {
        var particle = GetEffect(nowShowEffectType);
        IGameEffect newEffect = (IGameEffect)Instantiate((Object)particle);

        if (effectObjectPools.TryGetValue(nowShowEffectType, out var pool))
            newEffect.SetObjectPool(pool);

        return newEffect;
    }

    // 將物件放回物件池
    private void OnReleaseToPool(IGameEffect pooledObject)
    {
        pooledObject.GetGameObject().SetActive(false);
    }

    // 從物件池中取出物件
    private void OnGetFromPool(IGameEffect pooledObject)
    {
        pooledObject.GetGameObject().SetActive(true);
    }

    // 當超出物件池的上限時，將物件Destroy
    private void OnDestroyPooledObject(IGameEffect pooledObject)
    {
        Destroy(pooledObject.GetGameObject());
    }

    #endregion
}

public enum GameEffectType
{
    Remove = 0,
    Build = 1,
    Upgrade = 2,

    BulletHit = 100,
    MachineGunAttack = 101,
    RocketTowerAttack = 102,
    LaserTowerAttack = 103,
    MissileExplosion = 104,
    WindTowerAttack = 105,
    MakeMoney = 106,

    EnemyDeath = 300,
   
}