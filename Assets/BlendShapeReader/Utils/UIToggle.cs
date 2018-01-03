using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIToggleSet{
    public Sprite sprite1;
    public Sprite sprite2;
    public Image img;
}
 

public class UIToggle : MonoBehaviour {

    public UIToggleSet[] uitogglesets = new UIToggleSet[0];


    public void Toggle(int which){ 
        if(which>=uitogglesets.Length){
            Die("you do not have this many uitogglesets defined: " + which);
            return;
        }

        UIToggleSet uiset = uitogglesets[which];
        if(uiset.img.sprite == uiset.sprite1){
            uiset.img.sprite = uiset.sprite2;
        }else if(uiset.img.sprite == uiset.sprite2){
            uiset.img.sprite = uiset.sprite1;
        }

    }

    public void SetImage2( int n ){
        UIToggleSet uiset = uitogglesets[n];
        uiset.img.sprite = uiset.sprite2;
    }
    public void SetImage1(int n) {
        UIToggleSet uiset = uitogglesets[n];
        uiset.img.sprite = uiset.sprite1;
    }


    bool Die(string msg){
        print(msg);
        return false;
    }
}
