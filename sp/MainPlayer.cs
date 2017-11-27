using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	public string id;
	public float 
	public Rigidbody rb;
	public float maxSpeed;
	public float maxAccel;
	public Rigidbody Armor;
	public Vector3 speed = [10f,0f,0f];
	
	
	void Start () {
		id = 0;
		rb = GetComponent<Rigidbody>();
		
		// Change the mass of the object's Rigidbody.
		rb.mass = 10f;
		// Add a force to the Rigidbody.
    //rb.AddForce(Vector3.up * 10f);
		
		Debug.Log("Создан юнит: " + id);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("f")) {
			StartCoroutine("Fade");
		}
		if (Input.GetButtonDown("Fire1")) { // Ctrl
        FireRocket();
    }
		
		transform.Translate(0, 0, maxSpeed * Time.deltaTime);
	}
	
	void FireRocket() {
		Rigidbody rocketClone = (Rigidbody) Instantiate(rocket, transform.position, transform.rotation);
		rocketClone.velocity = transform.forward * speed;
	}
	
	void OnCollisionEnter(Collision otherObj) {
		if (otherObj.gameObject.tag == "Missile") {
				Destroy(gameObject,.5f);
		}
	}
	
	IEnumerator Fade() {
    for (float f = 1f; f >= 0; f -= 0.1f) {
        Color c = renderer.material.color;
        c.a = f;
        renderer.material.color = c;
        yield return null;
    }
	}
	
	bool ProximityCheck() {
    for (int i = 0; i < enemies.Length; i++) {
			if (Vector3.Distance(transform.position, enemies[i].transform.position) < dangerDistance) {
				return true;
			}
    }
    return false;
}
	
	IEnumerator DoCheck() {
    for(;;) {
			ProximityCheck();
			yield return new WaitForSeconds(.1f);
    }
	}
}