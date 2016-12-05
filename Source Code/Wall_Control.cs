using UnityEngine;
using System.Collections;

public class House_Control : MonoBehaviour {

	// Use this for initialization
	void Start () {
        data = GameObject.Find("Data");
        wallline = data.GetComponent<Data_Share>().wallline;

        InitialWall(wallline);
	}

    GameObject data;
    WallLine wallline;
	// Update is called once per frame
	void Update () {
	
	}

    void InitialWall(WallLine wl)
    {
        
    }
}
