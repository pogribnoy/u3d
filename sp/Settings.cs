using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class Settings  : MonoBehaviour{
	public string filePath = "settings.xml"; 			// текущий файл настроек
	public string defaultsFilePath = "settings.xml"; 	// файл настроек по умолчанию
	public string Lang = "EN";	// английский
	
	void Start(){
		// читаем файл с настройками и обновяем их
		int i = 0, j = 1;
		
		// читаем файл с ресурсами
		XmlDocument xmlDoc = new XmlDocument (); 
		try{
			xmlDoc.Load(filePath);
		}
		catch(Exception ex){
			//FileNotFoundException
			Debug.Log("Setting not loaded. Defaults will be used instead");
		}
	}
		
	public void Save(){
		// обновяем файл с настройками
		
	}
	
	public void LoadDefaults(){
		// загружаем настройки из файла настроек по умолчанию и сохраняем в текущий файл настроек
		
	}
	
} 