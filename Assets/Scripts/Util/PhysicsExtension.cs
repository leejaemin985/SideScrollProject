using UnityEngine;


public class DrawGizmoLineInfo
{
	public Color defaultColor { get; set; } = new Color(0.0f, 1.0f, 0.0f, 1.0f);

	public Color detectedColor { get; set; } = new Color(1.0f, 0.0f, 0.0f, 1.0f);

	public bool isHit { get; set; }
	public Vector3 start { get; set; }
	public Vector3 end { get; set; }
}

public class DrawGizmoCubeInfo : DrawGizmoLineInfo
{ 
	public Vector3 halfSize { get; set; }
}




public static class PhysicsExt
{
	public static bool Raycast(
		out DrawGizmoLineInfo info,
		Ray ray,
		out RaycastHit hitInfo,
		float maxDistance,
		int layerMask,
		QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
	{
		bool result = Physics.Raycast(
			ray,
			out hitInfo,
			maxDistance,
			layerMask,
			queryTriggerInteraction);

		info = new DrawGizmoLineInfo()
		{
			start = ray.origin,
			end = (result ? hitInfo.point : ray.origin + ray.direction * maxDistance),
			isHit = result
		};

		return result;
	}

	public static bool BoxCast(
		out DrawGizmoCubeInfo info,
		Vector3 center, 
		Vector3 halfExtents, 
		Vector3 direction, 
		out RaycastHit hitInfo, 
		Quaternion orientation, 
		float maxDistance, 
		int layerMask, 
		QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
	{
		bool result = Physics.BoxCast(
			center,
			halfExtents,
			direction,
			out hitInfo,
			orientation,
			maxDistance,
			layerMask,
			queryTriggerInteraction);

		info = new DrawGizmoCubeInfo()
		{
			isHit = result,
			start = center,
			end = (result ?
				center + direction * hitInfo.distance :
				center + direction * maxDistance),
			halfSize = halfExtents
		};

		return result;
	}



	public static Collider[] OverlapBox(
		out DrawGizmoCubeInfo info,
		Vector3 center,
		Vector3 halfExtens,
		Quaternion orientation,
		int layerMask,
		QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
	{

		Collider[] detectColliders =
		Physics.OverlapBox(center, halfExtens, orientation, layerMask, queryTriggerInteraction);
		
		info = new DrawGizmoCubeInfo()
		{
			isHit = detectColliders.Length > 0,
			start = center,
			halfSize = halfExtens
		};

		return detectColliders;
	}











	public static void DrawGizmoLine(in DrawGizmoLineInfo info)
	{
		if (info == null) return;

		Gizmos.color = info.isHit ? info.detectedColor : info.defaultColor;
		Gizmos.DrawLine(info.start, info.end);
	}

	public static void DrawGizmoBox(in DrawGizmoCubeInfo info)
	{
		if (info == null) return;

		Gizmos.color = info.defaultColor;
		Gizmos.DrawWireCube(info.start, info.halfSize * 2.0f);

		Gizmos.color = info.isHit ? info.detectedColor : info.defaultColor;
		Gizmos.DrawLine(info.start, info.end);
		Gizmos.DrawWireCube(info.end, info.halfSize * 2.0f);
	}


	public static void DrawGizmoOverlapBox(in DrawGizmoCubeInfo info)
	{
		if (info == null) return;

		Gizmos.color = info.isHit ? info.detectedColor : info.defaultColor;

		Gizmos.DrawWireCube(info.start, info.halfSize * 2.0f);

	}


}
