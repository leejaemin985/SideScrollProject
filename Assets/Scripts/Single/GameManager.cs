using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityObjectExtension
{
	public static GameManager GetGameManager(this Object unityObject) => GameManager.Get();
}

public class GameManager : MonoBehaviour
{
	/// <summary>
	/// 이 형태의 객체를 나타냅니다.
	/// </summary>
	private static GameManager _ThisInstance;

	/// <summary>
	/// GameManager 하위에 존재하는 관리 객체들을 나타냅니다.
	/// </summary>
	private List<IManagerClass> m_Managers = new();

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

	public static GameManager Get()
	{
		if (!_ThisInstance)
		{
			_ThisInstance = FindObjectOfType<GameManager>();
			_ThisInstance.OnGameManagerInitialized();
		}

		return _ThisInstance;
	}

	/// <summary>
	/// GameManager 객체 초기화
	/// </summary>
	private void OnGameManagerInitialized()
	{
		RegisterManager<SceneManager>();
	}

	/// <summary>
	/// 관리 객체를 찾습니다.
	/// </summary>
	/// <typeparam name="T">찾고자 하는 관리 객체 형식을 전달합니다.</typeparam>
	/// <returns>T 형식의 관리 객체를 반환합니다.</returns>
	public T GetManager<T>() where T : MonoBehaviour, IManagerClass 
		=> m_Managers.Find((IManagerClass type) => type.GetType() == typeof(T)) as T;

	private void RegisterManager<T>() where T : MonoBehaviour, IManagerClass
	{
		// GameManager 객체 하위에서 T 형식의 관리 객체 컴포넌트를 찾습니다.
		T manager = GetComponentInChildren<T>();

		// 찾지 못한 경우
		if (!manager)
		{
			// 빈 오브젝트를 생성하고, GameManager 하위 오브젝트로 추가합니다.
			GameObject newManagerObject = new GameObject(typeof(T).Name);
			newManagerObject.transform.SetParent(transform);

			// T 형식의 관리 객체 컴포넌트를 추가합니다.
			manager = newManagerObject.AddComponent<T>();
		}

		// 관리 객체를 리스트에 추가합니다.
		m_Managers.Add(manager);

		// 관리 객체 초기화
		manager.OnManagerInitialized();
	}
}
