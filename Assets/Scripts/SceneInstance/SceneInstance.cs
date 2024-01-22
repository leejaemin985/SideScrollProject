using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInstance : MonoBehaviour
{
	[Header("플레이어 컨트롤러 프리팹")]
	public PlayerController m_PlayerControllerPrefab;

	public PlayerController playerController { get; private set; }
	private PlayerCharacter _PlayerCharacter;

	private void Awake()
	{
		// 플레이어 컨트롤러 생성
		playerController = Instantiate(m_PlayerControllerPrefab);

		// 플레이어 캐릭터를 찾습니다.
		_PlayerCharacter = FindObjectOfType<PlayerCharacter>();

		// 플레이어 컨트롤러가 조종하는 캐릭터를 설정합니다.
		playerController.StartControlCharacter(_PlayerCharacter);
	}
}
