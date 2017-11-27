using System.Reflection;
using UnityEngine;

/*
Носитель: камера
Действие: создает набор вершин для рисования линии через mesh
*/

public static class Drawing3D{
	public static Camera cam;
 
	public static Vector3[] MakeQuad(Vector3 s, Vector3 e, float w) {
		w = w / 2;
		Vector3[] q = new Vector3[4];
	 
		//standard normal based on points
		//Vector3 n = Vector3.Cross(s, e);
		//Vector3 l = Vector3.Cross(n, e-s);
	 
		//camera forward based normal
		Vector3 l = Vector3.Cross(cam.transform.forward, e-s);
	 
		l.Normalize();
		q[0] = cam.transform.InverseTransformPoint(s + l * w );
		q[1] = cam.transform.InverseTransformPoint(s + l * -w);
		q[2] = cam.transform.InverseTransformPoint(e + l * w);
		q[3] = cam.transform.InverseTransformPoint(e + l * -w);
 
		return q;
	}
  
  // This static initializer works for runtime, but apparently isn't called when
  // Editor play mode stops, so DrawLine will re-initialize if needed.
	static Drawing3D() {
		Initialize();
	}
 
	private static void Initialize() {
			
	}
}