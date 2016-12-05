using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Apply : MonoBehaviour {

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
        paper = GameObject.Find("Paper");
        Canvas.ForceUpdateCanvases();
        isopen = false;
	}

    GameObject paper, canvas, currentlight;
    public LightKind lk;
    public float height;
    public bool isopen;

    void Update()
    {
        if (!isopen)
        {
            currentlight = paper.GetComponent<Light_Set>().currentlight;
            height = Mathf.Abs(currentlight.transform.position.z);
            lk = paper.GetComponent<Light_Set>().currenttype;
        }
        isopen = this.isActiveAndEnabled;
    }

	public void OnClick()
    {
        // If ensure, execute orders below (Set height and kind)
        currentlight = paper.GetComponent<Light_Set>().currentlight;
        paper.GetComponent<HueLight>().SetHeight(currentlight, -height);
        paper.GetComponent<HueLight>().SetLightKind(currentlight, lk);

        paper.GetComponent<Light_Set>().canvaschoose = false;
        isopen = false;
        canvas.SetActive(false);
    }
}
