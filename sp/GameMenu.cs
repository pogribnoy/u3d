using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour {
	public int Status = 0;			// текущий статус работы меню
	public static int STATUS_HIDDEN = 0;
	public static int STATUS_MAIN = 1;
	public static int STATUS_SERVER_INFO = 2;
	public Resources Res;
	public int btnWidth = 700;
	public int btnHeight = 50;
	public int Padding = 10;
	private int width = 300;
	private int height = 400;
	
	void Start(){
		GameObject uc = GameObject.Find("UserController");
		if(uc!=null) Res = (Resources)uc.GetComponent<Resources>();
	}
	
	void OnGUI(){
		if(Status == GameMenu.STATUS_HIDDEN){
			// рисуем только кнопочку в уголке
			if (GUI.Button(new Rect(Padding,Padding,btnHeight,btnHeight), Res.Text("Menu"))){
				Status = GameMenu.STATUS_MAIN;
			}
		}
		else if(Status == GameMenu.STATUS_MAIN){
			width = btnWidth+Padding*2;
			height = (btnHeight+Padding)*5+Padding;
			int top = Padding;
			GUI.BeginGroup (new Rect ((Screen.width - width)/2, (Screen.height-height)/2, width, height));
			// рамка меню
			GUI.Box (new Rect (0,0,width,height), Res.Text("Menu"));
			
			// кнопка "Назад к игре"
			if (GUI.Button(new Rect(Padding,top,btnWidth,btnHeight), Res.Text("Back to game"))){
				// возврат в игру
				Status = GameMenu.STATUS_HIDDEN;
			}
			top+=btnHeight+Padding;
			
			// кнопка "Перезапуск"
			if (GUI.Button(new Rect(Padding,top,btnWidth,btnHeight), Res.Text("Restart"))){
				// возврат в игру
				Status = GameMenu.STATUS_HIDDEN;
			}
			top+=btnHeight+Padding;
			
			// кнопка "Закрыть программу"
			if (GUI.Button(new Rect(Padding,top,btnWidth,btnHeight), Res.Text("Close application"))){
				// выходим из приложения
				Application.Quit();				
			}
			top+=btnHeight+Padding;
			GUI.EndGroup ();
		}
		else if(Status == GameMenu.STATUS_SERVER_INFO){
			
		}
	}
	
	public void Init(Resources Resource){
		Res = Resource;
	}
} 