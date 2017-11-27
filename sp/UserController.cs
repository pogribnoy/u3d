using UnityEngine;
using System.Collections;
/*
Носитель: контроллер пользователя, на котором висит главная камера
Действие: рисует линии указателя пункта назначения с помощью mesh
*/
public class UserController :  MonoBehaviour {
	// Бездействие
	private int ST_INACTION = 1;
	// Нажата ЛКМ при бездействии
	private int ST_INACTION_LKM_PRESSED = 2;
	// Выделение
	private int ST_SELECTION = 3;
	// Маршрутизация 1
	private int ST_NAVIGATION_1 = 4;
	// Маршрутизация 2
	private int ST_NAVIGATION_2 = 5;
	// Приближение
	private int ST_ZOOM = 6;
	// Клик ЛКМ при бездействии
	private int ST_INACTION_LKM_CLICK = 7;
	// Закончено выделение
	private int ST_SELECTION_FINISHED = 8;
	// Указан маршрут
	private int ST_NAVIGATION_DONE = 9;
	// Обзор при маршрутизации 1
	private int ST_NAVIGATION_1_SURVEY = 10;
	// Нажата ЛКМ при маршрутизации 1
	private int ST_NAVIGATION_1_LKM_PRESSED = 11;
	// Обзор при бездействии
	private int ST_INACTION_SURVEY = 12;
	// Нажата Ctrl
	private int ST_CTRL_PRESSED = 13;
	// Обзор при маршрутизации 2
	private int ST_NAVIGATION_2_SURVEY = 14;
	// Нажата ЛКМ при маршрутизации 2
	private int ST_NAVIGATION_2_LKM_PRESSED = 15;
	
	public Rect Selector = new Rect(0,0,0,0);
	
	// Статус интерфейса		
	public int Status = 1;
	
	public float mouseXSpeed = 250f;
	public float mouseYSpeed = 120f;
	public float mouseZSpeed = 300f;
	
	private float doubleClickStart = 0;
	private float doubleClickLapseTime = 0.3f;

	public GameObject target;		// объект, на который смотрит камера
	public float targetDistance = 3.0f;
		
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float zMinLimit = 2f;
	public float zMaxLimit = 20f;

	private float x = 0.0f;
	private float y = 3.0f;
	
	//public Camera camera;
	//private Tracker tracker;

	public SelectedUnits selectedUnits;
		
	void Start() {
		//tracker = new Tracker ();
		// объект, на который будет смотреть камера
		target = GameObject.Find("unit");
		// создаем контейнер выделенных юнитов
		selectedUnits = gameObject.GetComponent<SelectedUnits>();
		if(selectedUnits==null) Debug.Log("selectedUnits not found");
	}

