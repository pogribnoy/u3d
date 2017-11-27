using UnityEngine;
using System.Collections;

public class CannonAI : MonoBehaviour {  
	public GameObject Target; 	// Указываем объект, на который будет наводится наша пушка
	public NavMeshAgent Agent; // Указываем переменную агента  
	public Transform Weapon;		// точка вылета пуль
	public float hp = 100; 		// Переменная хп 
	private Transform Home;		// позицияпокоя пушки
	
	private Resources Res;

	void Start () {
		GameObject uc = GameObject.Find("UserController");
		if(uc!=null) Res = (Resources)uc.GetComponent<Resources>();
		//_agent = (NavMeshAgent)gameObject.GetComponent("NavMeshAgent"); // Указываем, что переменная _agent - это наш агент.  
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
		
	}
	
	void OnGUI() { 
		GUI.Label(new Rect(1, 61, 150, 50), "Cannon HP: " + hp); // Отображаем хп
		GUI.Label(new Rect(1, 121, 150, 50), "Cannon target: " + Target.tag); // Отображаем хп
	} 
	
	public void Fire(){
		// выстрел пушкой
		// берем пулю из пула
		GameObject bullet = Res.bulletPool.Spawn(Weapon.position, Weapon.rotation);
		// придаем ей скорость
		bullet.rigidbody.AddForce(transform.forward * 100); 
	}
	
	void ApplyDamage(float damage) {
		hp -= damage; // Вычитаем из хп переменную, указанную в начале метода 
		if (hp <= 0) { // Если хп меньше или равно 0 
			hp=0;
			Detonate(); // Вызываем метод Detonate 
		} 

	} 

	void Detonate(){ 
		// надо реализовать уничтожение юнита
		
		// возвращаем юнит в пул
		Res.cannonPool.Unspawn(gameObject);
		
		// отображаем взрыв
	} 
}  