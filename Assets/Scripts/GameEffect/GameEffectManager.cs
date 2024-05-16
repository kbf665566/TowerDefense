using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffectManager : MonoBehaviour
{

    [Header("Tower")]
    [SerializeField] private ParticleSystem buildEffect;
    [SerializeField] private ParticleSystem upgradeEffect;
    [SerializeField] private ParticleSystem removeEffect;
    [Header("Attack")]
    [SerializeField] private ParticleSystem machineGunAttackEffect;
    [SerializeField] private ParticleSystem rocketTowerAttackEffect;
    [SerializeField] private ParticleSystem laserTowerAttackEffect;
    [SerializeField] private ParticleSystem boomEffect;
    [Header("Enemy")]
    [SerializeField] private ParticleSystem enemyDeathEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ShowEffect(object s, GameEvent.GameEffectShowEvent e)
    {
        var effect = GetEffect(e.GameEffectType);
        var particle = Instantiate(effect,e.Pos,Quaternion.identity,transform);
        Destroy(particle, particle.main.duration + 2f);
    }

    private ParticleSystem GetEffect(GameEffectType effectType)
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