	void Update () {
		if (target) {
			// сохраняем текущую позицию
			Quaternion rotation = transform.rotation;
			Vector3 position = transform.position;

			// проверяем на то, что были нажаты клавиши
			if(Input.GetKeyDown(KeyCode.LeftShift)){
				// если нажали SHIFT
				if(Status == ST_NAVIGATION_1){
					Status = ST_NAVIGATION_2;
					//Debug.Log("Status = ST_NAVIGATION_2");
				}
				else if(Status == ST_NAVIGATION_1_SURVEY) { 
					Status = ST_NAVIGATION_2_SURVEY; 
					//Debug.Log("Status = ST_NAVIGATION_2_SURVEY"); 
				}
				else if(Status == ST_NAVIGATION_1_LKM_PRESSED) { 
					Status = ST_NAVIGATION_2_LKM_PRESSED; 
					//Debug.Log("Status = ST_NAVIGATION_2_LKM_PRESSED"); 
				}
				else if(Status == ST_NAVIGATION_2_SURVEY) { 
					Status = ST_NAVIGATION_1_SURVEY; 
					//Debug.Log("Status = ST_NAVIGATION_1_SURVEY"); 
				}
				else if(Status == ST_NAVIGATION_2_LKM_PRESSED) { 
					Status = ST_NAVIGATION_1_LKM_PRESSED; 
					//Debug.Log("Status = ST_NAVIGATION_1_LKM_PRESSED"); 
				}
			}
			else if (Input.GetKeyDown(KeyCode.LeftControl)){
				// если нажали CTRL
				if(Status != ST_SELECTION) { 
					Status = ST_CTRL_PRESSED; 
					//Debug.Log("Status = ST_CTRL_PRESSED"); 
				}
			}
			else if (Input.GetKeyDown(KeyCode.M)) {
				// если нажали M
				if(Status == ST_INACTION) { 
					Status = ST_NAVIGATION_1; 
					//Debug.Log("Status = ST_NAVIGATION_1"); 
				}
				else if(Status == ST_NAVIGATION_2) { 
					Status = ST_NAVIGATION_1; 
					//Debug.Log("Status = ST_NAVIGATION_1"); 
				}
				else if(Status == ST_INACTION_SURVEY) { 
					Status = ST_NAVIGATION_1; 
					//Debug.Log("Status = ST_NAVIGATION_1"); 
				}
			}
			else if (Input.GetKeyDown(KeyCode.F)){
				// если нажали F
				//Debug.Log("F key pressed"); 
				
			}
			// проверяем на то, что были отпущены клавиши
			else if (Input.GetKeyUp(KeyCode.LeftShift)){
				if(Status == ST_NAVIGATION_2) { 
					Status = ST_NAVIGATION_1; 
					//Debug.Log("Status = ST_NAVIGATION_1"); 
				}
			}
			else if (Input.GetKeyUp(KeyCode.LeftControl)){
				if(Status == ST_SELECTION) { 
					Status = ST_SELECTION_FINISHED; 
					//Debug.Log("Status = ST_SELECTION_FINISHED"); 
				}
				else if(Status == ST_CTRL_PRESSED) { 
					Status = ST_INACTION; 
					//Debug.Log("Status = ST_INACTION"); 
				}
			}
			
			// проверяем на то, что была нажата ЛК мыши
			if(Input.GetMouseButtonDown(0)) {
				if(Status==ST_INACTION) { 
					Status = ST_INACTION_LKM_PRESSED; 
					//Debug.Log("Status = ST_INACTION_LKM_PRESSED");
				}
				else if(Status==ST_NAVIGATION_1) { 
					Status = ST_NAVIGATION_1_LKM_PRESSED; 
					//Debug.Log("Status = ST_NAVIGATION_1_LKM_PRESSED");
				}
				else if(Status==ST_NAVIGATION_2) { 
					Status = ST_NAVIGATION_2_LKM_PRESSED; 
					//Debug.Log("Status = ST_NAVIGATION_2_LKM_PRESSED");
				}
				else if(Status==ST_CTRL_PRESSED) {
					Status = ST_SELECTION;
					//Debug.Log("Status = ST_SELECTION");
					
					// начинаем рисовать рамку выделения
					Selector.x = Input.mousePosition.x;
					Selector.y = Screen.height - Input.mousePosition.y;
				}
			}

			// проверяем на то, что была отпущена ЛК мыши (клик)
			if(Input.GetMouseButtonUp(0)) {
				if(Status==ST_SELECTION) {
					Status = ST_INACTION;
					//Debug.Log("Status = ST_INACTION");

					// снимаем выделение с выделенных унитов
					selectedUnits.removeAllUnits();
					
					// выделяем юниты, которые попали в выделение
					//Main.RG.ZOs.SelectUnits();
					// убираем селектор
					//Selector.Clear();
					// включаем вращение камеры
					//this.enable();
					Bounds bnds = new Bounds(new Vector3(Selector.center.x, Selector.center.y, 0), new Vector3(Selector.width, Selector.height, 0));
					GameObject[] allObject = FindObjectsOfType(typeof(GameObject)) as GameObject[]; 
					foreach(GameObject go in allObject) { 
						if(bnds.Contains(go.transform.position)) selectedUnits.addUnit(go); 
					}
				}
				else if(Status==ST_INACTION_LKM_PRESSED) {
					Status = ST_INACTION;
					
					// проверяем, не двойной ли это клик
					if ((Time.time - doubleClickStart) < doubleClickLapseTime){
						Debug.Log ("Double Click");
						//mainCam.transform.position = new Vector3(-0.13f, 0.87f, -8); 
						
						//Camera.main.orthographicSize = 0.4f; 
						doubleClickStart = -1;
					}
					else{
						doubleClickStart = Time.time;
					}
					
					//Debug.Log("Status = ST_INACTION");
					// кликнули при ненажатом Ctrl - снимаем выделение с юнитов или выделяем новый юнит
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, 1000)) {
						//Debug.DrawLine(ray.origin, hit.point);
						if(hit.collider.gameObject.tag == "unit") {
							hit.collider.gameObject.renderer.material.color = Color.blue;
							selectedUnits.addUnit(hit.collider.gameObject);
							Debug.Log("Clicked object " + hit.collider.gameObject.tag);
						}
					}
					else {
						selectedUnits.removeAllUnits();
						Debug.Log("Deselect all");

					}
				}
				else if(Status==ST_NAVIGATION_1_SURVEY) Status = ST_NAVIGATION_1;
				else if(Status==ST_NAVIGATION_1_LKM_PRESSED) {
					Status = ST_INACTION;
					//Debug.Log("Status = ST_INACTION");
					// команда на движение
					//navigate();
				}
				else if(Status==ST_INACTION_SURVEY) {
					Status = ST_INACTION;
					//Debug.Log("Status = ST_INACTION");
				}
				else if(Status==ST_NAVIGATION_2_SURVEY) Status = ST_NAVIGATION_2;
				else if(Status==ST_NAVIGATION_2_LKM_PRESSED) {
					Status = ST_INACTION;
					//Debug.Log("Status = ST_INACTION");
					// команда на движение
					//navigate();
				}
			}

