using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("카메라 영역 레이어")]
	public LayerMask m_CameraAreaLayer;

	/// <summary>
	/// 카메라 객체를 나타냅니다.
	/// </summary>
	private FollowCamera _FollowCamera;

	#region DEBUG
	private DrawGizmoLineInfo _DrawGizmoCameraAreaInfo;
	#endregion

	private void Start()
	{
		GameSceneInstance sceneInstance = SceneManager.instance.GetSceneInstance<GameSceneInstance>();
		_FollowCamera = sceneInstance.m_UseFollowCamera;
	}

	private void Update()
	{
		DoCheckCameraArea();

		// 카메라 목표 위치를 설정합니다.
		_FollowCamera.SetCameraTargetPosition(transform.position);
	}

	private void DoCheckCameraArea()
	{
		Vector3 start = transform.position + Vector3.back * 10.0f;
		Ray ray = new Ray(start, Vector3.forward);

		bool isHit = PhysicsExt.Raycast(out _DrawGizmoCameraAreaInfo,
			ray, out RaycastHit hitResult, Mathf.Infinity, m_CameraAreaLayer);
		
		if (isHit)
		{
			// 씬 객체를 얻습니다.
			GameSceneInstance sceneInstance = SceneManager.instance.GetSceneInstance<GameSceneInstance>();

			// 감지된 Collider 컴포넌트를 얻습니다.
			BoxCollider detectArea = hitResult.collider as BoxCollider;

			// Collider 를 이용하여 카메라 영역을 얻습니다.
			CameraArea cameraArea = sceneInstance.GetCameraArea(detectArea);

			// 카메라 영역 설정
			_FollowCamera.SetCameraArea(cameraArea);
		}
	}

	private void OnDrawGizmos()
	{
		PhysicsExt.DrawGizmoLine(_DrawGizmoCameraAreaInfo);
	}




}