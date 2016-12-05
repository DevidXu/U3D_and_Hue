using UnityEngine;
using System.Collections;

public class Light_Set : MonoBehaviour {

	// Use this for initialization
	void Start () {
        camera = GameObject.Find("Main Camera");
        grid = GameObject.Find("Grid");
        paper = GameObject.Find("Paper");

        huelights = new HueLight();
        huelights.SetRealSize(false);
        
        canvas = GameObject.Find("Canvas");
        applybtn = GameObject.Find("ApplyBtn");
        canvas.SetActive(false);

        createnew = false;
        canvaschoose = false;
	}

    GameObject camera, grid, paper, canvas;
    GameObject applybtn;
    public HueLight huelights;
    GameObject huelight;

    public GameObject currentlight;
    public LightKind currenttype;

    public GameObject bulb, lamp, pendant, shade;

    bool createnew;
    public bool canvaschoose;
    Mode m;

	// Update is called once per frame
	void Update () {

        if (canvaschoose) return;

        m = camera.GetComponent<Variable>().mode;
        if (m != Mode.Light) return;
        bool buttonchoose = grid.GetComponent<Mesh_Grid>().buttonchoose;
        // Only if you click on the Hue button, you can create new Hue light
        bool createnew = grid.GetComponent<Mesh_Grid>().createnew;

#if UNITY_EDITOR_WIN
        if (!buttonchoose && Input.GetMouseButtonUp(0) && createnew) 
        {
            // Create new light (Set the parameters)
            grid.GetComponent<Mesh_Grid>().createnew = false;

            Vector3 pos = World_Position(Input.mousePosition);
            // Create the light according to the kind
            currentlight = CreateHue(LightKind.bulb);
            currentlight.transform.position = pos;
            currenttype = LightKind.bulb;    // Default kind is bulb and can be fixed in the panel
            huelights.AddLight(currentlight, LightKind.bulb);

            canvaschoose = true;
            canvas.SetActive(true);
            applybtn.GetComponent<Apply>().lk = LightKind.bulb;
            return;
        }
        
        // Choose some existing light
        if (!buttonchoose && Input.GetMouseButtonUp(0) && !createnew) 
        {
            Vector2 Point = World_Position(Input.mousePosition);
            int rank = huelights.FindClosestLight(Point, camera.transform.position.z);
            if (rank > 30) return;
            currentlight = huelights.ReturnILight(rank);
            currenttype = huelights.ReturnIKind(rank);

            canvaschoose = true;
            canvas.SetActive(true);
            applybtn.GetComponent<Apply>().lk = currenttype;
        }  
#endif

#if UNITY_ANDROID
        if (Input.touchCount != 0)
        {
            Vector2 realpos = new Vector2(Input.touches[0].position.x, Screen.height - Input.touches[0].position.y);
            Rect Huepos = NewPos(0.9f, 0.4f, 0.09f, 0.07f);
            if (Huepos.Contains(realpos)) return;

            if (!buttonchoose && Input.touches[0].phase == TouchPhase.Ended && createnew)
            {
                // Create new light (Set the parameters)
                grid.GetComponent<Mesh_Grid>().createnew = false;

                Vector3 pos = World_Position(Input.mousePosition);
                currentlight = CreateHue(LightKind.bulb);
                currenttype = LightKind.bulb;
                currentlight.transform.position = pos;
                huelights.AddLight(currentlight, LightKind.bulb);

                canvaschoose = true;
                canvas.SetActive(true);
                applybtn.GetComponent<Apply>().lk = LightKind.bulb;
                return;
            }

            // Choose some existing light
            if (!buttonchoose && Input.touches[0].phase == TouchPhase.Ended && !createnew)
            {
                Vector2 Point = World_Position(Input.touches[0].position);
                int rank = huelights.FindClosestLight(Point, camera.transform.position.z);
                if (rank > 30) return;
                currentlight = huelights.ReturnILight(rank);
                currenttype = huelights.ReturnIKind(rank);

                canvaschoose = true;
                canvas.SetActive(true);
                applybtn.GetComponent<Apply>().lk = currenttype;
            }
        }
#endif
    }


    Rect NewPos(float beginx, float beginy, float lx, float ly)
    {
        return new Rect(Screen.width * beginx, Screen.height * beginy, Screen.width * lx, Screen.height * ly);
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
        else
            print("No hit on the paper!");
        return Point;
    }

    GameObject hl;
    public GameObject CreateHue(LightKind lk)
    {

        if (lk == LightKind.bulb)
            hl = Instantiate(Resources.Load("bulb") as GameObject);
        if (lk == LightKind.lamp)
            hl = Instantiate(Resources.Load("lamp") as GameObject);
        if (lk == LightKind.pendant)
            hl = Instantiate(Resources.Load("pendant") as GameObject);
        if (lk == LightKind.shade)
            hl = Instantiate(Resources.Load("shade") as GameObject);
        return hl;
    }


}


