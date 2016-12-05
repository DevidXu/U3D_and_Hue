using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GUI_Control3D : MonoBehaviour {

	// Use this for initialization
	void Start () {
        person = GameObject.Find("Person");
        datacube = GameObject.Find("Data");
        if (datacube!=null) person.transform.position = datacube.GetComponent<Data_Share>().humanpos;
	}

    public bool buttonchoose;
    GameObject person, datacube;
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(NewPos(0.9f, 0.1f, 0.09f, 0.07f), "Return"))
        {
            if (datacube != null) datacube.GetComponent<Data_Share>().LoadHumanPos(person.transform.position);
            buttonchoose = true;
            SceneManager.LoadScene("Main");
        }

        if (GUI.Button(NewPos(0.9f, 0.2f, 0.09f, 0.07f), "Center"))
        {
            buttonchoose = true;
            person.transform.position = new Vector3(0.0f, 0.0f, person.transform.position.z);
        }
    }

    Rect NewPos(float beginx, float beginy, float lx, float ly)
    {
        return new Rect(Screen.width * beginx, Screen.height * beginy, Screen.width * lx, Screen.height * ly);
    }
}
