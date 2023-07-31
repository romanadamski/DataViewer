using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BaseObjectPoolingController<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    private T prefab;
    [SerializeField]
    private Transform parent;

    private ObjectPool<T> _objectPool;
    private List<T> _outsidePool;

    protected void Init(int defaultCapacity = 10, int maxSize = 20, bool collectionChecks = false)
    {
        _objectPool = new ObjectPool<T>(
            OnCreate,
            OnGet,
            OnObjectRelease,
            OnObjectDestroy,
            collectionChecks,
            defaultCapacity,
            maxSize);

        _outsidePool = new List<T>();
    }

    protected virtual T OnCreate() => Instantiate(prefab, parent);
    protected virtual void OnGet(T obj)
    {
        obj.gameObject.SetActive(true);
        _outsidePool.Add(obj);
    }
    protected virtual void OnObjectRelease(T obj)
    {
        obj.gameObject.SetActive(false);
        _outsidePool.Remove(obj);
    }
    protected virtual void OnObjectDestroy(T obj)
    {
        _outsidePool.Remove(obj);
        Destroy(obj);
    }

    public T Get() => _objectPool.Get();
    public void Release(T obj) => _objectPool.Release(obj);
    public void ReleaseAll()
    {
        //reverse order, cause ObjectPool is stack based
        for (int i = _outsidePool.Count - 1; i >= 0; i--)
        {
            Release(_outsidePool[i]);
        }
    }
}
