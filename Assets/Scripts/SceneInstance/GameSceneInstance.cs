using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameSceneInstance : SceneInstance
{
	[Header("������ ���Ǵ� ī�޶�")]
	public FollowCamera m_UseFollowCamera;

	/// <summary>
	/// ���� ���Ǵ� ī�޶� �������� ��Ÿ���ϴ�.
	/// </summary>
	private Dictionary<BoxCollider, CameraArea> _CameraAreas = new();


	private Dictionary<Collider, EnemyCharacter> _EnemyCharacters = new();

	public void RegisterEnemyCharacter(EnemyCharacter newEnemyCharacter)
	{
		_EnemyCharacters.Add(newEnemyCharacter.enemyCollider, newEnemyCharacter);
	}


	/// <summary>
	/// BoxCollider �� ��ġ�ϴ� ī�޶� ������ ����ϴ�.
	/// </summary>
	/// <param name="key">CameraArea ������Ʈ�� �Բ� ���Ǵ� BoxCollider �� �����մϴ�.</param>
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
	/// ī�޶� ������ ����մϴ�.
	/// </summary>
	/// <param name="cameraArea">��Ͻ�ų ī�޶� ������ �����մϴ�.</param>
	public void RegisterCameraArea(CameraArea cameraArea)
	{
		_CameraAreas.Add(cameraArea.area, cameraArea);
	}

}

