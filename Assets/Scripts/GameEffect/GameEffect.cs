using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameEffect : MonoBehaviour,IGameEffect
{

    [SerializeField] private ParticleSystem effectParticle;
    public ParticleSystem EffectParticle => effectParticle;

    private IObjectPool<IGameEffect> objectPool;

    [Header("LightSetting")]
    [SerializeField] private float hideSecond = 0.1f;
    [SerializeField] private Light effectLight;
    private WaitForSeconds waitHide;
    private Coroutine wait;

    private void Awake()
    {
        waitHide = new WaitForSeconds(hideSecond);
    }

    private void OnDisable()
    {
        Hide();
    }

    private void OnEnable()
    {
        if (effectLight != null)
        {
            effectLight.enabled = true;
            wait = StartCoroutine(HideLight());
        }
    }

    private IEnumerator HideLight()
    {
        yield return waitHide;
        effectLight.enabled = false;
    }

    public void Hide()
    {
        transform.localScale = Vector3.one;
        effectParticle.Stop(true);
        objectPool.Release(this);

        if (effectLight != null)
        {
            effectLight.enabled = false;
            StopCoroutine(wait);
        }
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
