using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

class Pool
{
    public GameObject _prefab;
    private IObjectPool<GameObject> _pool;

    private Transform _root;
    public Transform Root
    {
        get
        {
            if (_root == null)
            {
                GameObject obj = new() { name = $"[Pool_Root] {_prefab.name}" };
                _root = obj.transform;
            }

            return _root;
        }
    }

    public Pool(GameObject prefab)
    {
        _prefab = prefab;
        _pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy, maxSize: 1000);
    }

    public void Push(GameObject obj)
    {
        _pool.Release(obj);
    }

    public GameObject Pop()
    {
        return _pool.Get();
    }

    #region Funcs

    private GameObject OnCreate()
    {
        GameObject obj = GameObject.Instantiate(_prefab);
        obj.transform.SetParent(Root);
        obj.name = _prefab.name;
        return obj;
    }

    private void OnGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroy(GameObject obj)
    {
        GameObject.Destroy(obj);
    }
    #endregion
}

public class PoolManager
{
    Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    public GameObject Pop(GameObject prefab)
    {
        if (!_pools.ContainsKey(prefab.name))
            CreatePool(prefab);

        return _pools[prefab.name].Pop();
    }

    public bool Push(GameObject obj)
    {
        if (!_pools.ContainsKey(obj.name))
            return false;

        _pools[obj.name].Push(obj);
        return true;
    }

    void CreatePool(GameObject prefab)
    {
        Pool pool = new Pool(prefab);
        _pools.Add(prefab.name, pool);
    }
    public GameObject GetOriginal(string name)
    {
        if (!_pools.ContainsKey(name))
            return null;

        return _pools[name]._prefab;
    }

}
