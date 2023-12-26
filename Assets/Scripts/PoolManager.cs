using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool
{
    private GameObject prefab;
    private IObjectPool<GameObject> pool;
    private Transform root;
    private Transform Root
    {
        get
        {
            if (root == null)
            {
                GameObject obj = new() { name = $"[Pool_Root] {prefab.name}" };
                root = obj.transform;
            }
            return root;
        }
    }

    public Pool(GameObject prefab)
    {
        this.prefab = prefab;
        this.pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    /// <summary>
    /// 풀에서 오브젝트를 가져옴. 없으면 새 오브젝트 생성
    /// </summary>
    /// <returns></returns>
    public GameObject Pop()
    {
        return pool.Get();
    }

    /// <summary>
    /// 오브젝트를 풀에 반환 후 비활성화
    /// </summary>
    /// <param name="obj"></param>
    public void Push(GameObject obj)
    {
        pool.Release(obj);
    }
    #region Callbacks

    /// <summary>
    /// 새로운 오브젝트 인스턴스를 만듬
    /// </summary>
    /// <returns></returns>
    private GameObject OnCreate()
    {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.transform.SetParent(Root);
        obj.name = prefab.name;
        return obj;
    }
    /// <summary>
    /// 풀에서 오브젝트를 가져옴
    /// </summary>
    /// <param name="obj"></param>
    private void OnGet(GameObject obj)
    {
        obj.SetActive(true);
    }
    /// <summary>
    /// 오브젝트를 풀에 반환
    /// </summary>
    /// <param name="obj"></param>
    private void OnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }
    /// <summary>
    /// 오브젝트 삭제
    /// </summary>
    /// <param name="obj"></param>
    private void OnDestroy(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    #endregion
}

public class PoolManager
{

    private Dictionary<string, Pool> pools = new();

    public GameObject Pop(GameObject prefab)
    {
        // #1. 풀이 없으면 새로 만든다.
        if (pools.ContainsKey(prefab.name) == false) CreatePool(prefab);

        // #2. 해당 풀에서 하나를 가져온다.
        return pools[prefab.name].Pop();
    }

    public bool Push(GameObject obj)
    {
        // #1. 풀이 있는지 확인한다. (보통 있는 것이 정상)
        if (pools.ContainsKey(obj.name) == false) return false;

        // #2. 풀에 게임오브젝트를 돌려준다.
        pools[obj.name].Push(obj);

        return true;
    }
    /// <summary>
    /// 새로운 풀을 생성하고 Dictionary에 추가
    /// </summary>
    /// <param name="prefab"></param>
    private void CreatePool(GameObject prefab)
    {
        Pool pool = new(prefab);
        pools.Add(prefab.name, pool);
    }

    public void Clear()
    {
        pools.Clear();
    }

}