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
	/// �� ������ ��ü�� ��Ÿ���ϴ�.
	/// </summary>
	private static GameManager _ThisInstance;

	/// <summary>
	/// GameManager ������ �����ϴ� ���� ��ü���� ��Ÿ���ϴ�.
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
	/// GameManager ��ü �ʱ�ȭ
	/// </summary>
	private void OnGameManagerInitialized()
	{
		RegisterManager<SceneManager>();
	}

	/// <summary>
	/// ���� ��ü�� ã���ϴ�.
	/// </summary>
	/// <typeparam name="T">ã���� �ϴ� ���� ��ü ������ �����մϴ�.</typeparam>
	/// <returns>T ������ ���� ��ü�� ��ȯ�մϴ�.</returns>
	public T GetManager<T>() where T : MonoBehaviour, IManagerClass 
		=> m_Managers.Find((IManagerClass type) => type.GetType() == typeof(T)) as T;

	private void RegisterManager<T>() where T : MonoBehaviour, IManagerClass
	{
		// GameManager ��ü �������� T ������ ���� ��ü ������Ʈ�� ã���ϴ�.
		T manager = GetComponentInChildren<T>();

		// ã�� ���� ���
		if (!manager)
		{
			// �� ������Ʈ�� �����ϰ�, GameManager ���� ������Ʈ�� �߰��մϴ�.
			GameObject newManagerObject = new GameObject(typeof(T).Name);
			newManagerObject.transform.SetParent(transform);

			// T ������ ���� ��ü ������Ʈ�� �߰��մϴ�.
			manager = newManagerObject.AddComponent<T>();
		}

		// ���� ��ü�� ����Ʈ�� �߰��մϴ�.
		m_Managers.Add(manager);

		// ���� ��ü �ʱ�ȭ
		manager.OnManagerInitialized();
	}
}
