using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class GameSEManager : MonoBehaviour
{
    [SerializeField] private GameSoundEffect seObj;

    [Header("Tower")]
    [SerializeField] private AudioClip buildEffect;
    [SerializeField] private AudioClip upgradeEffect;
    [SerializeField] private AudioClip removeEffect;
    [Header("Attack")]
    [SerializeField] private AudioClip machineGunAttackEffect;
    [SerializeField] private AudioClip rocketTowerAttackEffect;
    [SerializeField] private AudioClip laserTowerAttackEffect;
    [SerializeField] private AudioClip bulletEffect;
    [SerializeField] private AudioClip missileExplosionEffect;
    [SerializeField] private AudioClip windAttackEffect;
    [SerializeField] private AudioClip makeMoneyEffect;
    [Header("Enemy")]
    [SerializeField] private AudioClip enemyDeathEffect;

    [Header("MixerGroup")]
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup towerGroup;
    [SerializeField] private AudioMixerGroup enemyGroup;

    private IObjectPool<GameSoundEffect> seObjectPools;
    private int defaultCapacity = 20;
    private int maxCapacity = 500;
    private bool collectionCheck = true;
    // Start is called before the first frame update
    void Start()
    {
        seObjectPools = new ObjectPool<GameSoundEffect>(CreateSE, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxCapacity);
    }

    public void ShowEffect(object s, GameEvent.GameEffectShowEvent e)
    {
        var audioSetting = GetAudio(e.GameEffectType);
        if (audioSetting.mixerGroup != null && audioSetting.audioClip != null)
        {
            var newAudio = seObjectPools.Get();
            newAudio.transform.SetPositionAndRotation(e.Pos, Quaternion.identity);
            newAudio.transform.SetParent(transform);
            newAudio.SetAudio(audioSetting.mixerGroup,audioSetting.audioClip);
        }
    }

    private (AudioMixerGroup mixerGroup, AudioClip audioClip) GetAudio(GameEffectType effectType)
    {
        switch (effectType)
        {
            case GameEffectType.Remove:
                return (towerGroup, removeEffect);
            case GameEffectType.Build:
                return (towerGroup, buildEffect);
            case GameEffectType.Upgrade:
                return (towerGroup, buildEffect);

            case GameEffectType.BulletHit:
                return (towerGroup, bulletEffect);
            case GameEffectType.MachineGunAttack:
                return (towerGroup, machineGunAttackEffect);
            case GameEffectType.LaserTowerAttack:
                return (towerGroup, laserTowerAttackEffect);
            case GameEffectType.RocketTowerAttack:
                return (towerGroup, rocketTowerAttackEffect);
            case GameEffectType.MissileExplosion:
                return (towerGroup, missileExplosionEffect);
            case GameEffectType.WindTowerAttack:
                return (towerGroup, windAttackEffect);
            case GameEffectType.MakeMoney:
                return (towerGroup, makeMoneyEffect);

            case GameEffectType.EnemyDeath:
                return (enemyGroup, enemyDeathEffect);

            default:
                return (null,null);
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
    private GameSoundEffect CreateSE()
    {
        GameSoundEffect newSE = Instantiate(seObj);
        newSE.SetObjectPool(seObjectPools);

        return newSE;
    }

    // 將物件放回物件池
    private void OnReleaseToPool(GameSoundEffect pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // 從物件池中取出物件
    private void OnGetFromPool(GameSoundEffect pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    // 當超出物件池的上限時，將物件Destroy
    private void OnDestroyPooledObject(GameSoundEffect pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    #endregion
}
