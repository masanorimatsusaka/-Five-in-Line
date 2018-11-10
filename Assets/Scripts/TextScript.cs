using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour {
    private Text blackText;
    private Text whiteText;
	// Use this for initialization
	void Start () {
        blackText = this.transform.GetChild(0).GetComponent<Text>();
        whiteText = this.transform.GetChild(1).GetComponent<Text>();
    }
	
    public void SetText()
    {
        blackText.text = "黒："+StoneCalculationScript.blackScore;
        whiteText.text = "白："+StoneCalculationScript.whiteScore;
    }
}
