using UnityEngine;
using System.Collections;

public class Fix_Object : MonoBehaviour {

	// Use this for initialization
	void Start () {
        datacube = GameObject.Find("Data");

        wallline = new WallLine();
        if (datacube != null)
        {
            SetInitHeight(wallline);
            SetInitMaterial(wallline);
        }
        huelights = new HueLight();
        if (datacube != null) huelights.SetRealSize(false);
	}

    GameObject datacube;
    WallLine wallline;
    HueLight huelights;
	// Update is called once per frame
	void Update () {
	
	}

    void SetInitHeight(WallLine wl)
    {
        int size = wl.GetSize();
        wl.SetAllHeight(3.0f);
    }

    public Material yellowwall;
    void SetInitMaterial(WallLine wl)
    {
        wl.SetAllMaterial(yellowwall);
    }
}
