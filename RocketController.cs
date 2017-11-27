using UnityEngine; 
using System.Collections; 

public class RocketController : MonoBehaviour{ 
  public Transform target; // цель, если не задана, то это - неуправляемая ракета
  public bool isAlive = true;	// ракета активна - работает двигатель, наводится и пр.
	public float currentSpeed = 0f;
	public float maxSpeed = 10f;
	public float accel = 2f;
	
	public float lifeTime = 5f; // время жизни
	public float burnTime = 0f;	// время создания
	
	public float startEngineTimeout = 1f; // задержка включения двигателя
	public float afterLifeEndTimeout = 1f; // время жизни объекта после того, как закончится топливо
	public float correctTargetTimeout = 0.5f; // задержка между корректировками курса
	public float updateTimeout = 0.1f; // задержка между обновлениями времени жизни, значения скорости и пр.
	
	public float DetecionCooliderScale = 1f	// у ракет, которые взрываются на расстоянии, надо увеличить, у простых - по размеру ракеты, т.е. будет контактное попадание

  void Start(){ 
		burnTime = Time.time;
		// запускаем корутину для пуска двигателей с задержкой
    StartCoroutine(StartEngine());
		
		// если цель задана запускаем корутину для наведения ракеты
		if(target) StartCoroutine(CorrectTarget());
  } 
	
  void Update(){
		// смещение с предыдущего кадра
		float step = currentSpeed * Time.deltaTime;
		// изменяем положение
		//transform.position = Vector3.MoveTowards(transform.position, target.position, step);
		transform.Translate(Vector3.forward * step, Space.World);
  } 
	
	// сбрасываем настройки, следует делать после удаления из сцены
	private void Reset(){ 
		currentSpeed = 0f;
		burnTime = 99999999f;
		isAlive = true;
  }
	
	// используется для старта двигателя с отсрочкой
	public IEnumerator StartControlEngine(){ 
		// задерживаем на таймаут
    yield return new WaitForSeconds(startEngineTimeout);
		float last_time = Time.time;
		// TODO. Запустить визуализацию работы двигателя
		
		// ускорение должно быть только для разгона (увеличения скорости)
		while(currentSpeed<maxSpeed) {
			float accel_step = accel * (Time.time-last_time);
			currentSpeed += accel_step;
			
			yield return new WaitForSeconds(updateTimeout);
			last_time = Time.time
		}
			
		// выходим из корутины
		yield break;
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
	
	// корректировка направления
	public IEnumerator CorrectTarget(){ 
		while(isAlive && target != null) { //если текущая цель указана, то наводимся
				transform.LookAt(target); // поворот ракеты в сторону цели
				// TODO. OPT. Надо учитывать вектор смещения цели и направлять ракету на упреждение
			}
			// задерживаем на таймаут
			yield return new WaitForSeconds(correctTargetTimeout);
		}
		
		// выходим из корутины, если двигатель не работает или цель не указана
		yield break;
  }
} 