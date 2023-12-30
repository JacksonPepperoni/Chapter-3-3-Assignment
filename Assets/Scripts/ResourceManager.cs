using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string key, Transform parent = null)
    {
            GameObject prefab = Load<GameObject>($"Prefabs/{key}");
            return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject obj)
    {
        if (obj != null)
            Object.Destroy(obj);
    }
}
