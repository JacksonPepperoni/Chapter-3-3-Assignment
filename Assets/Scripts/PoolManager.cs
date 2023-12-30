using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public IObjectPool<Brick> brickPool;
    
    public PoolManager()
    {
        Initialize();
    }

    private void Initialize()
    {
        brickPool = new ObjectPool<Brick>(OnCreateBrick, OnGetBrick, OnReleaseBrick, OnDestroyBrick, maxSize: 1000);
    }

    private Brick OnCreateBrick()
    {
        return Main.Resource.Instantiate("Brick").GetComponent<Brick>();
    }

    private void OnGetBrick(Brick obj)
    {
       // Debug.Log("풀링겟");
        obj.gameObject.SetActive(true);
    }

    private void OnReleaseBrick(Brick obj)
    {
       // Debug.Log("풀링회수");
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyBrick(Brick obj)
    {
        GameObject.Destroy(obj.gameObject);
    }
}
