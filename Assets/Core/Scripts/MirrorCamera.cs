using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
	public bool mirrorX = true;
	public bool mirrorY = false;
	Camera cam;

	private void Awake()
	{
		cam = GetComponent<Camera>();
	}

	private void OnPreCull()
	{
		cam.ResetWorldToCameraMatrix();
		cam.ResetProjectionMatrix();
		cam.projectionMatrix = cam.projectionMatrix * Matrix4x4.Scale(new Vector3(mirrorX ? -1 : 1, mirrorY ? -1 : 1, 1));
	}

	private void OnPreRender()
	{
		GL.invertCulling = mirrorX != mirrorY;
	}

	private void OnPostRender()
	{
		GL.invertCulling = false;
	}
}
