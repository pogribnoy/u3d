using UnityEngine;
using System.Collections;

public class UserUnitAI : MonoBehaviour {  
	public Transform Target; 		// Указываем переменную, к которой будет двигаться наш агент 
	public GameObject objTarget; 	// Указываем переменную? содержащую объект, с которым будет взаимодействовать наш юнит (атаковать, охранять, следовать и т.д.) 
	public NavMeshAgent navAgent; 	// Указываем переменную агента  
	public Transform Weapon;			// точка вылета пуль
	public Rigidbody _bullet; 		// тело пули
	public float hp = 100; 			// Переменная хп 
	private Transform Home;			// позиция, к которой привязан юнит
	
	public int healthBarWidth = 50;
	public int healthBarHeight  = 10;
	public Texture2D healthBarTexture;
	public Vector3 screenPosition;
	public Vector2 screenSizeMin;
	public Vector2 screenSizeMax;
	public bool isSelected = false;
	public bool isSelecting = false;
	public UserController uc;
	//private List<Command> cmds = new List<Command>();
	private int action = 0;
	private int relation = 0;
	
	private int ACTION_STOP = 0;
	private int ACTION_MOVE = 1;
	
	private int RELATION_INDIFFERENT = 0;
	private int RELATION_AGGRESSIVE = 1;
	
	private Resources Res;

	void Start () {
		GameObject ucObj = GameObject.Find("UserController");
		if(ucObj!=null) {
			uc = (UserController)ucObj.GetComponent<UserController>();
			Res = (Resources)uc.GetComponent<Resources>();

		}
		navAgent = (NavMeshAgent)gameObject.GetComponent("NavMeshAgent"); // Указываем, что переменная _agent - это наш агент.
		// надо настроить характеристики юнита, можно брать данные из ресурсов
		navAgent.radius = 3f;
		navAgent.acceleration = 1f;
		navAgent.speed = 3f;
		navAgent.stoppingDistance = 2f;
		navAgent.angularSpeed = 90f;
		navAgent.autoBraking = true;
	}  

	void Update () { 
		/*_home = transform.parent.transform; 
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if(Vector3.Distance(_agent.transform.position, player.transform.position) < 100){ 
			_target = player.transform;
		} 
		else{
			_target = _home;
		}
			
		if(Vector3.Distance(_agent.transform.position, player.transform.position) < 50){ 
			_agent.speed = 0;
			Fire();
		} 
		else{ 
			_agent.speed = 3.5f;  
		} 
		
		_agent.SetDestination(_target.position); // Заставляем агента двигаться в сторону _target'а */
		
		if(isSelected || isSelecting) {
			screenPosition = Camera.main.WorldToScreenPoint(transform.position);
			screenPosition.y = Screen.height - screenPosition.y; // координатная система GUI идет от низа экрана, а экранные координаты - от верха
			screenSizeMin = Camera.main.WorldToScreenPoint(gameObject.renderer.bounds.min);
			screenSizeMax = Camera.main.WorldToScreenPoint(gameObject.renderer.bounds.max);
			//gameObject.renderer.bounds.
		}
	}
	
	void OnGUI() { 
		if(isSelected) {
			// рамка вокруг юнита
			Drawing2D.DrawRect(Utils.obj3dProjection(gameObject, camera), Color.green, 1.0f, false);
			
			// рамка здоровья
			//healthBarWidth = (int)screenSizeMin.x; //(int)Mathf.Abs(screenSizeMin.x-screenPosition.x);
			GUI.DrawTexture(new Rect(screenPosition.x - healthBarWidth/2, screenPosition.y - healthBarHeight/2, healthBarWidth, healthBarHeight), healthBarTexture);
			
			int width = 200;
			int height = 200;
			int lineH = 10;
			// надо вывести панель команд и панель информации о юните
			GUI.BeginGroup (new Rect (Screen.width - width, Screen.height-height, width, height));
			
			GUI.Box(new Rect (Screen.width - width, Screen.height-height, width, height), "");
			GUI.Label(new Rect(0, 0, 200, lineH), "HP: " + hp); // Отображаем хп 
			if(Target!=null) GUI.Label(new Rect(0, 10, 200, lineH), "Target: " + Target.ToString()); // Отображаем цель 
			
			GUI.EndGroup ();
		}
		else if(isSelecting) {
			// рамка вокруг юнита
			Drawing2D.DrawRect(Utils.obj3dProjection(gameObject, Camera.main), Color.green, 1.0f, false);			
		}
	} 
	
	public void Fire(){
		// выстрел пушкой
		// берем пулю из пула
		GameObject bullet = Res.bulletPool.Spawn(Weapon.position, Weapon.rotation);
		// придаем ей скорость
		bullet.rigidbody.AddForce(transform.forward * 100); 
	}
	
	public void ApplyDamage(float damage) {
		hp -= damage; // Вычитаем из хп переменную, указанную в начале метода 
		if (hp <= 0) { // Если хп меньше или равно 0 
			hp=0;
			Detonate(); // Вызываем метод Detonate 
		} 
	} 

	private void Detonate(){ 
		// надо реализовать уничтожение юнита
		
		// возвращаем юнит в пул
		Res.unitPool.Unspawn(gameObject);
		
		// отображаем взрыв
	} 
	
	public void goTo(Transform target) {
		action = ACTION_MOVE;
		Target = target;
		navAgent.stoppingDistance = 0f;
		navAgent.SetDestination(Target.position);
	} 
	
	public void DeSelect(){
		isSelected = false;
		renderer.material.color = Color.green;
	}

	public void Select(){
		isSelected = true;
		renderer.material.color = Color.clear;
	}
	
}  