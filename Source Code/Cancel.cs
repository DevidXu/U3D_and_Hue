using UnityEngine;
using System.Collections;

public class Cancel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
        paper = GameObject.Find("Paper");
        applybtn = GameObject.Find("ApplyBtn");
	}
	
	// Update is called once per frame
    GameObject paper, canvas, applybtn;
	
    public void OnClick()
    {
        applybtn.GetComponent<Apply>().isopen = false;
        paper.GetComponent<Light_Set>().canvaschoose = false;
        canvas.SetActive(false);
    }
}
