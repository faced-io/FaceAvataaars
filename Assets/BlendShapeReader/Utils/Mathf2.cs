using UnityEngine;
using System.Collections;

public class Mathf2  {
	
	public static Camera cam;
	
	public static RaycastHit WhatDidWeHit(Vector2 v){
		//Debug.Log (cam.name);
		Ray ray = cam.ScreenPointToRay (v);
		RaycastHit hit = new RaycastHit ();
		hit.point = FarFarAway;
		//Debug.Draw
		if(Physics.Raycast(ray, out hit,3000f)){
			return hit;
		}
		return hit;
	}
	
	public static float Round10th(float n){
		return Mathf.Round(n*10f)/10;
	}
	
	public static string GenUUID(){
		return System.Guid.NewGuid().ToString("D");
	}
	
	public static Vector3 RoundVector3(Vector3 v){
		return new Vector3(round(v.x),round(v.y),round(v.z));
	}

	public static Quaternion RandRot(){
		return Quaternion.Euler (Random.Range (0, 360f), Random.Range (0, 360f), Random.Range (0, 360f));
	}

	public static Quaternion RandRotY(){
		return Quaternion.Euler (0, Random.Range (0, 360f),0);
	}
	
	public static float round(float f){
		return Mathf.Round(f);
	}
	
	public static float String2Float (string str){
		return float.Parse (str);
	}
	
	public static bool isNumeric(string str){
		int temp = -1;
		if(int.TryParse(str,out temp)){
			return true;
		}else return false;
	}
	
	public static int String2Int (string str) {
		return (int)Mathf.Floor(String2Float(str));
	}
	
	public static Color String2Color (string str){
		str = str.Replace("RGBA(","");str = str.Replace(")","");
		string[] a = str.Split(","[0]);
		return new Color(String2Float(a[0]),String2Float(a[1]),String2Float(a[2]),String2Float(a[3]));
	}
	
	public static Vector3 String2Vector3 (string str){
		str = str.Replace("(","");str = str.Replace(")","");
		string[] a = str.Split(","[0]);
		return new Vector3(String2Float(a[0]),String2Float(a[1]),String2Float(a[2]));
		
	}
	
	public static Vector2 String2Vector2 (string str){
		str = str.Replace("(","");str = str.Replace(")","");
		string[] a = str.Split(","[0]);
		return new Vector2(String2Float(a[0]),String2Float(a[1]));
		
	}
	
	public static float RoundFraction( float val,float denominator ){
		// rounds to nearest 1/denominator
		return Mathf.Floor(val * denominator) / denominator;
	}
	
	
	public static Quaternion String2Quat(string str){
		str = str.Replace("(","");str = str.Replace(")","");
		string[] a = str.Split(","[0]);		
		return new Quaternion(String2Float(a[0]),String2Float(a[1]),String2Float(a[2]),String2Float(a[3]));
	}
	
	public static Vector3 UnsignedVector3(Vector3 v){
		return new Vector3(Mathf.Abs (v.x),Mathf.Abs (v.y),Mathf.Abs (v.z));		
	}
	
	public static Vector3 FarFarAway = new Vector3(-9999f,-9999f,-9999f);
	
	public static string GetUnixTime(){
		var epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
		var timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
		return timestamp.ToString();
	}
	
	public static string GetPositionString(Transform t){
		return t.position.ToString ("F2");
	}
	
	public static string GetRotationString(Transform t){
		return t.rotation.ToString ("F6");
	}
	
	public static string GetScaleString(Transform t){
		return t.localScale.ToString ("F4");
	}
	
}