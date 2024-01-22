using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameSceneInstance : SceneInstance
{
	[Header("씬에서 사용되는 카메라")]
	public FollowCamera m_UseFollowCamera;

	/// <summary>
	/// 씬에 사용되는 카메라 영역들을 나타냅니다.
	/// </summary>
	private Dictionary<BoxCollider, CameraArea> _CameraAreas = new();


	private Dictionary<Collider, EnemyCharacter> _EnemyCharacters = new();

	public void RegisterEnemyCharacter(EnemyCharacter newEnemyCharacter)
	{
		_EnemyCharacters.Add(newEnemyCharacter.enemyCollider, newEnemyCharacter);
	}


	/// <summary>
	/// BoxCollider 와 일치하는 카메라 영역을 얻습니다.
	/// </summary>
	/// <param name="key">CameraArea 컴포넌트와 함께 사용되는 BoxCollider 를 전달합니다.</param>
	/// <returns></returns>
	public CameraArea GetCameraArea(BoxCollider key)
	{
		if (_CameraAreas.TryGetValue(key, out CameraArea area))
		{
			return area;
		}

		return null;
	}

	/// <summary>
	/// 카메라 영역을 등록합니다.
	/// </summary>
	/// <param name="cameraArea">등록시킬 카메라 영역을 전달합니다.</param>
	public void RegisterCameraArea(CameraArea cameraArea)
	{
		_CameraAreas.Add(cameraArea.area, cameraArea);
	}

}

