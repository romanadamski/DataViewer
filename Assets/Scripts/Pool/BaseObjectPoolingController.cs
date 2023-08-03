using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseObjectPoolingController<T> : MonoBehaviour where T : MonoBehaviour
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

    protected virtual T OnCreate()
    {
        var obj = Instantiate(prefab, parent);
        obj.name = obj.name.Replace("(Clone)", $"{obj.GetInstanceID()}");
        return obj;
    }

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
    public void ReleaseAll() => _outsidePool.ToList().ForEach(obj => Release(obj));
}
