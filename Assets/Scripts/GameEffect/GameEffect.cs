using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameEffect : MonoBehaviour
{

    [SerializeField] private ParticleSystem effectParticle;
    public ParticleSystem EffectParticle => effectParticle;

    private IObjectPool<GameEffect> objectPool;
    public IObjectPool<GameEffect> ObjectPool { set => objectPool = value; }

    private void OnDisable()
    {
        effectParticle.Stop(true);
        objectPool.Release(this);
    }
}
