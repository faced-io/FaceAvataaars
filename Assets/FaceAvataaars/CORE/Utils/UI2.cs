using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class UI2  
{
    public static Sprite CreateSprite(Texture2D tex){
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 72f);
    }

    public static GameObject CreateButton(GameObject prefab,string childForSprite,Sprite s,bool resize){
        GameObject g = GameObject.Instantiate(prefab) as GameObject;

        if(childForSprite.Length<1){
            g = AssignSprite(g, s);
        }else{
            g= AssignSprite(g.transform.Find(childForSprite), s);
        }

        if (resize) {
            Vector2 sd = g.GetComponent<RectTransform>().sizeDelta;
            Vector2 spritedims = new Vector2(s.texture.width, s.texture.height);
            float heighttowidth = spritedims.y / spritedims.x;
            sd = new Vector3(  sd.x/heighttowidth, sd.y);
            g.GetComponent<RectTransform>().sizeDelta = sd;
        }

        return g;

    }
    public static GameObject CreateButton(GameObject prefab,string childForSprite,Texture2D tex, bool resize){
        return CreateButton(prefab, childForSprite,CreateSprite(tex),resize);
    }
    public static GameObject AssignSprite(GameObject g,Sprite s){
         g.GetComponent<Image>().sprite = s;
        return g;
    }
    public static GameObject AssignSprite(Transform t, Sprite s) {
        t.GetComponent<Image>().sprite = s;
        return t.gameObject;
    }

    public static void ToggleButton(Transform t, bool f) {
        t.GetComponent<Button>().interactable = f;
    }

    public static void ClearGOs(Transform t){
        foreach(Transform child in t){
            GameObject.Destroy(t.gameObject);
        }
    }
    public static void ClearGOs(Transform t,string omit) {
        foreach (Transform child in t) {
            if(child.name!=omit)
            GameObject.Destroy(child.gameObject);
        }
    }
    public static GameObject CreateColorButton(GameObject prefab,Color color){
        GameObject g = GameObject.Instantiate(prefab) as GameObject;
        g.GetComponent<Image>().color = color;
        return g;
    }

}
