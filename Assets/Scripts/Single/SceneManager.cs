using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : ManagerClassBase<SceneManager>
{
	/// <summary>
	/// 현재 씬에서 사용중인 씬 객체를 나타냅니다.
	/// 씬이 변경될 때마다 사용중인 씬의 씬 객체를 참조합니다.
	/// </summary>
	private SceneInstance _SceneInstance;

	public override void OnManagerInitialized()
	{
		base.OnManagerInitialized();

		// 씬이 교체될 때마다 발생하는 이벤트를 설정합니다.
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
	/// T 형식의 씬 객체를 얻습니다.
	/// </summary>
	/// <typeparam name="T">얻을 씬 객체 형식을 전달합니다.</typeparam>
	/// <returns>T 형식의 씬 객체를 반환합니다.</returns>
	public T GetSceneInstance<T>() where T : SceneInstance
	{
		return (FindObjectOfType<SceneInstance>() ??
				new GameObject("SceneInstance").AddComponent<SceneInstance>()) as T;
	}






}
