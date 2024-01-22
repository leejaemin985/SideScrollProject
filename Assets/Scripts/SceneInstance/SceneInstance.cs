using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInstance : MonoBehaviour
{
	[Header("�÷��̾� ��Ʈ�ѷ� ������")]
	public PlayerController m_PlayerControllerPrefab;

	public PlayerController playerController { get; private set; }
	private PlayerCharacter _PlayerCharacter;

	private void Awake()
	{
		// �÷��̾� ��Ʈ�ѷ� ����
		playerController = Instantiate(m_PlayerControllerPrefab);

		// �÷��̾� ĳ���͸� ã���ϴ�.
		_PlayerCharacter = FindObjectOfType<PlayerCharacter>();

		// �÷��̾� ��Ʈ�ѷ��� �����ϴ� ĳ���͸� �����մϴ�.
		playerController.StartControlCharacter(_PlayerCharacter);
	}
}
