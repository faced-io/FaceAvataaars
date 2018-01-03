using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Parse {

	public static string[] DollarSV(string s){
		return processSplit (s, "$");
	}
	
	
	public static Dictionary<string,string> SSVCSV2Dic(string t){
		
		Dictionary<string,string> dic = new Dictionary<string,string>();
		t = removeTrailingChar(";",t);
		string[] a = t.Split(";"[0]);
		for(int i=0;i<a.Length;i++){
			string[] b = a[i].Split(","[0]);
			dic[b[0]] = b[1];
		}
		return dic;
		
	}
	
	// creates dictionary out of second element of ssvcsv
	public static Dictionary<string,string> SSVCSV2Dic2(string t){
		
		Dictionary<string,string> dic = new Dictionary<string,string>();
		t = removeTrailingChar(";",t);
		string[] a = t.Split(";"[0]);
		for(int i=0;i<a.Length;i++){
			string[] b = a[i].Split(","[0]);
			if(b.Length>2)dic[b[0]] = b[2];
			else dic[b[0]]="-";
		}
		return dic;
		
	}
	
	public static Dictionary<string,string> SSVCSV2Dic3(string t){
		
		Dictionary<string,string> dic = new Dictionary<string,string>();
		t = removeTrailingChar(";",t);
		string[] a = t.Split(";"[0]);
		for(int i=0;i<a.Length;i++){
			string[] b = a[i].Split(","[0]);
			if(b.Length>2)dic[b[0]] = b[3];
			else dic[b[0]]="-";
		}
		return dic;
		
	}
	
	/** @function {StringArray} processCSV converts amp (&) string to String[] 
	 * @param {String} t string to process
	 */
	public static string[] AmpSV(string t){//Debug.Log("CSV");
		return processSplit(t,"&");
	}
	
	/** @function {StringArray} processCSV converts CSV (,) string to String[] 
	 * @param {String} t string to process
	 */
	public static string[] CSV(string t){//Debug.Log("CSV");
		return processSplit(t,",");
	}
	
	/** @function {StringArray} processSSV converts SSV (;) string to String[] 
	 * @param {String} t string to process
	 */
	public static string[] SSV(string t){//Debug.Log("SSV");
		return processSplit(t,";");
	}
	
	/** @function {StringArray} processTSV converts TSV (~) string to String[] 
	 * @param {String} t string to process
	 */
	public static string[] TSV(string t){//Debug.Log("TSV");
		return processSplit(t,"~");
	}
	
	public static string[] DotSV(string t){
		return processSplit (t, ".");
	}
	
	public static string[] DSV(string t){
		return processSplit(t,"$");	
	}
	
	public static string[] ESV(string t){//Debug.Log("ESV");
		return processSplit(t,"!");
	}
	
	public static string[] EqSV(string t){
		return processSplit(t,"=");	
	}
	
	public static string[] CaretSV(string t){//Debug.Log("Caret");
		return processSplit(t,"^");
	}
	
	public static string[] PSV(string t){//Debug.Log("Caret");
		return processSplit(t,"|");
	}
	
	public static string[] SpaceSV(string t){
		return processSplit(t," ");	
	}
	
	public static string[] EnterSV(string t){
		return processSplit (t,"\n");
	}
	
	public static string[] processSplit(string t,string d){
		if (t == null)
			return null;
		if(t.Length>0){
			t = removeTrailingChar(d,t);
			return t.Split(d[0]);
		}else return null;
	}
	
	/** @function {String} removeTrailingChar removes trailing character c in t
	 * @param {String} t text
	 * @param {String} c character to remove
	 * @return t string t with trailing c removed
	 */
	public static string removeTrailingChar(string c,string t){
		if(t.Length>0){
			if(t.Substring(t.Length-1,1)==c)return t.Substring(0,t.Length-1);
			else return t;
		}else return t;
	}
	
	
	
}