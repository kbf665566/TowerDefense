using UnityEngine;
using UnityEngine.Pool;

public interface IGameEffect
{
    void Hide();
    Transform GetTransform();
    GameObject GetGameObject();
    void SetObjectPool(IObjectPool<IGameEffect> objectPool);
}
