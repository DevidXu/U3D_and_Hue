using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Para_Pace : MonoBehaviour {

	// Use this for initialization
	void Start () {
        applybtn = GameObject.Find("ConfirmBtn");
	}

    GameObject applybtn;
	// Update is called once per frame
	void Update () {
        this.GetComponent<Slider>().value = applybtn.GetComponent<Para_Apply>().pace;
	}

    public void SliderTest()
    {
        if (applybtn.GetComponent<Para_Apply>().choosecolor) return;

        float pace = this.GetComponent<Slider>().value;
        applybtn.GetComponent<Para_Apply>().pace = pace;
    }
}
