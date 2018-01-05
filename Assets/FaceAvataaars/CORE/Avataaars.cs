/* v0.2 
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
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public partial class Avataaars : MonoBehaviour {

    public string folderName = "Avataaars";
    public PartsAndFolderNames[] partsAndFolderNames = new PartsAndFolderNames[0];
    public AttachmentPointMapper[] attachmentPointObjs = new AttachmentPointMapper[0];
    public AvataaarPart[] crucialParts = new AvataaarPart[0]; // eyes and other things that require isOpened tag
    public EmotionsDefinable[] emotionsDefinable = new EmotionsDefinable[0];

    public static Dictionary<AttachmentPoints, List< AvataaarPart >> dicAtach2AvataaarPart = new Dictionary<AttachmentPoints, List< AvataaarPart >>();

    public static Dictionary<AttachmentPoints,Transform> dicAttachmentPoint2Transform = new Dictionary<AttachmentPoints, Transform>();
    public static Dictionary<Emotions, List< AvataaarPart> > dicEmo2Part = new Dictionary<Emotions, List < AvataaarPart >>();

    public static Dictionary<Transform, List<Sprite>> dicTransform2Sprite = new Dictionary<Transform, List<Sprite>>();
    public static Dictionary<Transform,List< PartsAndFolderNames>> dicTransform2PartsAndFolderNames = new Dictionary<Transform, List< PartsAndFolderNames>>();


    public Emotions currentEmotion;

    public Transform tHead;

    void Awake() {
        for (int i = 0; i < attachmentPointObjs.Length;i++){
            AttachmentPointMapper mapper = attachmentPointObjs[i];
         //   dicKey2Attachment.Add(mapper.appleKey,mapper);
            dicAttachmentPoint2Transform.Add(mapper.attachmentPoints,mapper.tParent);
        } 
        ProcessAvataaars();
    }

    void Start(){
        Scene current = SceneManager.GetActiveScene();
        if (current.name == "TheAvataaarPuppeteer"){ 
            BlendShapeReader.SubscribeEachBlendShapeUpdate += UpdateAvataaarFaceTime;
            BlendShapeReader.SubscribeEachBlendShapeUpdateBasic += UpdateAvataaarFaceTimeOnly;
        }else if (current.name == "TheAvataaarPuppeteerAR")
            PuppeteerByFace.SubscribeEachBlendShapeUpdate += UpdateAvataaarFaceFromTransform;
    }

    void ProcessAvataaars() {
        // crucial parts
        for (int i = 0; i < crucialParts.Length;i++){
          //  AddSprite(crucialParts[i].sprite,crucialParts[i]);
            AddAttachmentPoint(crucialParts[i]);
        }
 
        // customizable parts loading
        for (int i = 0; i < partsAndFolderNames.Length;i++){
            Sprite[] sprites = Resources.LoadAll<Sprite>(partsAndFolderNames[i].folderName);
            for (int j = 0; j < sprites.Length;j++){
                AddTransformSpritePoint(partsAndFolderNames[i].attachmentTransform, sprites[j]);
                AddTransformPartsfoldername(partsAndFolderNames[i].attachmentTransform,partsAndFolderNames[i]);
            }
        }
    }

    private void AddTransformSpritePoint(Transform t,Sprite s){
        if (dicTransform2Sprite.ContainsKey(t))
            dicTransform2Sprite[t].Add(s);
        else
            dicTransform2Sprite[t] = new List<Sprite> { s };
    }
    private void AddTransformPartsfoldername(Transform t, PartsAndFolderNames s) {
        if (dicTransform2PartsAndFolderNames.ContainsKey(t))
            dicTransform2PartsAndFolderNames[t].Add(s);
        else
            dicTransform2PartsAndFolderNames[t] = new List<PartsAndFolderNames> { s };
    }

    public void UpdatePartAppearance(AvataaarPart ap,float yscale){
        Transform t = dicAttachmentPoint2Transform[ap.attachmentPoint];
        t.GetComponent<Image>().sprite = ap.sprite;
        t.localScale = new Vector3(ap.xMult*ap.baseMultiplier, yscale, ap.baseMultiplier);
    //  if(ap.attachmentPoint == AttachmentPoints.EyeLeft)  print(ap.sprite.name);
    }

   /* private void AddSprite(Sprite sprite,AvataaarPart apart){
        if(apart.uniqueSprite)
            dicSprite2AvataaarPart.Add(sprite, apart);
    }*/
    private void AddAttachmentPoint(AvataaarPart apart) {
        if (dicAtach2AvataaarPart.ContainsKey(apart.attachmentPoint)) {
            dicAtach2AvataaarPart[apart.attachmentPoint].Add(apart);
        } else {
            dicAtach2AvataaarPart[apart.attachmentPoint] = new List<AvataaarPart>() { apart };
        }

        if (dicEmo2Part.ContainsKey(apart.emotions)) {
            dicEmo2Part[apart.emotions].Add(apart);
        } else {
            dicEmo2Part[apart.emotions] = new List<AvataaarPart>() { apart };
        }
    } 
 

    // see AvataaarsPuppeteering
    public void UpdateAvataaarFaceTransformOnly(Dictionary<string,float> bs){
        foreach (KeyValuePair<string, float> kvp in bs) {
            // if (kvp.Key == "eyeBlink_R") print("R"+kvp.Value);
            // if (kvp.Key == "eyeBlink_L") print("L"+kvp.Value);
            if (TryAssignEmotion(kvp.Key, kvp.Value))
                break;
            //TryMoveMouthOrEye(kvp.Key, kvp.Value); 
        }
        LoopBlendsThroughCrucialParts(bs);
    }

    public void UpdateAvataaarFaceFromTransform(Dictionary<string, float> bs,Transform t){
        UpdateAvataaarFaceTransformOnly(bs);
        RotateHead (t.rotation );
     //   tHead.position 
    }

    private void RotateHead(Quaternion q){
        q *= Quaternion.Euler(0, 180, 0);
        Vector3 e = q.eulerAngles;
        tHead.rotation = Quaternion.Euler(0,0, Mathf2.ClampAngle(-e.z,-10f,10f));
    }

    public void UpdateAvataaarFace(Dictionary<string, float> bs,Vector3 pos,Quaternion rot,Quaternion camrot) {
        

        UpdateAvataaarFaceTransformOnly(bs);
        RotateHead(rot);
        
    }
    public void UpdateAvataaarFaceTime(float time,Dictionary<string,float> bs,Vector3 pos,Quaternion rot,Quaternion camrot){
        
        UpdateAvataaarFace(bs,pos,rot,camrot); // actually not time dependent (?) TODO consider refactor
        
    }
    public void UpdateAvataaarFaceTimeOnly(float time, Dictionary<string, float> bs) {

        UpdateAvataaarFaceTransformOnly(bs); // actually not time dependent (?) TODO consider refactor

    }

}
