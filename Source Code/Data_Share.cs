using UnityEngine;
using System.Collections;

public class Data_Share : MonoBehaviour {

	// Use this for initialization
	void Start () {
        data = GameObject.Find("Data");
        Object.DontDestroyOnLoad(data);
        wallline = null;
        huelights = null;
        humanpos = new Vector3(0.0f, 0.0f, -0.95f);
        camerapos = new Vector3(0.0f, 0.0f, -40.0f);
	}

    GameObject data;
    public WallLine wallline;
    public HueLight huelights;
    public Vector3 camerapos;    // In all 2D scene
    public Vector3 humanpos;   // In 3D scene

	// Update is called once per frame
	void Update () {
	
	}

    public void LoadWallLine(WallLine wl)
    {
        wallline = wl;
    }

    public void LoadHueLight(HueLight hl)
    {
        huelights = hl;
    }

    public void LoadCameraPos(Vector3 pos)
    {
        camerapos = pos;
    }

    public void LoadHumanPos(Vector3 pos)
    {
        humanpos = pos;
    }

}
