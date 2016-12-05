using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Para_Switch : MonoBehaviour {

	// Use this for initialization
	void Start () {
        applybtn = GameObject.Find("ConfirmBtn");
        ground = GameObject.Find("Ground");
        open = false;
	}

    GameObject ground, applybtn;
    public bool open;
    bool cancelchange;
	// Update is called once per frame
	void Update () {
        if (!open)
        {
            int rank = ground.GetComponent<Light_Parameter>().rank;
            this.GetComponent<Toggle>().isOn = ground.GetComponent<Light_Parameter>().huelights.GetIOpen(rank);
        }
        open = this.isActiveAndEnabled;
	}

    public void OnClick()
    {
        applybtn.GetComponent<Para_Apply>().open = !applybtn.GetComponent<Para_Apply>().open;
    }
}
