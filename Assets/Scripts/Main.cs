using UnityEngine;

public class Main : MonoBehaviour
{
    private static Main instance;
    private static bool initialized;
    public static Main Instance
    {
        get
        {
            if (!initialized)
            {
                initialized = true;

                GameObject obj = GameObject.Find("Main");
                if (obj == null)
                {
                    obj = new() { name = "Main" };
                    obj.AddComponent<Main>();
                    DontDestroyOnLoad(obj);
                    instance = obj.GetComponent<Main>();
                }
            }
            return instance;
        }
    }

    private readonly PoolManager _pool = new();
    private readonly MinesweeperManager _mine = new();
    private readonly ResourceManager _resource = new();
    public static PoolManager Pool => Instance?._pool;
    public static MinesweeperManager Mine => Instance?._mine;
    public static ResourceManager Resource => Instance?._resource;


}