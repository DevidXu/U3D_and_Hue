using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GUI_Control : MonoBehaviour {

	// Use this for initialization
	void Start () {
        buttonchoose = false;
        camera = GameObject.Find("Main Camera");
        data = GameObject.Find("Data");
        if (data!=null) camera.transform.position = data.GetComponent<Data_Share>().camerapos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    GameObject camera, data;
    public bool buttonchoose;
    void OnGUI()
    {
        if (GUI.Button(NewPos(0.9f, 0.1f, 0.09f, 0.07f), "Return"))
        {
            buttonchoose = true;
            SceneManager.LoadScene("Main");
        }

        if (GUI.Button(NewPos(0.9f, 0.2f, 0.09f, 0.07f), "Center"))
        {
            buttonchoose = true;
            camera.transform.position = new Vector3(0.0f, 0.0f, camera.transform.position.z);
        }
    }

    Rect NewPos(float beginx, float beginy, float lx, float ly)
    {
        return new Rect(Screen.width * beginx, Screen.height * beginy, Screen.width * lx, Screen.height * ly);
    }
}
