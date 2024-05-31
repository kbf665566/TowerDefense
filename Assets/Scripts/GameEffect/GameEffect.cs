using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameEffect : MonoBehaviour,IGameEffect
{

    [SerializeField] private ParticleSystem effectParticle;
    public ParticleSystem EffectParticle => effectParticle;

    private IObjectPool<IGameEffect> objectPool;

    private void OnDisable()
    {
        Hide();
    }

    public void Hide()
    {
        transform.localScale = Vector3.one;
        effectParticle.Stop(true);
        objectPool.Release(this);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void SetObjectPool(IObjectPool<IGameEffect> objectPool)
    {
        this.objectPool = objectPool; ;
    }
}
