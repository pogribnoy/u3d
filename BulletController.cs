using UnityEngine; 
using System.Collections; 

public class BulletController : MonoBehaviour{ 
	public float currentSpeed = 0f;
	
	public float lifeTime = 5f; // ����� �����
	public float burnTime = 0f;	// ����� ��������
	
	public float afterLifeEndTimeout = 1f; // ����� ����� ������� ����� ����, ��� ���������� �������
	public float updateTimeout = 0.1f; // �������� ����� ������������ ������� �����
	
	public float DetecionCooliderScale = 1f	// � ��������, ������� ���������� �� ����������, ���� ���������, � ������� - �� ������� �������, �.�. ����� ���������� ���������

  void Start(){ 
		burnTime = Time.time;
  } 
	
  void Update(){
		// �������� � ����������� �����
		float step = currentSpeed * Time.deltaTime;
		// �������� ���������
		transform.Translate(Vector3.forward * step, Space.World);
  } 
	
	// ���������� ���������, ������� ������ ����� �������� �� �����
	private void Reset(){ 
		currentSpeed = 0f;
		burnTime = 99999999f;
  }
	
	// ������������ ��� ������� ������� �����
	public IEnumerator LifeTime(){
		while((Time.time-burnTime) <= lifeTime) {
			// ����������� �� �������
			yield return new WaitForSeconds(updateTimeout);
		}
		// ����� ����� �����
		// TODO. ������ �������� ���������
		isAlive = false;
		// �������� ����� �����
		yield return new WaitForSeconds(afterLifeEndTimeout);
		Explosion();
		
		// ������� �� ��������
		yield break;
  } 
	
	// ��������
	public void Explosion(){ 
		// TODO.
  }
	
} 