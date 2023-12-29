using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string key, Transform parent = null)
    {
        return Object.Instantiate(Resources.Load<GameObject>($"Prefabs/{key}"), parent);
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null)
            return;

        Object.Destroy(obj);

    }
}
