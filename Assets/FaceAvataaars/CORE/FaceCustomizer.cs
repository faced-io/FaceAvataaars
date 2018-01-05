/* v0.1 TODO: more involved shirt customizer
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
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FaceCustomizer : MonoBehaviour {

    public GameObject goUI_Customizable;

    [Header("Customizable")] 
    public Transform tSpriteButton_PartContainer;
    public GameObject goPrefabButton;

    public float buttonWidth;

    static Transform currentlyEditing;
    public Transform tStartLoad;

    [Header("Colors")]
    public Transform tSpriteButton_ColorContainer;
    public GameObject goPrefabButtonColor;

    void Start(){
        ViewonlyMode();
    }

    void Awake(){
        if(buttonWidth <= Mathf.Epsilon)
        buttonWidth = goPrefabButton.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void Toggle(){
        if(goUI_Customizable.activeSelf)
            ViewonlyMode();
        else
            EditableMode();
    }

    public void EditableMode(){
        LoopButtons(Avataaars.dicTransform2Sprite, true);
        goUI_Customizable.SetActive(true);
        SelectFacePart(tStartLoad);
    }

    public void ViewonlyMode(){
        LoopButtons(Avataaars.dicTransform2Sprite,false);
        goUI_Customizable.SetActive(false);
    }

    void LoopButtons(Dictionary<Transform,List<Sprite>> dic,bool f){
        foreach(Transform t in dic.Keys){
            UI2.ToggleButton(t, f);
        }
    }

    public void SelectFacePart( ){
        GameObject g = EventSystem.current.currentSelectedGameObject;

        if (Avataaars.dicTransform2Sprite.ContainsKey(g.transform)) {
            SelectFacePart(g.transform);
        }
    }

    void SelectFacePart(Transform t){
        print("Selected " + t.name);
        LoadFacePartChoices(t, Avataaars.dicTransform2Sprite[t]);
    }

    public void SelectButton(){
        GameObject g = EventSystem.current.currentSelectedGameObject;
        print("Selected " + g.name);
        if(currentlyEditing!=null){
            currentlyEditing.GetComponent<Image>().sprite = g.GetComponent<Image>().sprite;
        }
    }

    void LoadFacePartChoices(Transform t,List<Sprite> sprites){
        currentlyEditing = t;
        UI2.ClearGOs(tSpriteButton_PartContainer,"Button");
         for (int i = 0; i < sprites.Count; i++) { 
            GameObject g = UI2.CreateButton(goPrefabButton,"",sprites[i],true);
            g.transform.parent = tSpriteButton_PartContainer;
            g.SetActive(true);
            g.name = sprites[i].name;
            buttonWidth = g.GetComponent<RectTransform>().sizeDelta.x;
        }
       
        Vector2 sd = tSpriteButton_PartContainer.GetComponent<RectTransform>().sizeDelta;
        tSpriteButton_PartContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonWidth*sprites.Count ,sd.y);

        LoadColors(Avataaars.dicTransform2PartsAndFolderNames[t]);
    }

    void LoadColors(List<Avataaars.PartsAndFolderNames> pf){
        UI2.ClearGOs(tSpriteButton_ColorContainer, "ButtonCOLOR");
        List<Color> colors = new List<Color>();
        for (int i = 0; i < pf.Count;i++){
            for (int j = 0; j < pf[i].colors.Length; j++) {
                if (!colors.Contains((pf[i].colors[j]))) {
                    colors.Add(pf[i].colors[j]);
                }
            }
        }
        if (colors.Count == 0) tSpriteButton_ColorContainer.parent.gameObject.SetActive(false);
        else 
            tSpriteButton_ColorContainer.parent.gameObject.SetActive(true);
        for (int i = 0; i < colors.Count;i++){
            GameObject g = UI2.CreateColorButton(goPrefabButtonColor,colors[i]);
            g.transform.parent = tSpriteButton_ColorContainer;
            g.name = colors[i].ToString();
            g.SetActive(true);
        }
        Vector2 sd = tSpriteButton_ColorContainer.GetComponent<RectTransform>().sizeDelta;
        Vector2 sd2 = goPrefabButtonColor.GetComponent<RectTransform>().sizeDelta;
        tSpriteButton_ColorContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(sd2.x * colors.Count, sd.y);

    }

    public void SelectColor(){
        GameObject g = EventSystem.current.currentSelectedGameObject;

        currentlyEditing.GetComponent<Image>().color = g.GetComponent<Image>().color;
    
    
    }

 
}