			// проверяем на движение мыши
			float moveX = Input.GetAxis("Mouse X") * mouseXSpeed;// * 0.02f;
			float moveY = Input.GetAxis("Mouse Y") * mouseYSpeed;// * 0.02f;

			//transform.Translate(0, 0, translation);
			//transform.Rotate(0, rotation, 0);
			
			if(Mathf.Abs(moveX)!=0.0f || Mathf.Abs(moveY)!=0.0f) {
				if(Status==ST_INACTION_LKM_PRESSED) { 
					Status = ST_INACTION_SURVEY; 
					//Debug.Log("Status = ST_INACTION_SURVEY"); 
				}
				else if(Status==ST_NAVIGATION_1_LKM_PRESSED) { 
					Status = ST_NAVIGATION_1_SURVEY; 
					//Debug.Log("Status = ST_INACTION_SURVEY"); 
				}
				else if(Status==ST_NAVIGATION_2_LKM_PRESSED) { 
					Status = ST_NAVIGATION_2_SURVEY; 
					//Debug.Log("Status = ST_INACTION_SURVEY"); 
				}
				else if(Status==ST_SELECTION){
					// выделяем: рисуем рамку и определяем, кто в нее попал		
					//Selector.Update(x, y);
					// в OnGUI есть обращение к координате мыши, возможно, что здесь это не нужно
					Selector.width = Mathf.Abs(Input.mousePosition.x - Selector.x);
					Selector.height = Mathf.Abs(Input.mousePosition.y - Selector.y);
				}
				else if(Status==ST_INACTION_SURVEY) {
					moveX *= Time.deltaTime;
					moveY *= Time.deltaTime;
					x += moveX;//Input.GetAxis("Mouse X") * mouseXSpeed * 0.02f;
					y -= moveY;//Input.GetAxis("Mouse Y") * mouseYSpeed * 0.02f;
					
					y = ClampAngle(y, yMinLimit, yMaxLimit);
					
					rotation = Quaternion.Euler(y, x, 0);
				}
			}
			
			// проверяем на ролик мыши
			if(Input.GetAxis("Mouse ScrollWheel")!=0) {
				if(((Status > 0) && (Status < 6))||Status == ST_CTRL_PRESSED){
					// зуммируем
					targetDistance -= Input.GetAxis("Mouse ScrollWheel") * mouseZSpeed * 0.02f;
					targetDistance = ClampZoom(targetDistance, zMinLimit, zMaxLimit);
					//Debug.Log("camera.z=" + transform.position.z);
				}
			}

			// обновляем позицию
			position = rotation * (new Vector3(0.0f, 0.0f, -targetDistance)) + target.transform.position;
			transform.rotation = rotation;
			transform.position = position;
		}
	}

	public static float ClampAngle (float angle, float min, float max) {
		if (angle < -360) angle += 360;
		if (angle > 360) angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
	public static float ClampZoom (float dist, float min, float max) {	
		return Mathf.Clamp (dist, min, max);
	}
	
	void OnGUI(){
		if(Status==ST_SELECTION) {
			// если выполняется выделение, рисуем рамку выделения (селектор)
			Vector2 pointA = new Vector2(Selector.x, Selector.y);			
			Vector2 pointB = Input.mousePosition;
			pointB.y = Screen.height - pointB.y;
			Vector2 pointC = new Vector2(pointB.x, Selector.y);
			Vector2 pointD = new Vector2(Selector.x, pointB.y);
			Drawing2D.DrawLine(pointA, pointC, Color.blue, 1.0f, false);
			Drawing2D.DrawLine(pointC, pointB, Color.blue, 1.0f, false);
			Drawing2D.DrawLine(pointB, pointD, Color.blue, 1.0f, false);
			Drawing2D.DrawLine(pointD, pointA, Color.blue, 1.0f, false);
		}
	}
} 