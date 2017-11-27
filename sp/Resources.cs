using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;

public class Resources : MonoBehaviour{
	public string filePath = "resources.xml"; 			// файл ресурсов
	//public string[][] Texts;
	private List<List<string>> Texts;
	private Settings Set;					// настройки
	public GameObjectPool bulletPool;		// пул снарядов
	public GameObject bulletPrefab; 		// префаб снаряда
	
	public GameObjectPool unitPool;		// пул юнитов
	public GameObject unitPrefab; 		// префаб юнита
	
	public GameObjectPool cannonPool;		// пул пушек
	public GameObject cannonPrefab; 		// префаб пушки
		
	void Start(){
		Set = (Settings)gameObject.GetComponent<Settings>();
	
		// читаем файл с ресурсами
		/*XmlDocument xmlDoc = new XmlDocument ();
		try{		
			xmlDoc.Load(filePath);
		}
		catch(Exception ex){
			Debug.Log("Settings not loaded. Defaults will be used instead: "+ex.Message);
			Texts = new List<List<string>>(1);
			Texts[0] = new List<string>(1);
			// создаем пул пуль
			bulletPool = new GameObjectPool(bulletPrefab,500,bulletHandler,true); 
			bulletPool.prePopulate(500);
			
			// создаем пул юнитов
			unitPool = new GameObjectPool(bulletPrefab,100,unitHandler,true); 
			unitPool.prePopulate(100);
			return ;
		}

		if(xmlDoc!=null){
			XmlNodeList transList = xmlDoc.GetElementsByTagName ("Translation");
			Texts = new List<List<string>>(transList.Count);
			int i = 0;
			foreach (XmlNode trans in transList) { 
				List<string> t = new List<string>(transList.Count*2+1);

				XmlAttributeCollection attr = trans.Attributes; 
				// берем язык
				t.Add(attr[0].Value);
				
				int j=1;
				// пробегаемся по всем переводам терминов из языка
				XmlNodeList termList = xmlDoc.GetElementsByTagName ("Term");
				foreach (XmlNode term in termList) { 
					XmlNode node = term.SelectSingleNode("Key");
					t.Add(node.Value);
					node = term.SelectSingleNode("Value");
					t.Add (node.Value);
					//j+=2;				
				}
				Texts.Add (t);
				//i++;
			} 
		}*/
		
		// создаем пул пуль
		bulletPool = new GameObjectPool(bulletPrefab,500,bulletHandler,true); 
        bulletPool.prePopulate(500);
		
		// создаем пул юнитов
		unitPool = new GameObjectPool(bulletPrefab,100,unitHandler,true); 
        unitPool.prePopulate(100);
	}
		
	public string Text(string input){
		/*//for(int i=0; i<Texts.Count; i++){
		foreach (List<string> list in Texts) { 
			// ищем язык
			//if(Texts[i][0] == Set.Lang){
			if(list.IndexOf(Set.Lang)>=0){
				Debug.Log("Language found");
				// нашли нужный язык
				//for(int j=1; j<Texts[i].Count; j+=2){
					// ищем слово на инглише
				//	if(Texts[i][j] == input) return Texts[i][j+1];
				//}
			}
		}*/
		// сейчас передаем такое сообщение, а вообще надо возвращать input - оригинал
		//return "<No translation for '"+input+"'>";
		return input;
	}
	
	void bulletHandler(GameObject target){     
		target.name = "Bullet"; 
	} 
	void unitHandler(GameObject target){     
		target.name = "Simple unit"; 
	} 
	
} 