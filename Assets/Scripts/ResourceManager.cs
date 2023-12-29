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

            GameObject obj = Main.Pool.GetOriginal(name);
            if (obj != null)
                return obj as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{key}");
        if (prefab == null)
        {
            Debug.Log($"불러오기실패 : {key}");
            return null;
        }

        if (pooling)
            return Main.Pool.Pop(prefab);

        GameObject obj = Object.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null)
            return;

        if (Main.Pool.Push(obj))
            return;

        Object.Destroy(obj);

        Debug.Log("파파괴");
    }
}
