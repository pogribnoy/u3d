using UnityEngine; 
using System; 
using System.Collections; 

public class GameObjectPool { 

	private GameObject _prefab; 
	private Stack available; 
	private ArrayList all; 
			
	private Action<GameObject> initAction; 
	private bool setActiveRecursively; 
			
	// ---- getters & setters ---- 
	#region getters & setters 
			
	// returns the prefab being used by the pool. 
	public GameObject prefab { 
		get { return _prefab; } 
	} 
			
	// returns the number of active objects. 
	public int numActive { 
		get { return all.Count - available.Count; } 
	} 
			
	// returns the number of available objects. 
	public int numAvailable { 
		get { return available.Count; } 
	} 
			
	#endregion 
	// ---- constructor ---- 
	#region constructor 
			
	public GameObjectPool(GameObject prefab, uint initialCapacity, Action<GameObject> initAction, bool setActiveRecursively) { 
		this._prefab = prefab; 
		this.initAction = initAction; 
		this.setActiveRecursively = setActiveRecursively; 
				
		available =    (initialCapacity > 0)    ? new Stack((int) initialCapacity)  : new Stack(); 
		all =  (initialCapacity > 0)    ? new ArrayList((int) initialCapacity)    : new ArrayList(); 
	} 
			
	#endregion 
	// ---- public methods ---- 
	#region public methods 
			
	public GameObject Spawn(Vector3 position, Quaternion rotation) { 
		GameObject result; 
				
		if (available.Count == 0){ 
				 
			// create an object and initialize it. 
			result = GameObject.Instantiate(prefab, position, rotation) as GameObject; 
			 
			// run optional initialization method on the object 
			if (initAction != null) initAction(result); 
				 
			all.Add(result); 
				 
		} else { 
			result = available.Pop() as GameObject; 
				 
			// get the result's transform and reuse for efficiency. 
			// calling gameObject.transform is expensive. 
			Transform resultTrans = result.transform; 
			resultTrans.position = position; 
			resultTrans.rotation = rotation; 
				 
			SetActive(result, true); 
		} 
		return result; 
	} 
	
	public bool Destroy(GameObject target) { 
		if (!available.Contains(target)) { 
			available.Push(target); 
				 
			SetActive(target, false); 
			return true; 
		}
		return false; 
	} 
			
	// Unspawns all the game objects created by the pool. 
	public void DestroyAll() { 
		for (int i=0; i<all.Count; i++) { 
			GameObject target = all[i] as GameObject; 
			if (target.activeSelf) Destroy(target); 
		} 
	} 
	
	// Unspawns all the game objects and clears the pool. 
	public void Clear() { 
		DestroyAll(); 
		available.Clear(); 
		all.Clear(); 
	} 
		 
	public bool Unspawn (GameObject obj) { 
		if (!available.Contains (obj)) { 
			// Make sure we don't insert it twice. 
			available.Push (obj); 
			SetActive (obj, false); 
			return true; 
			// Object inserted back in stack. 
		} 
		return false; 
		// Object already in stack.
	} 
			
	// Applies the provided function to some or all of the pool's game objects. 
	public void ForEach(Action<GameObject> action, bool activeOnly) { 
		for (int i=0; i<all.Count; i++){ 
			GameObject target  = all[i] as GameObject; 
			if (!activeOnly || target.activeSelf) action(target); 
		} 
	} 
			
	 public void prePopulate (int num) { 
		GameObject[] array = new GameObject[num]; 
		for (int i = 0; i < num; i++) { 
			array[i] = Spawn (Vector3.zero, Quaternion.identity); 
			SetActive (array[i], true); 
		} 
		for (int j = 0; j < num; j++) { 
			Unspawn (array[j]); 
		} 
	} 
			
	#endregion 
	// ---- protected methods ---- 
	#region protected methods 
			
	// Activates or deactivates the provided game object using the method 
	// specified by the setActiveRecursively flag. 
	protected void SetActive(GameObject target, bool value) { 
		if (setActiveRecursively) target.SetActive(value); 
		else target.SetActive(value); 
	} 
			
	#endregion 
} 