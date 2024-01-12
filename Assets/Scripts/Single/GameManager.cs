using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityObjectExtension
{
    public static GameManager GetGameManager(this Object unityObject) => GameManager.Get();
}

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    private List<IManagerClass> m_Managers = new();

    public static GameManager Get()
    {
        if (!Instance)
        {
            Instance = FindObjectOfType<GameManager>();
            Instance.OnGameManagerInitialized();

        }

        return Instance;
    }

    private void OnGameManagerInitialized()
    {
        RegisterManager<SceneManager>();
    }
    public T GetManager<T>() where T : MonoBehaviour, IManagerClass =>
        m_Managers.Find((IManagerClass type) => type.GetType() == typeof(T)) as T;

    private void RegisterManager<T>() where T : MonoBehaviour, IManagerClass
    {
        T manager = GetComponentInChildren<T>();

        if (!manager)
        {
            GameObject newManagerObject = new GameObject(typeof(T).Name);
            newManagerObject.transform.SetParent(transform);

            manager = newManagerObject.AddComponent<T>();

        }

        m_Managers.Add(manager);
        manager.OnManagerInitialized();


    }


    private void Awake()
    {
        GameManager gameManager = Get();
        if (gameManager != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }


}
