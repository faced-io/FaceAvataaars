/*// BlendShapeReader.cs v0.011 (Created November 1, 2017 | Modified Jan 3, 2018 | I. Yosun Chang - added UIInputWait)

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeReader : MonoBehaviour {

	static List<Dictionary<string,float>> listBlendShapes = new List<Dictionary<string,float>>();
	static List<float> listTimes = new List<float>();

	public bool playing=false; float time = 0f;

	float nextTime = 0f; int current = 0;

	//public BlendShapeMapper bsm; 

	public  string teststringurl="";

    public bool useDelegate = false;
    public delegate void ProcessEachBlendShapeUpdate(float time,Dictionary<string, float> blendshapes);
    public static ProcessEachBlendShapeUpdate SubscribeEachBlendShapeUpdate;

    public UIInputWait inputwait;  

	void Start(){ 
		LoadBlendShapesURL (teststringurl); //test

        UIInputWait.SubscribeWhenStringReceived += LoadBlendShapesURL;
       // SubscribeEachBlendShapeUpdate += CustomProcessBlendShape(time,dicBlendShapes);
	}

	public void LoadBlendShapesURL(string url){
        if (url==null||url.Length<5) return;
		if (url != PlayerPrefs.GetString ("lastblendshape_url")) {
			StartCoroutine (ActuallyLoadBlendShape (url));
		} else
			 LoadBlendShapes(PlayerPrefs.GetString("lastblendshape_string"));
	}
	IEnumerator ActuallyLoadBlendShape(string url){
		WWW w = new WWW (url);
        print("Loading " + url);
		yield return w;

        if (w.error == null) {
            PlayerPrefs.SetString("lastblendshape_url", url);
            PlayerPrefs.SetString("lastblendshape_string", w.text);
            LoadBlendShapes(w.text);
            inputwait.UI_DoneWaiting();
        } else
            inputwait.SetInputText(w.error);
	}

	public void LoadBlendShapes(string s){
		
		BlendShapeHelper.ReadBlendShapes (s, out listBlendShapes, out listTimes);
		time = 0f;
		nextTime = listTimes [0];
		print ("Loaded BlendShapeString Characters: "+s.Length + " frames: "+listTimes.Count);
 

	}

	void Update(){
		if (playing) {
			time += Time.deltaTime;

			if (NextTimeInc ()) {
				LoadBlendShapes (nextTime,listBlendShapes [current]);
			}
		}
	}

	public void StartPlaying(){
        print("StartPlaying");
		playing = true;

		time = 0f;
		nextTime = listTimes [0];
		LoadBlendShapes (nextTime,listBlendShapes [0]);

	}
	public void StopPlaying(){
        print("StopPlaying");
		playing = false;
	}
	public void TogglePlaying(){
		if (playing)
            StopPlaying ();
		else
			StartPlaying ();
	}
 
	void LoadBlendShapes(float t,Dictionary<string,float> blendshapes){ 
		//bsm.PlayConvertedRemapping (t,blendshapes);

        if(useDelegate){
            SubscribeEachBlendShapeUpdate(t, blendshapes);
        }
	}

	bool NextTimeInc(){ 
		if (time > nextTime) {
			current++;
			if (current >= listTimes.Count) {
				current = 0;
				time = 0f;
			}

			nextTime = listTimes [current];
			//print (nextTime);
			return true;
		}else 
			return false;
	}

}
