using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FollowCamera : MonoBehaviour
{
    [Header("ī�޶� �Ÿ�")]
    public float m_CameraDistance = 10.0f;

    [Header("ī�޶� �̵�")]
    public float m_CameraFollowSpeed = .6f;


    /// <summary>
    /// ī�޶��� ������ ��Ÿ���ϴ�.
    /// </summary>
    private CameraArea _CameraArea;

    /// <summary>
    /// ī�޶��� �� ũ�⸦ ��Ÿ���ϴ�.
    /// </summary>
    public Vector3 _CameraViewSize;

    /// <summary>
    /// ī�޶� �̵��� ��ǥ ��ġ�Դϴ�.
    /// </summary>
    private Vector3 _TargetPosition;





    private void Update()
    {
        UpdateCameraPosition();
    }

    /// <summary>
    /// ī�޶��� ��ġ�� �����մϴ�.
    /// </summary>
    private void UpdateCameraPosition()
    {
        // ī�޶� ������ �������� ���� ��� �������� �ʽ��ϴ�.
        if (!_CameraArea) return;

        // ī�޶� ���� ũ�⸦ ����ϴ�.
        Vector3 halfSize = _CameraViewSize * 0.5f;

        // ī�޶� ��ġ�� �̵��� �� �ִ� �ּ� ��ġ, �ִ� ��ġ�� ����մϴ�.
        Vector3 moveMinPosition = _CameraArea.cameraAreaMin + halfSize;
        Vector3 moveMaxPosition = _CameraArea.cameraAreaMax - halfSize;

        // ī�޶� ��ġ�� �����մϴ�.
        Vector3 newPosition = Vector3.MoveTowards(transform.position, _TargetPosition, m_CameraFollowSpeed * Time.deltaTime);

        Debug.Log("_TargetPosition = " + _TargetPosition);

        // moveMinPosition ~ moveMaxPosition ���̿� ī�޶� ��ġ�� �� �ֵ��� �մϴ�.
        newPosition.x = Mathf.Clamp(newPosition.x, moveMinPosition.x, moveMaxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, moveMinPosition.y, moveMaxPosition.y);
        Debug.Log("newPosition = " + newPosition);

        // ����� ��ġ�� �����մϴ�.
        transform.position = newPosition;
    }



    /// <summary>
    /// ī�޶� ������ �����մϴ�.
    /// </summary>
    /// <param name="newCameraArea">������ų ī�޶� ������ �����մϴ�.</param>
    public void SetCameraArea(CameraArea newCameraArea)
    {
        if (_CameraArea == newCameraArea) return;
        _CameraArea = newCameraArea;
    }



    /// <summary>
    /// ī�޶� ��ǥ ��ġ�� �����մϴ�.
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
