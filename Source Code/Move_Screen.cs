using UnityEngine;
using System.Collections;

public class Move_Screen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        camera = GameObject.Find("Main Camera");
        canvas = GameObject.Find("Canvas");
        distance = camera.transform.position.z;
        dmax = -10.0f; dmin = -40.0f;
        xmax = 30.0f; ymax = 30.0f;
	}

    GameObject camera, data, canvas;
	// Update is called once per frame
	void Update () {
        if (canvas!=null && canvas.activeSelf) return;
#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Moved)
                UpdateCameraPos();
        }
        else
            if (Input.touchCount == 2)
                if (Input.touches[0].phase == TouchPhase.Moved && Input.touches[0].phase == TouchPhase.Moved)
                    UpdateCameraDis();
#endif

#if UNITY_EDITOR_WIN
        if (Input.GetMouseButton(0))
        {
            UpdateCameraPos();
        }
        camera.transform.position += new Vector3(0.0f, 0.0f, 5.0f * Input.GetAxis("Mouse ScrollWheel"));
#endif
	}

    float newdis, olddis, distance, dmax, dmin, xmax, ymax;
    Vector3 pos;

    void UpdateCameraDis()
    {
        Vector3 s1 = Input.touches[0].position;
        Vector3 s2 = Input.touches[1].position;
        newdis = Vector2.Distance(s1, s2);

        // Normalize the center vector
        Vector2 cen = new Vector2((s1.x + s2.x) / 2, (s1.y + s2.y) / 2);
        float length = Mathf.Sqrt(cen.x * cen.x + cen.y * cen.y);
        cen = new Vector2(cen.x / length, cen.y / length);

        // Judge the move trend of fingers. Updage the distance
        if (newdis > olddis)
            distance += Time.deltaTime * 45.0f;
        else
            distance -= Time.deltaTime * 45.0f;
        if (distance > dmax) distance = dmax;
        if (distance < dmin) distance = dmin;
        olddis = newdis;
        pos = camera.transform.position;
        pos.x += 5.0f * cen.x * Time.deltaTime;
        pos.y += 5.0f * cen.y * Time.deltaTime;
        if (pos.x > xmax) pos.x = xmax;
        if (pos.x + xmax < 0) pos.x = 0 - xmax;
        if (pos.y > ymax) pos.y = ymax;
        if (pos.y + ymax < 0) pos.y = 0 - ymax;
        camera.transform.position = new Vector3(pos.x, pos.y, distance);
    }

    float s01, s02;
    void UpdateCameraPos()
    {
        // Actually we move the camera, therefore there is a "-"
        s01 = -Input.GetAxis("Mouse X");
        s02 = -Input.GetAxis("Mouse Y");
        pos = camera.transform.position;
        float speed = (0.75f + (pos.z - dmax) / dmin) * 5.0f;
        pos.x += speed * Time.deltaTime * s01; pos.y += speed * Time.deltaTime * s02;
        if (pos.x > xmax) pos.x = xmax;
        if (pos.x < -xmax) pos.x = -xmax;
        if (pos.y > ymax) pos.y = ymax;
        if (pos.y < -ymax) pos.y = -ymax;
        camera.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }
}
