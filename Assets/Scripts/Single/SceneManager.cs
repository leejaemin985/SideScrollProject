using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : ManagerClassBase<SceneManager>
{
	/// <summary>
	/// ���� ������ ������� �� ��ü�� ��Ÿ���ϴ�.
	/// ���� ����� ������ ������� ���� �� ��ü�� �����մϴ�.
	/// </summary>
	private SceneInstance _SceneInstance;

	public override void OnManagerInitialized()
	{
		base.OnManagerInitialized();

		// ���� ��ü�� ������ �߻��ϴ� �̺�Ʈ�� �����մϴ�.
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

	/// <summary>
	/// T ������ �� ��ü�� ����ϴ�.
	/// </summary>
	/// <typeparam name="T">���� �� ��ü ������ �����մϴ�.</typeparam>
	/// <returns>T ������ �� ��ü�� ��ȯ�մϴ�.</returns>
	public T GetSceneInstance<T>() where T : SceneInstance
	{
		return (FindObjectOfType<SceneInstance>() ??
				new GameObject("SceneInstance").AddComponent<SceneInstance>()) as T;
	}






}
