using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputWait : MonoBehaviour {

    public InputField inputfield;
    public Sprite spriteWait; public Button ButtonToDisable; Sprite button_lastSprite;


    public delegate void StringProcessor( string s);
    public static StringProcessor SubscribeWhenStringReceived;

    string lastSet;

    public void OnTextUpdate(string s) {
        //OnTextUpdate(0, s);
        if (s == lastSet || inputfield.text == lastSet) return;
        SubscribeWhenStringReceived( inputfield.text);
        UI_Waiting();
    }

    public void SetInputText(string s){
        lastSet = s;
        inputfield.text = s;
    }

   /* public void OnTextUpdate(int n, string s) {
        if (n >= inputfields.Length) {
            Die("you do not have this many inputfields defined: " + n);
            return;
        }

        SubscribeWhenStringReceived(n, inputfields[n].text);
        UI_Waiting();
    }*/

    public void UI_Waiting() {
        print("Wait");
        ButtonToDisable.interactable = false;
        Sprite templast = ButtonToDisable.GetComponent<Image>().sprite;
        if(templast!=spriteWait) button_lastSprite = ButtonToDisable.GetComponent<Image>().sprite;
        ButtonToDisable.GetComponent<Image>().sprite = spriteWait;
    }

    public void UI_DoneWaiting() {
        print("Done");
        ButtonToDisable.interactable = true;
        if (button_lastSprite != null)
        ButtonToDisable.GetComponent<Image>().sprite = button_lastSprite;
    }
 
}
