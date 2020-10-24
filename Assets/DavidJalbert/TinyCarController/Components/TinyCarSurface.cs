using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyCarSurface : MonoBehaviour
{
	public static TinyCarSurfaceParameters DEFAULT = new TinyCarSurfaceParameters();

	public TinyCarSurfaceParameters parameters;
}

[System.Serializable]
public class TinyCarSurfaceParameters
{
	public string name = "";
	public float steeringMultiplier = 1f;
	public float accelerationMultiplier = 1f;
	public float speedMultiplier = 1f;
	public float forwardFrictionMultiplier = 1f;
	public float lateralFrictionMultiplier = 1f;

	public string getName()
	{
		if (name.Length > 0) return name;
		return "(default)";
	}
}