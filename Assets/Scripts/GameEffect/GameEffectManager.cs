using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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
    [SerializeField] private GameEffect boomEffect;
    [Header("Enemy")]
    [SerializeField] private GameEffect enemyDeathEffect;


    private GameEffectType nowShowEffectType;
    private Dictionary<GameEffectType, IObjectPool<GameEffect>> effectObjectPools;
    private int defaultCapacity = 20;
    private int maxCapacity = 100;
    private bool collectionCheck = true;
    // Start is called before the first frame update
    void Start()
    {
        effectObjectPools = new Dictionary<GameEffectType, IObjectPool<GameEffect>>();
        for (int i = 0; i < 8; i++)
            effectObjectPools.Add((GameEffectType)i, new ObjectPool<GameEffect>(CreateEffect,
            OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxCapacity));
    }

    public void ShowEffect(object s, GameEvent.GameEffectShowEvent e)
    {
        nowShowEffectType = e.GameEffectType;
        if(effectObjectPools.TryGetValue(e.GameEffectType,out var pool))
        {
            var newEffect = pool.Get();
            newEffect.transform.SetPositionAndRotation(e.Pos, Quaternion.identity);
            newEffect.transform.SetParent(transform);
        }
    }

    private GameEffect GetEffect(GameEffectType effectType)
    {
        switch(effectType)
        {
            case GameEffectType.Remove:
                return removeEffect;
            case GameEffectType.Build:
                return buildEffect;
            case GameEffectType.Upgrade:
                return upgradeEffect;

            case GameEffectType.Boom:
                return boomEffect;
            case GameEffectType.MachineGunAttack:
                return machineGunAttackEffect;
            case GameEffectType.LaserTowerAttack:
                return laserTowerAttackEffect;
            case GameEffectType.RocketTowerAttack:
                return rocketTowerAttackEffect;

            case GameEffectType.EnemyDeath:
                return boomEffect;

            default:
                return null;
        }
    }

    private void OnEnable()
    {
        EventHelper.EffectShowEvent += ShowEffect;
    }

    private void OnDisable()
    {
        EventHelper.EffectShowEvent -= ShowEffect;
    }

    #region  物件池
    // 物件池中的物件不夠時，建立新的物件去填充物件池
    private GameEffect CreateEffect()
    {
        var particle = GetEffect(nowShowEffectType);
        GameEffect newEffect = Instantiate(particle);

        if (effectObjectPools.TryGetValue(nowShowEffectType, out var pool))
            newEffect.ObjectPool = pool;

        return newEffect;
    }

    // 將物件放回物件池
    private void OnReleaseToPool(GameEffect pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // 從物件池中取出物件
    private void OnGetFromPool(GameEffect pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    // 當超出物件池的上限時，將物件Destroy
    private void OnDestroyPooledObject(GameEffect pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    #endregion
}

public enum GameEffectType
{
    Remove,
    Build,
    Upgrade,

    Boom,
    MachineGunAttack,
    RocketTowerAttack,
    LaserTowerAttack,

    EnemyDeath,
   
}