/* v0.1 
MIT License

Copyright(c) 2017-2018
I. Yosun Chang

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

partial class Avataaars  : MonoBehaviour {

      void LoopBlendsThroughCrucialParts(Dictionary<string,float> bs){

        for (int i = 0; i < crucialParts.Length;i++){
            AvataaarPart ap = crucialParts[i];
            foreach (KeyValuePair<string, float> kvp in bs) { 
                if((ap.emotionallyDependent)){
                    if(currentEmotion == ap.emotions && dicEmo2Part[ap.emotions].Contains(ap))
                        ProcessAP(ap, kvp.Key, kvp.Value);
                }else 
                    ProcessAP(ap, kvp.Key, kvp.Value);
            }
        }
   
    }

      bool TryAssignEmotion(string applekey, float val) {
        for (int i = 0; i < emotionsDefinable.Length; i++) {
            EmotionsDefinable ed = emotionsDefinable[i];
            if (ed.appleKey == applekey && val > ed.threshhold) {
                currentEmotion = ed.emotion;
                print("Emotion assigned " + currentEmotion);
                return true;
            }
        }
        currentEmotion = Emotions.Neutral;
        print("No Emotion assigned " + currentEmotion);
        return false;
    }
      void TryMoveMouthOrEye(string applekey, float val) {
        List<AvataaarPart> aps = dicEmo2Part[currentEmotion];
        if (aps.Count == 0)
            aps = dicEmo2Part[Emotions.Neutral];
        for (int i = 0; i < aps.Count; i++) {
            ProcessAP(aps[i],applekey,val);
        }
    }
    private void ProcessAP(AvataaarPart ap,string applekey,float val){
        if(ap.appleKey == applekey) {
            float percent;
            /* if ((aps[i].yScaleMinMax.x - aps[i].yScaleMinMax.y)<Mathf.Epsilon)
                  percent = aps[i].baseMultiplier;
             else */
            percent = ap.yScaleMinMax.x + (val - ap.threshhold) * ap.yScaleMinMax.y;

            if (ap.greaterThan) {
                if (val > ap.threshhold)
                    ConditionalThreshold(ap, percent);
            } else {
                if (val < ap.threshhold)
                    ConditionalThreshold(ap, percent);
            }

        }
    }
    private  void ConditionalThreshold(AvataaarPart ap,float percent){
        if (ap.useThreshhold) {
            UpdatePartAppearance(ap, percent);
        } else {
            UpdatePartAppearance(ap, ap.baseMultiplier);
        } 
    } 
}
