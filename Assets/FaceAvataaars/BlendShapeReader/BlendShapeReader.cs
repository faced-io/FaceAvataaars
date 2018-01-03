/*// BlendShapeReader.cs v0.02  //TODO use coroutines for timer instead of update lolz

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

    static List<Dictionary<string, float>> listBlendShapes = new List<Dictionary<string, float>>();
    static List<float> listTimes = new List<float>();
    static List<Vector3> listPos = new List<Vector3>();
    static List<Quaternion> listRot = new List<Quaternion>();
    public List<Quaternion> camRot = new List<Quaternion>();

    public bool playing = false; float time = 0f;

    float nextTime = 0f; int current = 0;

    //public BlendShapeMapper bsm;

    public string teststringurl = "";

    public bool useDelegate = false;
    public delegate void ProcessEachBlendShapeUpdate(float time, Dictionary<string, float> blendshapes, Vector3 pos, Quaternion rot, Quaternion camrot);
    public static ProcessEachBlendShapeUpdate SubscribeEachBlendShapeUpdate;

    public delegate void ProcessEachBlendShapeUpdateBasic(float time, Dictionary<string, float> blendshapes);
    public static ProcessEachBlendShapeUpdateBasic SubscribeEachBlendShapeUpdateBasic;

    public UIInputWait inputwait;

    public Transform tHead;

    void Start() {
        LoadBlendShapesURL(teststringurl); //test

        UIInputWait.SubscribeWhenStringReceived += LoadBlendShapesURL;
        // SubscribeEachBlendShapeUpdate += CustomProcessBlendShape(time,dicBlendShapes);
    }

    public void LoadBlendShapesURL(string url) {
        if (url == null || url.Length < 5) return;
        if (url != PlayerPrefs.GetString("lastblendshape_url")) {
            StartCoroutine(ActuallyLoadBlendShape(url));
        } else
            LoadBlendShapes(PlayerPrefs.GetString("lastblendshape_string"));
    }
    IEnumerator ActuallyLoadBlendShape(string url) {
        WWW w = new WWW(url);
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

    public void LoadBlendShapes(string s) {

        BlendShapeHelper.ReadBlendShapes(s, out listBlendShapes, out listTimes, out listPos, out listRot, out camRot);
        time = 0f;
        nextTime = listTimes[0];
        print("Loaded BlendShapeString Characters: " + s.Length + " frames: " + listTimes.Count);


    }

    void Update() {
        if (playing) {
            time += Time.deltaTime;

            if (NextTimeInc()) {
                if(listPos.Count >= (current+1))
                  LoadBlendShapes(nextTime, listBlendShapes[current], listPos[current], listRot[current], camRot[current]);
                else 
                    LoadBlendShapes(nextTime, listBlendShapes[current]);
            }
        }
    }

    public void StartPlaying() {
        print("StartPlaying");
        playing = true;

        time = 0f;
        nextTime = listTimes[0];
        if (listRot.Count > 0 && listPos.Count > 0 && camRot.Count > 0)
            LoadBlendShapes(nextTime, listBlendShapes[0], listPos[0], listRot[0], camRot[0]);
        else
            LoadBlendShapes(nextTime, listBlendShapes[0]);

    }
    public void StopPlaying() {
        print("StopPlaying");
        playing = false;
    }
    public void TogglePlaying() {
        if (playing)
            StopPlaying();
        else
            StartPlaying();
    }

    void LoadBlendShapes(float t, Dictionary<string, float> blendshapes) {
        if (useDelegate)
            SubscribeEachBlendShapeUpdateBasic(t, blendshapes);
       /* else
            bsm.PlayConvertedRemapping(t, blendshapes);*/
    }

    void LoadBlendShapes(float t, Dictionary<string, float> blendshapes, Vector3 pos, Quaternion rot, Quaternion camrot) {

        if (useDelegate) {
            SubscribeEachBlendShapeUpdate(t, blendshapes, pos, rot, camrot);
        } else {
           // bsm.PlayConvertedRemapping(t, blendshapes);

            if (pos != null) {
                tHead.transform.position = pos;
                tHead.transform.rotation = rot * Quaternion.Euler(0, 180f, 0);

                Camera.main.transform.rotation = camrot;
            }
        }

    }

    bool NextTimeInc() {
        if (time > nextTime) {
            current++;
            if (current >= listTimes.Count) {
                current = 0;
                time = 0f;
            }

            nextTime = listTimes[current];
            //print (nextTime);
            return true;
        } else
            return false;
    }

}
