using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Base class for pooling objects using UnityEngine's ObjectPool mechanism, stack based.
/// </summary>
/// <typeparam name="T">MonoBehaviour's type we want to pool.</typeparam>
public abstract class BaseObjectPoolingController<T> : MonoBehaviour where T : MonoBehaviour
{
    [Tooltip("Prefab of object we want to pool.")]
    [SerializeField]
    private T prefab;

    [Tooltip("Every object from pool will be instantiate under this object.")]
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

    /// <summary>
    /// Called when object is instantiate.
    /// Can be overriden.
    /// </summary>
    /// <returns>Instantiaded object.</returns>
    protected virtual T OnCreate()
    {
        var obj = Instantiate(prefab, parent);
        obj.name = obj.name.Replace("(Clone)", $"{obj.GetInstanceID()}");
        return obj;
    }

    /// <summary>
    /// Called when object is taken from pool.
    /// Can be overriden.
    /// </summary>
    /// <param name="obj">Object we get from pool.</param>
    protected virtual void OnGet(T obj)
    {
        obj.gameObject.SetActive(true);
        _outsidePool.Add(obj);
    }

    /// <summary>
    /// Called when object is returned from pool.
    /// Can be overriden.
    /// </summary>
    /// <param name="obj">Returned object.</param>
    protected virtual void OnObjectRelease(T obj)
    {
        obj.gameObject.SetActive(false);
        _outsidePool.Remove(obj);
    }

    /// <summary>
    /// Called when object is destroyed.
    /// Can be overriden.
    /// </summary>
    /// <param name="obj">Destroyed object.</param>
    protected virtual void OnObjectDestroy(T obj)
    {
        _outsidePool.Remove(obj);
        Destroy(obj);
    }

    /// <summary>
    /// Get first object from pooled objects stack.
    /// </summary>
    /// <returns>First object from pooled objects stack.</returns>
    public T Get() => _objectPool.Get();

    /// <summary>
    /// Returns given object to pool.
    /// </summary>
    /// <param name="obj">Object we want to return to pool.</param>
    public void Release(T obj) => _objectPool.Release(obj);

    /// <summary>
    /// Returns all objects to pool.
    /// </summary>
    public void ReleaseAll() => _outsidePool.ToList().ForEach(obj => Release(obj));
}
