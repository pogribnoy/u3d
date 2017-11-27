using UnityEngine;
using System.Collections;

public class SelectedUnits : MonoBehaviour {
	//public GameObject UserController;	// компонента скрипта управления для пользователя, чтобы не извлекать на лету
	
	public ArrayList Units = new ArrayList();	// выделенные юниты
	public ArrayList UnitsScripts = new ArrayList();	// скрипты выделенных юнитов
	public bool isHereUnits = false;
	public GameObject mainUnit;
	public Vector3 geomCenter;
	public int _oldStatus = 0;
	
	// маленький кружок возле курсора
	public GameObject trackerSelector;
	// большой круг при прокладывании маршрута
	public GameObject trackerPlane;
	// линии ориентации при прокладывании маршрута
	private GameObject trackerLines;

	private UserController UC;
	
	public Transform Target; 		// точка, к которой будет двигаться наше выделение 
	public GameObject objTarget; 	// объект, с которым будет взаимодействовать наше выделение (атаковать, охранять, следовать и т.д.), объектом м.б. объединение

	void Start() {
		UC = GetComponent<UserController>();
		
	}

	void Update(){
		if(Units.Count>0){
			recalcGeomCenter();
			
			if(UC.Status == 4){
				//trackerSelector.position = trackerLines.updateTrackerLineH(geomCenter,trackerSelector.position);
				if(Input.GetKeyDown(KeyCode.LeftShift)){
					UC.Status = 5;
				}
				if(Input.GetKeyDown(KeyCode.M)){
					UC.Status = _oldStatus;
					//trackerSelector.active = false;
				}
			}
			else if(UC.Status == 5) {
				//trackerSelector.transform.position = trackerLines.updateTrackerLineV(geomCenter,trackerSelector.transform.position);
			}
			else {
				if(Input.GetKeyDown(KeyCode.M)){
					_oldStatus = UC.Status;
					UC.Status = 4;
					//trackerSelector.active = true;
				}
				if(Input.GetKeyUp(KeyCode.LeftShift)){
					UC.Status = 4;
				}
			}
		}
	}

	void OnGUI(){

	}
	
	public void addUnit(GameObject u){
		if(Units.Count==0){
			// еще не выделено ни одного элемента
			// выделяем
			Units.Add(u);
			ArrayList scripts = new ArrayList();
			scripts.Add((UserUnitAI)(u.GetComponent<UserUnitAI>()));
			UnitsScripts.Add(scripts);
			geomCenter = u.transform.position;
			//((UnitSelect)u).isSelected = true;
			Debug.Log("Unit added. Units.Count="+Units.Count.ToString());
		}
		else{
			// уже есть выделенные юниты
			if(!Units.Contains(u)){
				// такой юнит еще не был выделен
				// выделяем
				Units.Add(u);
				// пересчитываем геометриеский центр только для добавления этого юнита (поэтому не вызываем функцию recalcGeomCenter - чтобы не перебирать все юниты)
				Vector3 pos = u.transform.position;
				geomCenter.x += (pos.x-geomCenter.x)/2;
				geomCenter.y += (pos.y-geomCenter.y)/2;
				geomCenter.z += (pos.z-geomCenter.z)/2;
			}
		}
		// надо бы тутже получать указатель на компонент управления юнитом, чтобы каждый раз его не извлекать при обновлении
		//u.isSelected = true;
	}
	
	public void removeUnit(GameObject u){
		if(Units.Count>0){
			// есть выделенные юниты
			if(Units.Contains(u)){
				// такой юнит есть среди выделенных
				// снимаем выделение
				Units.Remove(u);
				// пересчитываем геометриеский центр только для удаления этого юнита (поэтому не вызываем функцию recalcGeomCenter - чтобы не перебирать все юниты)
				Vector3 pos = u.transform.position;
				geomCenter.x -= (pos.x-geomCenter.x)/2;
				geomCenter.y -= (pos.y-geomCenter.y)/2;
				geomCenter.z -= (pos.z-geomCenter.z)/2;
			}
		}
	}
	
	private void recalcGeomCenter(){
		// есть выделенные юниты
		// пересчитываем геометриеский центр
		Vector3 pos;
		// берем позицию первого юнита
		//geomCenter = Units[0].transform.position;
		foreach (GameObject u in Units){
			// получаем позицию очередного юнита
			pos = u.transform.position;
			// пересчитываем геометрический центр
			geomCenter.x += (pos.x-geomCenter.x)/2;
			geomCenter.y += (pos.y-geomCenter.y)/2;
			geomCenter.z += (pos.z-geomCenter.z)/2;
		}
	}
	
	public void removeAllUnits(){
		if(Units.Count>0){
			//GameObject u;
			// есть выделенные юниты
			for(int i=0; i<Units.Count; i++){
				((UserUnitAI)(UnitsScripts[i])).DeSelect();
			}
			
			Units = new ArrayList();
		}
		/*if(trackerLines) trackerLines.SetActive(false);
		else Debug.LogError("SelectedUnits. Not initialised 'trackerLines'");
		if(trackerSelector) trackerSelector.SetActive(false);
		else Debug.LogError("SelectedUnits. Not initialised 'trackerSelector'");
		if(trackerPlane) trackerPlane.SetActive(false);
		ele Debug.LogError("SelectedUnits. Not initialised 'trackerPlane'");*/
		// надо у остальных юнитов убрать маркеры расстояния до плоскости 
	}
	
	/**
	 *Задает юнитам матрицу конечных расположений в зависимости от их массы и выбранного построения (не реализованно)
	 */
	private void buildTracks(){
		Debug.Log("-------------------");
		Debug.Log("Target locked");
		
		// теперь надо остальным юнитам построить сетку местоназначений
		for(int i=0; i<Units.Count; i++){
			GameObject u = (GameObject)Units[i];
			UserUnitAI ai = GetComponent<UserUnitAI>();
			ai.goTo(Target);
			/*if(UnitsScripts[i]!=null){
				for(int j=0; j<UnitsScripts[i].Count; j++){
					if(UnitsScripts[i][j] is UserUnitAI){
						UnitsScripts[i][j].goTo(Target);
					}
				}
			}*/
			//u.goToXYZ(trackerSelector.transform.position.x, trackerSelector.transform.position.y, trackerSelector.transform.position.z);
		}
	}
	
	/**
	 *Возвращает радиус тракера для прокладки маршрута 
	 */
	public float trackerR() {
		Vector3 v = trackerSelector.transform.position;
		return Mathf.Sqrt((v.x-geomCenter.x)*(v.x-geomCenter.x) + (v.y-geomCenter.y)*(v.y-geomCenter.y) + (v.z-geomCenter.z)*(v.z-geomCenter.z));
	}
	
	
	/**
	 *Направляет выделенную группу в указанную точку  
	 */
	public void MoveToTarget() {
		// указываем всем юнитам, куда им двигаться
		buildTracks();
	}
} 