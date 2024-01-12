using UnityEngine;


public interface IManagerClass
{
    void OnManagerInitialized();
}


public class ManagerClassBase<T> : 
    MonoBehaviour, IManagerClass
    where T : MonoBehaviour, IManagerClass
{
    private static T Instance;

    public static T instance => Instance ??
        (Instance = GameManager.Get().GetManager<T>());

    public virtual void OnManagerInitialized()
    {

    }
}


