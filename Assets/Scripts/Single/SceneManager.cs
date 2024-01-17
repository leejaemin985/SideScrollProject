using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : ManagerClassBase<SceneManager>
{
    
    private SceneInstance _SceneInstance;

    public override void OnManagerInitialized()
    {
        base.OnManagerInitialized();

        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(
        UnityEngine.SceneManagement.Scene prevScene,
        UnityEngine.SceneManagement.Scene nextScene)
    {
        if (nextScene.isLoaded)
        {
            _SceneInstance = GetSceneInstance<SceneInstance>();
        }

    }

    public T GetSceneInstance<T>() where T : SceneInstance
    {
        if (!_SceneInstance)
        {
            return (FindObjectOfType<SceneInstance>() ??
                new GameObject("SceneInstance").AddComponent<SceneInstance>()) as T;
            
        }

        return _SceneInstance as T;
    }

}
