using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeightSet : MonoBehaviour {

	// Use this for initialization
	void Start() {
        applybtn = GameObject.Find("ApplyBtn");
	}

    GameObject applybtn;
    void Update()
    {
        this.GetComponent<Slider>().value = applybtn.GetComponent<Apply>().height;
    }

    public void SliderTest()
    {
        float height = this.GetComponent<Slider>().value;
        applybtn.GetComponent<Apply>().height = height;
    }

}
