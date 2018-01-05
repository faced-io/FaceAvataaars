using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Avataaars  : MonoBehaviour {

    static string[] appleKeys = new string[] { "eyeBlink_R", "eyeWide_R", "mouthLowerDown_L", "eyeLookDown_R", "cheekSquint_L", "mouthDimple_R", "browInnerUp", "eyeLookIn_L", "mouthPress_L", "mouthStretch_R", "browDown_L", "mouthFunnel", "noseSneer_L", "eyeLookOut_L", "eyeLookIn_R", "mouthLowerDown_R", "browOuterUp_R", "mouthLeft", "cheekSquint_R", "jawOpen", "eyeBlink_L", "jawForward", "mouthPress_R", "noseSneer_R", "jawRight", "mouthShrugLower", "eyeSquint_L", "eyeLookOut_R", "mouthFrown_L", "cheekPuff", "mouthStretch_L", "mouthRollLower", "mouthUpperUp_R", "mouthShrugUpper", "eyeSquint_R", "mouthSmile_L", "eyeLookDown_L", "eyeWide_L", "mouthClose", "jawLeft", "mouthDimple_L", "mouthFrown_R", "mouthPucker", "mouthRight", "browDown_R", "eyeLookUp_L", "mouthSmile_R", "mouthUpperUp_L", "browOuterUp_L", "mouthRollUpper", "eyeLookUp_R" };
    // unity keys generated from BlendShapeHelper.ReadBlendShapes() - choose enum or just string keys

    public enum Emotions {
        Neutral,
        Happy,
        Sad,
        // Angry
    }

    public enum AttachmentPoints {
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
        Freeform,// faceshop.io v0.1f AttachmentPoints
        EyebrowsBoth
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
    public class PartsAndFolderNames {
        public Transform attachmentTransform;
        public string generalClassifier;
        public string subClassifier;
        public string folderName;
        public Color[] colors;
    }

    [System.Serializable]
    public class AttachmentPointMapper {
        public AttachmentPoints attachmentPoints;
        // public string appleKey;
        public Transform tParent;
    }

    [System.Serializable]
    public class AvataaarPart {
        public string name;
        public Sprite sprite;
        public Vector3 anchorPos;

        public bool uniqueSprite = true;
        public float baseMultiplier = 1.5f;

        public bool emotionallyDependent = true;
        public Emotions emotions;// = new Emotions[0];

        public AttachmentPoints attachmentPoint;

        public bool greaterThan = true;
        public float threshhold = 0.5f;
        public string appleKey;

        public bool useThreshhold;
        public Vector2 yScaleMinMax = new Vector2(0.75f, 1.5f); // scaled from threshhold to max

        public int xMult = 1;
    }

    [System.Serializable]
    public class EmotionsDefinable {
        public string appleKey;
        public float threshhold; // >than
        public Emotions emotion;
    }

	 
}
