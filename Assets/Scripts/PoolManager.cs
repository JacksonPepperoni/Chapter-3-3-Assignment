using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public IObjectPool<Brick> _brickPool;
    public PoolManager()
    {
        Initialize();
    }

    public void Initialize()
    {
        _brickPool = new ObjectPool<Brick>(OnCreateBrick, OnGetBrick, OnReleaseBrick, OnDestroyBrick, maxSize: 1000);
    }

    public Brick OnCreateBrick()
    {
        return Main.Resource.Instantiate("Brick").GetComponent<Brick>();
    }

    public void OnGetBrick(Brick obj)
    {
       // Debug.Log("풀링겟");
        obj.gameObject.SetActive(true);
    }

    public void OnReleaseBrick(Brick obj)
    {
       // Debug.Log("풀링회수");
        obj.gameObject.SetActive(false);
    }

    public void OnDestroyBrick(Brick obj)
    {
        GameObject.Destroy(obj.gameObject);
    }
}
