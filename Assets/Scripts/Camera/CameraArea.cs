using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public sealed class CameraArea : MonoBehaviour
{
	private BoxCollider _Area;
	public BoxCollider area => _Area ?? (_Area = GetComponent<BoxCollider>());

	public Vector3 cameraAreaMin => transform.position + area.center - (area.size * 0.5f);
	public Vector3 cameraAreaMax => transform.position + area.center + (area.size * 0.5f);

	private void Start()
	{
		// �� ī�޶� ������ ����մϴ�.
		SceneManager.instance.GetSceneInstance<GameSceneInstance>().RegisterCameraArea(this);
	}


}
