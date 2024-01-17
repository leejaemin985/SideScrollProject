using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FollowCamera : MonoBehaviour
{
    [Header("카메라 거리")]
    public float m_CameraDistance = 10.0f;

    [Header("카메라 이동")]
    public float m_CameraFollowSpeed = .6f;


    /// <summary>
    /// 카메라의 영역을 나타냅니다.
    /// </summary>
    private CameraArea _CameraArea;

    /// <summary>
    /// 카메라의 뷰 크기를 나타냅니다.
    /// </summary>
    public Vector3 _CameraViewSize;

    /// <summary>
    /// 카메라가 이동될 목표 위치입니다.
    /// </summary>
    private Vector3 _TargetPosition;





    private void Update()
    {
        UpdateCameraPosition();
    }

    /// <summary>
    /// 카메라의 위치를 갱신합니다.
    /// </summary>
    private void UpdateCameraPosition()
    {
        // 카메라 영역을 감지하지 못한 경우 실행하지 않습니다.
        if (!_CameraArea) return;

        // 카메라 절반 크기를 얻습니다.
        Vector3 halfSize = _CameraViewSize * 0.5f;

        // 카메라 위치가 이동될 수 있는 최소 위치, 최대 위치를 계산합니다.
        Vector3 moveMinPosition = _CameraArea.cameraAreaMin + halfSize;
        Vector3 moveMaxPosition = _CameraArea.cameraAreaMax - halfSize;

        // 카메라 위치를 연산합니다.
        Vector3 newPosition = Vector3.MoveTowards(transform.position, _TargetPosition, m_CameraFollowSpeed * Time.deltaTime);

        Debug.Log("_TargetPosition = " + _TargetPosition);

        // moveMinPosition ~ moveMaxPosition 사이에 카메라가 배치될 수 있도록 합니다.
        newPosition.x = Mathf.Clamp(newPosition.x, moveMinPosition.x, moveMaxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, moveMinPosition.y, moveMaxPosition.y);
        Debug.Log("newPosition = " + newPosition);

        // 연산된 위치를 적용합니다.
        transform.position = newPosition;
    }



    /// <summary>
    /// 카메라 영역을 설정합니다.
    /// </summary>
    /// <param name="newCameraArea">설정시킬 카메라 영역을 전달합니다.</param>
    public void SetCameraArea(CameraArea newCameraArea)
    {
        if (_CameraArea == newCameraArea) return;
        _CameraArea = newCameraArea;
    }



    /// <summary>
    /// 카메라 목표 위치를 설정합니다.
    /// </summary>
    /// <param name="targetPosition"></param>
    public void SetCameraTargetPosition(Vector3 targetPosition)
    {
        _TargetPosition = targetPosition + (Vector3.back * m_CameraDistance);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            transform.position + (Vector3.forward * m_CameraDistance),
            _CameraViewSize);
    }



}
