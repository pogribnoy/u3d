using UnityEngine; 
using System.Collections; 
public class HighlightObject : MonoBehaviour{ 

  public Color initialColor; 
  public Color highlightColor; 
  public Color mousedownColor; 
  private bool mouseon = false; 

  void OnMouseEnter(){ 
   mouseon = true; 
   renderer.material.SetColor("_Emission", highlightColor); 
  } 

  void OnMouseExit(){ 
   mouseon = false; 
   renderer.material.SetColor("_Emission", initialColor); 
  } 

  void OnMouseDown(){ 
   renderer.material.SetColor("_Emission", mousedownColor); 
  } 

  void OnMouseUp(){ 
   if (mouseon) 
    renderer.material.SetColor("_Emission", highlightColor); 
   else 
    renderer.material.SetColor("_Emission", initialColor); 
  } 
} 