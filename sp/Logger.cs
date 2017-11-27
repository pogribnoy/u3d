using UnityEngine;
using System.Collections;

public class Logger :  MonoBehaviour {
	public int Top = 10;
	public int Right = 10;
	public int Width = 100;
	public int Height = 200;
	public int Lines = 10;
	public GUIStyle skin;
	
	private ArrayList log = new ArrayList();
	
	void Start() {
		//log = new Array();
	}
	
	void Update() {
		
	}
	
	void AddLine(string str) {
		if(log.Count==10) {
			// удалить первый элемент
		}
		log.Add(str);
	}
	
	void OnGUI() {
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "This is a log", skin);
		for(int i=0; i< log.Count; i++) {
			GUI.Label(new Rect(Screen.width - Width - Right + 2, Screen.height - Height - Top + 2 + i*10, Width-4, 10), (string)log[i], skin);
		}
	}
} 