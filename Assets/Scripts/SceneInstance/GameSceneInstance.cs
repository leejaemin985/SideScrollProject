using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameSceneInstance : SceneInstance
{
    public FollowCamera m_UseFollowCamera;


    private Dictionary<BoxCollider, CameraArea> _CameraAreas => new();
    public List<CameraArea> m_Test = new();

    public CameraArea GetCameraArea(BoxCollider key)
    {
        if (_CameraAreas.TryGetValue(key, out CameraArea area))
        {
            return area;
        }
        else
        {
            return null;
        }
    }


    public void RegisterCameraArea(CameraArea cameraArea)
    {
        m_Test.Add(cameraArea);
        _CameraAreas.Add(cameraArea.area, cameraArea);
    }

}
