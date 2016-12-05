using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ChooseBulb : MonoBehaviour {

	// Use this for initialization
	void Start () {
        applybtn = GameObject.Find("ApplyBtn");
        paper = GameObject.Find("Paper");
	}

    GameObject applybtn, paper;
    void Update()
    {
        if (applybtn.GetComponent<Apply>().lk == LightKind.bulb)
            this.GetComponent<Toggle>().isOn = true;
        else this.GetComponent<Toggle>().isOn = false;
    }
    public void OnClick()
    {
        applybtn.GetComponent<Apply>().lk = LightKind.bulb;
    }
}
