using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInstance : MonoBehaviour
{
    [Header("플레이어 컨트롤러 프리펩")]
    public PlayerController m_PlayerControllerPrefab;

    public PlayerController playerController { get; private set; }
    private PlayerCharacter _PlayerCharacter;

    private void Awake()
    {

        playerController = Instantiate(m_PlayerControllerPrefab);

        _PlayerCharacter = FindObjectOfType<PlayerCharacter>();


        playerController.StartControlCharacter(_PlayerCharacter);
    }
}
