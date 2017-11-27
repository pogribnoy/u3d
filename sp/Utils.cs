using UnityEngine; 
using System; 
using System.Collections;

public static class Utils {  
	
	public static Rect obj3dProjection(GameObject obj, Camera camera) {  
		Vector2[] vecPts = new Vector2[8];
		Transform tr = obj.transform;
		Vector3 min = obj.collider.bounds.min;
		Vector3 max = obj.collider.bounds.max;
		int i=0;
					
		Vector3 vec = new Vector3();
		vec = camera.WorldToScreenPoint(max);
		vecPts[i] = new Vector2(vec.x, vec.y);
		vec = camera.WorldToScreenPoint(new Vector3(min.x, max.y, max.z));
		vecPts[i] = new Vector2(vec.x, vec.y);
		vec = camera.WorldToScreenPoint(new Vector3(max.x, min.y, max.z));
		vecPts[i] = new Vector2(vec.x, vec.y);
		vec = camera.WorldToScreenPoint(new Vector3(max.x, max.y, min.z));
		vecPts[i] = new Vector2(vec.x, vec.y);
		vec = camera.WorldToScreenPoint(new Vector3(min.x, min.y, max.z));
		vecPts[i] = new Vector2(vec.x, vec.y);
		vec = camera.WorldToScreenPoint(new Vector3(min.x, max.y, min.z));
		vecPts[i] = new Vector2(vec.x, vec.y);
		vec = camera.WorldToScreenPoint(new Vector3(max.x, min.y, min.z));
		vecPts[i] = new Vector2(vec.x, vec.y);
		vec = camera.WorldToScreenPoint(min);
		vecPts[i] = new Vector2(vec.x, vec.y);
		
		for(i=0;i<8; i++) vecPts[i].y = Screen.height-vecPts[i].y;
		
		float minX = float.MaxValue;  
		float minY = float.MaxValue;  
		float maxX = float.MinValue;  
		float maxY = float.MinValue; 
		
		foreach(Vector2 pt in vecPts) {  
			if (pt.x < minX) minX = pt.x;  
			if (pt.x > maxX) maxX = pt.x;  
			if (pt.y < minY) minY = pt.y;  
			if (pt.y > maxY) maxY = pt.y; 
		}  
		return new Rect(minX, minY, maxX - minX, maxY-minY);  
	}
}