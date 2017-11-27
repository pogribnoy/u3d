using UnityEngine; 
using System.Collections; 

public class BulletController : MonoBehaviour{ 
	public float currentSpeed = 0f;
	
	public float lifeTime = 5f; // время жизни
	public float burnTime = 0f;	// время создания
	
	public float afterLifeEndTimeout = 1f; // время жизни объекта после того, как закончится топливо
	public float updateTimeout = 0.1f; // задержка между обновлениями времени жизни
	
	public float DetecionCooliderScale = 1f	// у снарядов, которые взрываются на расстоянии, надо увеличить, у простых - по размеру снаряда, т.е. будет контактное попадание

  void Start(){ 
		burnTime = Time.time;
  } 
	
  void Update(){
		// смещение с предыдущего кадра
		float step = currentSpeed * Time.deltaTime;
		// изменяем положение
		transform.Translate(Vector3.forward * step, Space.World);
  } 
	
	// сбрасываем настройки, следует делать после удаления из сцены
	private void Reset(){ 
		currentSpeed = 0f;
		burnTime = 99999999f;
  }
	
	// используется для отсчета времени жизни
	public IEnumerator LifeTime(){
		while((Time.time-burnTime) <= lifeTime) {
			// задерживаем на таймаут
			yield return new WaitForSeconds(updateTimeout);
		}
		// время жизни вышло
		// TODO. Убрать анимацию двигателя
		isAlive = false;
		// взрываем через время
		yield return new WaitForSeconds(afterLifeEndTimeout);
		Explosion();
		
		// выходим из корутины
		yield break;
  } 
	
	// взрываем
	public void Explosion(){ 
		// TODO.
  }
	
} 