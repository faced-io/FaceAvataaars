using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;


public class PuppeteerByFace : MonoBehaviour {

    private UnityARSessionNativeInterface m_session;

    Dictionary<string, float> currentBlendShapes;
    public Transform tHypotheticalFace;

    public delegate void ProcessEachBlendShapeUpdate( Dictionary<string, float> bs, Transform t );  
    public static ProcessEachBlendShapeUpdate SubscribeEachBlendShapeUpdate;

    public int totalFaces = 0;

    void Awake(){
        tHypotheticalFace = transform;
    } 

	void Start () {
        m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface();

        Application.targetFrameRate = 60;
        ARKitFaceTrackingConfiguration config = new ARKitFaceTrackingConfiguration();
        config.alignment = UnityARAlignment.UnityARAlignmentGravity;
        config.enableLightEstimation = false;

        if (config.IsSupported) {

            m_session.RunWithConfig(config);

            UnityARSessionNativeInterface.ARFaceAnchorAddedEvent += FaceAdded;
            UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdated;
            UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;

        } else print("Failed to start ARKit");

	}
	
    void FaceAdded(ARFaceAnchor anchorData) { 
        totalFaces++;
        print("FaceAdded: "+totalFaces);//+ anchorData.identifierStr); // identifierStr does not work
       
        currentBlendShapes = anchorData.blendShapes;
        tHypotheticalFace.position = UnityARMatrixOps.GetPosition(anchorData.transform);
        tHypotheticalFace.rotation = UnityARMatrixOps.GetRotation(anchorData.transform);
        SubscribeEachBlendShapeUpdate(currentBlendShapes,tHypotheticalFace);

    }

    void FaceUpdated(ARFaceAnchor anchorData) {
        currentBlendShapes = anchorData.blendShapes;
        tHypotheticalFace.position = UnityARMatrixOps.GetPosition(anchorData.transform);
        tHypotheticalFace.rotation = UnityARMatrixOps.GetRotation(anchorData.transform);
        SubscribeEachBlendShapeUpdate(currentBlendShapes, tHypotheticalFace);
    }

    void FaceRemoved(ARFaceAnchor anchorData) {
       // + anchorData.identifierStr);
        totalFaces--;
        print("FaceRemoved: "+totalFaces);
    }

	 
}
