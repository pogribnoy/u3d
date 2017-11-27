using UnityEngine;
using System.Collections;

public class Game :  MonoBehaviour {
	public ArrayList Units = new ArrayList(); 			// все созданные юниты, которыми можно управлять: корабли, станции, базы, но не снаряды, ракеты и т.д.
	//public Resources Res;
	//public Settings Set;
	
	void Start(){
		// надо загрузить настройки
		//Res = (Resources)gameObject.GetComponent<Resources>();
		//Set = (Settings)gameObject.GetComponent<Settings>();
		
	}
	
	void Update(){
		// просчитываем модель игры
		
	}

	void OnGUI(){
		
	}
	
	public void addUnit(GameObject u){		
		if(!Units.Contains(u)){
			Units.Add(u);
			Debug.Log("Unit created. Total units.Count="+Units.Count.ToString());
		}
		else{
			Debug.Log("You're trying to create same unit twice!. Total units.Count="+Units.Count.ToString());
		}
	}
		
	public void removeUnit(GameObject u){
		if(Units.Count>0){
			// есть юниты
			if(Units.Contains(u)){
				// такой юнит есть среди используемых юнитов
				
				// удаляем из массива используемых юнитов
				Units.Remove(u);
			}
		}
		else{
			Debug.Log("You're trying to delete unit, There are no units in game!");
		}
	}
	
	public void removeAllUnits(){
		for(int i=0; i<Units.Count; i++){
			GameObject u = Units[i] as GameObject;
			// надо возвращать объект в пул
			u.SetActive(false);
		}
	}
	
} 