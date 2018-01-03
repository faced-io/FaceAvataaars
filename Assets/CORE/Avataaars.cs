/* v0.1 
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

public enum Emotions{
    Neutral,
    Happy,
    Sad,
   // Angry
}

public enum AttachmentPoints{
    FaceMask,
    Background,
    Foreground,
    Postprocessing,
    Sky,
    NoseTip,
    NoseBase,
    NoseMid,
    NosePeak,
    Mouth,
    EyeLeft,
    EyeRight,
    EyebrowLeft,
    EyebrowRight,
    FaceLeft,
    FaceRight,
    Forehead,
    Chin,
    NeckBase,
    TopOfHead,
    Freeform
   /* eyeBlink_R,
    eyeWide_R,
    mouthLowerDown_L,
    eyeLookDown_R,
    cheekSquint_L,
    mouthDimple_R,
    browInnerUp,
    eyeLookIn_L,
    mouthPress_L,
    mouthStretch_R,
    browDown_L,
    mouthFunnel,
    noseSneer_L,
    eyeLookOut_L,
    eyeLookIn_R,
    mouthLowerDown_R,
    browOuterUp_R,
    mouthLeft,
    cheekSquint_R,
    jawOpen,
    eyeBlink_L,
    jawForward,
    mouthPress_R,
    noseSneer_R,
    jawRight,
    mouthShrugLower,
    eyeSquint_L,
    eyeLookOut_R,
    mouthFrown_L,
    cheekPuff,
    mouthStretch_L,
    mouthRollLower,
    mouthUpperUp_R,
    mouthShrugUpper,
    eyeSquint_R,
    mouthSmile_L,
    eyeLookDown_L,
    eyeWide_L,
    mouthClose,
    jawLeft,
    mouthDimple_L,
    mouthFrown_R,
    mouthPucker,
    mouthRight,
    browDown_R,
    eyeLookUp_L,
    mouthSmile_R,
    mouthUpperUp_L,
    browOuterUp_L,
    mouthRollUpper,
    eyeLookUp_R,*/
}

[System.Serializable]
public class PartsAndFolderNames{
    public AttachmentPoints attachmentPoint;
    public string generalClassifier;
    public string subClassifier;
    public string folderName;
}

[System.Serializable]
public class AttachmentPointMapper{
    public AttachmentPoints attachmentPoints;
   // public string appleKey;
    public Transform tParent; 
}

[System.Serializable]
public class AvataaarPart{ 
    public string name;
    public Sprite sprite;
    public Vector3 anchorPos;

    public bool uniqueSprite = true;
    public float baseMultiplier = 1.5f;

    public Emotions emotions;// = new Emotions[0];
    public AttachmentPoints attachmentPoint;

    public bool greaterThan = true;
    public float threshhold=0.5f;
    public string appleKey;

    public bool useThreshhold;
    public Vector2 yScaleMinMax=new Vector2(0.75f,1.5f); // scaled from threshhold to max
}

[System.Serializable]
public class EmotionsDefinable{
    public string appleKey;
    public float threshhold; // >than
    public Emotions emotion;
}

public partial class Avataaars : MonoBehaviour {

    static string[] appleKeys = new string[] { "eyeBlink_R", "eyeWide_R", "mouthLowerDown_L", "eyeLookDown_R", "cheekSquint_L", "mouthDimple_R", "browInnerUp", "eyeLookIn_L", "mouthPress_L", "mouthStretch_R", "browDown_L", "mouthFunnel", "noseSneer_L", "eyeLookOut_L", "eyeLookIn_R", "mouthLowerDown_R", "browOuterUp_R", "mouthLeft", "cheekSquint_R", "jawOpen", "eyeBlink_L", "jawForward", "mouthPress_R", "noseSneer_R", "jawRight", "mouthShrugLower", "eyeSquint_L", "eyeLookOut_R", "mouthFrown_L", "cheekPuff", "mouthStretch_L", "mouthRollLower", "mouthUpperUp_R", "mouthShrugUpper", "eyeSquint_R", "mouthSmile_L", "eyeLookDown_L", "eyeWide_L", "mouthClose", "jawLeft", "mouthDimple_L", "mouthFrown_R", "mouthPucker", "mouthRight", "browDown_R", "eyeLookUp_L", "mouthSmile_R", "mouthUpperUp_L", "browOuterUp_L", "mouthRollUpper", "eyeLookUp_R" };

    public string folderName = "Avataaars";
    public PartsAndFolderNames[] partsAndFolderNames = new PartsAndFolderNames[0];
    public AttachmentPointMapper[] attachmentPointObjs = new AttachmentPointMapper[0];
    public AvataaarPart[] crucialParts = new AvataaarPart[0]; // eyes and other things that require isOpened tag
    public EmotionsDefinable[] emotionsDefinable = new EmotionsDefinable[0];

 //   public static Dictionary<Sprite, AvataaarPart> dicSprite2AvataaarPart = new Dictionary<Sprite, AvataaarPart>();
    public static Dictionary<AttachmentPoints, List< AvataaarPart >> dicAtach2AvataaarPart = new Dictionary<AttachmentPoints, List< AvataaarPart >>();
    //public static Dictionary<string, AttachmentPointMapper> dicKey2Attachment = new Dictionary<string, AttachmentPointMapper>();
    public static Dictionary<AttachmentPoints,Transform> dicAttachmentPoint2Transform = new Dictionary<AttachmentPoints, Transform>();
    public static Dictionary<Emotions, List< AvataaarPart> > dicEmo2Part = new Dictionary<Emotions, List < AvataaarPart >>();

    public Emotions currentEmotion;

    void Awake() {
        for (int i = 0; i < attachmentPointObjs.Length;i++){
            AttachmentPointMapper mapper = attachmentPointObjs[i];
         //   dicKey2Attachment.Add(mapper.appleKey,mapper);
            dicAttachmentPoint2Transform.Add(mapper.attachmentPoints,mapper.tParent);
        } 
        ProcessAvataaars();
    }

    void Start(){
        BlendShapeReader.SubscribeEachBlendShapeUpdate += UpdateAvataaarFaceTime;

    }

    void ProcessAvataaars() {
        // crucial parts
        for (int i = 0; i < crucialParts.Length;i++){
          //  AddSprite(crucialParts[i].sprite,crucialParts[i]);
            AddAttachmentPoint(crucialParts[i]);
        }

        //TODO load folders for other parts
        for (int i = 0; i < partsAndFolderNames.Length;i++){
            
        }
    }

    public void UpdatePartAppearance(AvataaarPart ap,float yscale){
        Transform t = dicAttachmentPoint2Transform[ap.attachmentPoint];
        t.GetComponent<Image>().sprite = ap.sprite;
        t.localScale = new Vector3(ap.baseMultiplier, yscale, ap.baseMultiplier);
        print(ap.sprite.name);
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
    public void UpdateAvataaarFace(Dictionary<string, float> bs) {
        foreach (KeyValuePair<string, float> kvp in bs) {
           // if (kvp.Key == "eyeBlink_R") print("R"+kvp.Value);
           // if (kvp.Key == "eyeBlink_L") print("L"+kvp.Value);
            TryAssignEmotion(kvp.Key, kvp.Value);
            TryMoveMouthOrEye(kvp.Key, kvp.Value); 
        }
    }
    public void UpdateAvataaarFaceTime(float time,Dictionary<string,float> bs){
        UpdateAvataaarFace(bs);
        
    }

}
