using UnityEngine;
using System.Collections;

public class RmBtn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
        paper = GameObject.Find("Paper");
        applybtn = GameObject.Find("ApplyBtn");
	}

    GameObject canvas, paper, applybtn;
    
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        GameObject currentlight = paper.GetComponent<Light_Set>().currentlight;
        // Delete the hue light from Paper
        paper.GetComponent<HueLight>().DeleteLight(ref currentlight);
        paper.GetComponent<Light_Set>().canvaschoose = false;
        applybtn.GetComponent<Apply>().isopen = false;
        canvas.SetActive(false);
    }
}
