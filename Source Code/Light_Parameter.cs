using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Light_Parameter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        huelights = new HueLight();
        camera = GameObject.Find("Main Camera");
        canvas = GameObject.Find("Canvas");

        canvas.SetActive(false);
        rank = 0;

	}

    public HueLight huelights;
    GameObject camera, canvas;
    public int rank;
    bool canvaschoose;
	// Update is called once per frame
	void Update () {
        if (canvas.activeSelf) return;
        rank = 100;
#if UNITY_EDITOR_WIN
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 Point = World_Position(Input.mousePosition);
            rank = huelights.FindClosestLight(Point, camera.transform.position.z);
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Vector2 Point = World_Position(Input.touches[0].position);
            rank = huelights.FindClosestLight(Point, camera.transform.position.z);
        }
#endif

        if (rank > 30) return;
        else
        {
            canvas.SetActive(true);
            GameObject.Find("Switch").GetComponent<Toggle>().isOn = huelights.GetIOpen(rank);
        }
	}

    Vector2 World_Position(Vector2 screenpos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector2 Point = new Vector2(0.0f, 0.0f);
        if (Physics.Raycast(ray, out hit))
        {
            Point = hit.point;//得到碰撞点的坐标

        }
        else Debug.Log("The hit is out of screen");

        return Point;
    }
}
