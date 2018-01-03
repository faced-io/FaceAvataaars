/*// BlendShapeReader.cs v0.02 (Created November 1, 2017 | I. Yosun Chang) added transform
/*
MIT License

Copyright(c) 2017-2018
I. Yosun Chang i@permute.xyz yosun@faced.io 415.779.6786

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
using System.Collections.Generic;
using UnityEngine; // ironically only needed for Debug.Log

public class BlendShapeHelper  {

	float lastval = 0;

    public static string DumpBlendShapes( List<Dictionary<string,float>> listBlendShapes,List<float> listTimes,List<Vector3> pos,List<Quaternion> rot,List<Quaternion> camrot){
		string s = "";

		for (int i = 0; i < listBlendShapes.Count; i++) {
            s += listTimes[i] + "," + GenBlendlet(listBlendShapes[i]); // first blendlet has time attached to it
		//	Debug.Log("jawOpen "+i+" "+listBlendShapes[i]["jawOpen"]);
            s += "*" + pos[i].ToString("F4") + "~" + rot[i].ToString("F4") +"~"+camrot[i].ToString("F4");
            s += "^";
		}
      
        Debug.Log("DumpBlendShapes " + listBlendShapes.Count);
		return s;
	}

	static string DumpAppleKeys(string s){
		string[] str = Parse.SSV (s); string dump = "{";
		for (int i = 0; i < str.Length; i++) {
			string[] str2 = Parse.CSV (str [i]);
			dump += "\""+str2 [0]+"\",";
		}
		return dump.Substring (0, dump.Length - 1) + "}";
	}

    static string DumpAppleKeysEnum(string s) {
        string[] str = Parse.SSV(s); string dump = "{";
        for (int i = 0; i < str.Length; i++) {
            string[] str2 = Parse.CSV(str[i]);
            dump +=  str2[0] + ",\n";
        }
        return dump.Substring(0, dump.Length - 1) + "}";
    } 


	static Dictionary<string,float> ParseBlendlet(string s){
		Dictionary<string,float> blendlet = new Dictionary<string,float> ();
		string[] str = Parse.SSV (s);
		for (int i = 0; i < str.Length; i++) {
		//	Debug.Log (str [i]);
			string[] str2 = Parse.CSV (str [i]);
			blendlet.Add (str2[0],Mathf2.String2Float(str2[1]));


		}
		return blendlet;
	}
	/*static List<float> ParseTime(string s){
		List<float> times = new List<float> ();
		string[] str = Parse.SSV (s);
		for (int i = 0; i < str.Length; i++) {
			string[] str2 = Parse.CSV (str [i]);
			times.Add (Mathf2.String2Float(str2[0]));
		}
		return times;
	}*/ //deprecated

	static string GenBlendlet(Dictionary<string,float> blendshapes){
		string s = "";
		foreach (KeyValuePair<string,float> kvp in blendshapes) {
			s += kvp.Key + "," + kvp.Value + ";";
			 
		}
		return s;
	}

	public static void ReadBlendShapes(string s,out List<Dictionary<string,float>> list,out List<float> times,out List<Vector3> pos,out List<Quaternion> rot,out List<Quaternion> camrot){ 
		list = new List<Dictionary<string,float>> ();
		times = new List<float> ();
        pos = new List<Vector3>();
        rot = new List<Quaternion>();
        camrot = new List<Quaternion>();

		if (s.Length > 3) {
			string[] str = Parse.CaretSV (s); 
 
			Debug.Log ("TotalBlendShapeFrames: "+str.Length);
			for (int i = 0; i < str.Length; i++) { 
                string[] sstar = Parse.StarSV(str[i]);
                string processed = sstar [0];

				string[] csv = Parse.CSV (processed);
				float t = Mathf2.String2Float (csv [0]);
				processed = DumpDelimiterAfter (1, ",", csv);

				list.Add (ParseBlendlet (processed)); 
			
				times.Add (t);

                if (sstar.Length > 1) {
                    string[] ttemp = Parse.TSV(sstar[1]);
                    pos.Add(Mathf2.String2Vector3(ttemp[0]));
                    rot.Add(Mathf2.String2Quat(ttemp[1]));
                    if(ttemp.Length>2)
                    camrot.Add(Mathf2.String2Quat(ttemp[2]));
                }

		/*		if (i == 0)
					Debug.Log(DumpAppleKeys (processed)); 
                if (i == 0)
                    Debug.Log(DumpAppleKeysEnum(processed));*/
				
			} 

			if (times [0] > 1f) {
				int timedelta = Mathf.FloorToInt(times[0]);

				for (int i = 0; i < times.Count; i++) {
					times [i] -= timedelta;
				}
			}

           

		} else
			Debug.Log (s.Length);
	 
	}

	static string DumpDelimiterAfter(int n,string delimiter,string[] s){
		string processed = "";
		for (int i = n; i < s.Length; i++) {
			processed += s[i]+delimiter;
		}
		return processed.Substring(0,processed.Length-1);
	}
}
