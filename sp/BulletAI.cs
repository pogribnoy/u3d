using UnityEngine;
using System.Collections;

public class BulletAI : MonoBehaviour {
	public int Damage = 5;	// мощность
	public float lifeTime = 3f;	// время жизни
	public float force = 20f; // энергия
	private Resources Res;
	
	void Update(){
		lifeTime-=Time.deltaTime;
		if(lifeTime<=0){
			// возвращаем объект в пул
			Res.bulletPool.Unspawn(gameObject);
		}
	}
	
	void Start(){
		// достаем объект с ресурсами
		GameObject uc = GameObject.Find("UserController");
		Res = (Resources)uc.GetComponent<Resources>();
	}
	
	// Создаём метод с коллизией col 
	void OnCollisionEnter(Collision col) {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		ContactPoint contact = col.contacts[0]; //Создаём контакт коллизии 
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal); // Поворот коллизии 
		Vector3 pos = contact.point; // Позиция коллизии - это позиция контакта 
		Vector3 direct = transform.TransformDirection(Vector3.forward); // Дирекция 

		if (col.rigidbody) // Если коллизия с ригидбоди (rigidbody) 
			col.rigidbody.AddForceAtPosition(direct * force, col.transform.position); // Придаём импульс этому rigidbody 

		if(col.collider.tag == "unit"){ // Если коллизия unit 
			// отправляем сообщение о повреждении
			col.gameObject.SendMessage("ApplyDamage", Damage); // Вызываем у плеера метод ApplyDamage 
			
			// прячем пулю и играем анимацию взрыва
			
			// возвращаем объект в пул
			Res.bulletPool.Unspawn(gameObject);
			
			// отображаем взрыв
		}
		
		//Destroy(gameObject); // Удаляем пулю 
	}
}  