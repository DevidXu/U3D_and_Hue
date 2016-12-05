using UnityEngine;
using System.Collections;

public class Draw_Line : MonoBehaviour {


	// Use this for initialization
	void Start () {
        // Initialize the gameobject we will use
        camera = GameObject.Find("Main Camera");
        grid = GameObject.Find("Grid");
        paper = GameObject.Find("Paper");

        wallline = new WallLine();
        wallline.SetAllHeight(1.0f);
        wallline.SetAllMaterial(yellowwall);

        m = Mode.View;

        // Arrange the position of three button
        float hdw = (float)Screen.height / Screen.width;
        pencilpos = NewPos(0.04f, 0.04f, 0.12f * hdw, 0.12f);
        rubberpos = NewPos(0.04f, 0.18f, 0.12f * hdw, 0.12f);
        lightpos = NewPos(0.04f, 0.32f, 0.12f * hdw, 0.12f);
	}

    public GameObject wall, camera, grid, paper, data;
    public Material yellowwall;
    public WallLine wallline;
    GameObject line;
    Mode m;
    Rect pencilpos, rubberpos, lightpos;

	// Update is called once per frame
	void Update () {
        JudgeMode();
        m = camera.GetComponent<Variable>().mode;
        // Here judge on the touch to convert to the mode
        if (grid.GetComponent<Mesh_Grid>().buttonchoose) buttonchoose = true;

        if (m == Mode.Draw) Draw_Paper();
        if (m == Mode.Rubber) Rubber_Paper();
    }

    public Texture2D pencil_selected, pencil_unselected;
    public Texture2D rubber_selected, rubber_unselected;
    public Texture2D light_selected, light_unselected;
    void OnGUI()
    {

        if (camera.GetComponent<Variable>().mode != Mode.Draw)
            GUI.DrawTexture(pencilpos, pencil_unselected);
        else
            GUI.DrawTexture(pencilpos, pencil_selected);

        if (camera.GetComponent<Variable>().mode!=Mode.Rubber)
            GUI.DrawTexture(rubberpos, rubber_unselected);
        else
            GUI.DrawTexture(rubberpos, rubber_selected);

        if (camera.GetComponent<Variable>().mode != Mode.Light)
            GUI.DrawTexture(lightpos, light_unselected);
        else
            GUI.DrawTexture(lightpos, light_selected);
    }

    Rect NewPos(float beginx, float beginy, float lx, float ly)
    {
        return new Rect(Screen.width * beginx, Screen.height * beginy, Screen.width * lx, Screen.height * ly);
    }
    public bool buttonchoose = false;
    Vector2 realpos;
    void JudgeMode()
    {
        buttonchoose = false;
#if UNITY_EDITOR_WIN
        realpos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        if (pencilpos.Contains(realpos))
        {
            buttonchoose = true;
            if (Input.GetMouseButtonUp(0))
            {
                if (m != Mode.Draw)
                    camera.GetComponent<Variable>().mode = Mode.Draw;
                else
                    camera.GetComponent<Variable>().mode = Mode.View;
            }
        }
        if (rubberpos.Contains(realpos))
        {
            buttonchoose = true;
            if (Input.GetMouseButtonUp(0))
            {
                if (m != Mode.Rubber)
                    camera.GetComponent<Variable>().mode = Mode.Rubber;
                else
                    camera.GetComponent<Variable>().mode = Mode.View;
            }
        }
        if (lightpos.Contains(realpos))
        {
            buttonchoose = true;
            if (Input.GetMouseButtonUp(0))
            {
                if (m != Mode.Light)
                    camera.GetComponent<Variable>().mode = Mode.Light;
                else
                    camera.GetComponent<Variable>().mode = Mode.View;
            }
        }
#endif

        // An important difference between windows and android
       // when mousebutton is ended, GetMouseButton(0) is false; However, Input.touchCount = 1
#if UNITY_ANDROID
        if (Input.touchCount == 1) 
        {
            realpos = new Vector2(Input.touches[0].position.x, Screen.height - Input.touches[0].position.y);
            if (pencilpos.Contains(realpos))
            {
                buttonchoose = true;
                if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    if (m != Mode.Draw)
                        camera.GetComponent<Variable>().mode = Mode.Draw;
                    else
                        camera.GetComponent<Variable>().mode = Mode.View;
                }
            }
            if (rubberpos.Contains(realpos))
            {
                buttonchoose = true;
                if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    if (m != Mode.Rubber)
                        camera.GetComponent<Variable>().mode = Mode.Rubber;
                    else
                        camera.GetComponent<Variable>().mode = Mode.View;
                }
            }
            if (lightpos.Contains(realpos))
            {
                buttonchoose = true;
                if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    if (m != Mode.Light)
                        camera.GetComponent<Variable>().mode = Mode.Light;
                    else
                        camera.GetComponent<Variable>().mode = Mode.View;
                }
            }
        }
#endif
    }



    // Work under draw mode
    float x, y,ox,oy;
    Vector2 start, ostart;
    // x, y: the screen length of the line;  
    // start: in the world cordinate      ostart: in the screen cordinate
    // Find the position on the screen (from screen cordinate to world cordinate)
    // Pay attention : this requires the grid cordinate to be 0(since mouseposition.z = 0)
    void Draw_Paper()
    {
        if (buttonchoose) return;
#if UNITY_EDITOR_WIN

        // Whether create new line
        //if (Input.touches[0].phase == TouchPhase.Began)
        if (Input.GetMouseButtonDown(0))
        {
            line = Instantiate(wall);
            // Find the position on the screen (from screen cordinate to world cordinate)
            Vector2 Point = World_Position(Input.mousePosition);
            Vector2 virtualPoint = wallline.FindClosestPoint(Point, camera.transform.position.z);
            ostart = Input.mousePosition;
            start = virtualPoint;
            x = y = 0;
        }

        // When finger moving on the screen
        //if (new_line_drawing && Input.touches[0].phase == TouchPhase.Moved)
        if (Input.GetMouseButton(0))
        {
            x += Input.GetAxis("Mouse X");
            y += Input.GetAxis("Mouse Y");
            wallline.FixLine(line, start, ostart, x, y, camera.transform.position.z);
        }
      
        //if (new_line_drawing && Input.touches[0].phase == TouchPhase.Ended) 
        if (Input.GetMouseButtonUp(0)) 
        {
            if (Mathf.Abs(x) < 0.01f || Mathf.Abs(y) < 0.01f)
            {
                line = null;
                return;
            }
            wallline.AddWall(line);
            line = null;
        }
#endif

#if UNITY_ANDROID
        // Whether create new line
        //if (Input.touches[0].phase == TouchPhase.Began)
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 p = Input.GetTouch(0).position;
            Vector2 Point = World_Position(p);
            line = Instantiate(wall);
            // Find the position on the screen (from screen cordinate to world cordinate)
            Vector2 virtualPoint = wallline.FindClosestPoint(Point, camera.transform.position.z);
            ostart = p;
            start = virtualPoint;
            x = y = 0;
            wallline.FixLine(line, start, ostart, x, y, camera.transform.position.z);  // Since the input system in android will not go into next "if"
        }

        // When finger moving on the screen
        //if (new_line_drawing && Input.touches[0].phase == TouchPhase.Moved)
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            x += Input.GetAxis("Mouse X");
            y += Input.GetAxis("Mouse Y");
            wallline.FixLine(line, start, ostart, x, y, camera.transform.position.z);
        }

        //if (new_line_drawing && Input.touches[0].phase == TouchPhase.Ended) 
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (Mathf.Abs(x) < 0.01f || Mathf.Abs(y) < 0.01f)
            {
                line = null;
                return;
            }
            wallline.AddWall(line);
            line = null;
        }
#endif
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

    void Rubber_Paper()
    {
        if (buttonchoose) return;
        Vector2 Point = new Vector2(0.0f, 0.0f);
        GameObject dline = null;
#if UNITY_EDITOR_WIN
        if (!Input.GetMouseButtonUp(0)) return;
        Point = World_Position(Input.mousePosition);
        dline = wallline.FindClosestLine(Point, camera.transform.position.z);
        wallline.DeleteWall(ref dline);
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 0) return;
        Point = World_Position(Input.touches[0].position);
        dline = wallline.FindClosestLine(Point, camera.transform.position.z);
        wallline.DeleteWall(ref dline);
#endif
    }

}


// Create a class for easy process on the line creation or deletion
// Method: GetSize, AddWall, DeleteWall
public class WallLine:MonoBehaviour{
    // Initialize the line array
    public static GameObject[] lines = new GameObject[100];
    
    public static int size = 0;

    // Return length of array
    public int GetSize() { return size; }

    // Add GameObject Wall to array
    public void AddWall(GameObject wall)
    {
        // If created, it cannot be cancelled when scene is changed
        lines[size] = wall;
        Object.DontDestroyOnLoad(lines[size]);
        
        lines[size].SetActive(true);
        size += 1;
        if (size >= 100) Debug.Log("There are too many wall lines!");
    }

    // Delete GameObject wall if it exists
    public bool DeleteWall(ref GameObject wall)
    {
        if (wall == null) return true;
        int pos = 0; bool find = false;
        for (int i = 0; i < size; i++)
            if (lines[i].Equals(wall))
            {
                pos = i; find = true;
                break;
            }

        if (!find)
        {
            Debug.Log("Cannot find the targeted wall line!");
            return false;
        }

        lines[pos] = lines[size - 1];
        lines[size - 1] = null;
        size -= 1;
        wall.SetActive(false);
        return true;
    }

    // Fix the line length when the mouse is moving 
    public void FixLine(GameObject line, Vector2 start, Vector2 ostart, float x, float y, float distance)
    {
        Vector2 Point = new Vector2(0.0f, 0.0f);
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            Point.x = ostart.x + x; Point.y = ostart.y;
            Point = World_Position(Point);
            Vector2 virtualpoint = FindClosestPoint(Point, distance);
            if (virtualpoint == Point)
            {
                GameObject nearline = FindClosestLine(Point, distance);
                if (nearline != null)
                {
                    if (nearline.transform.localScale.x == 0.1f)
                        virtualpoint = new Vector2(nearline.transform.position.x, Point.y);
                    if (nearline.transform.localScale.y == 0.1f)
                        virtualpoint = new Vector2(Point.x, nearline.transform.position.y);
                    nearline = null;
                }
            }
            line.transform.position = new Vector3((virtualpoint.x + start.x) / 2, start.y, 0.0f);
            line.transform.localScale = new Vector3(Mathf.Abs(virtualpoint.x - start.x), 0.1f, 1.0f);
        }
        else
        {
            Point.x = ostart.x; Point.y = ostart.y + y;
            Point = World_Position(Point);
            Vector2 virtualpoint = FindClosestPoint(Point, distance);
            if (virtualpoint == Point)
            {
                GameObject nearline = FindClosestLine(Point, distance);
                if (nearline != null)
                {
                    if (nearline.transform.localScale.x == 0.1f)
                        virtualpoint = new Vector2(nearline.transform.position.x, Point.y);
                    if (nearline.transform.localScale.y == 0.1f)
                        virtualpoint = new Vector2(Point.x, nearline.transform.position.y);
                    nearline = null;
                }
            }
            line.transform.position = new Vector3(start.x, (virtualpoint.y + start.y) / 2, 0.0f);
            line.transform.localScale = new Vector3(0.1f, Mathf.Abs(virtualpoint.y - start.y), 1.0f);
        }
    }

    // Use point_to_screen ray to find world position of 
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

    // Below is used for finding closest points under touch
    
    // point is the position in the world, distance is the camera position
    // Find the closest two-side points of wall-lines to point (depending on distance)
    public Vector2 FindClosestPoint(Vector2 point, float distance)
    {
        float dis = Mathf.Abs(distance / 100) * 2.0f + 0.1f;
        Vector2 returnpoint = new Vector2(100.0f, 100.0f);
        Vector2 point1 = new Vector2(0.0f, 0.0f);
        Vector2 point2 = new Vector2(0.0f, 0.0f);
        float min = 100.0f, d = 100.0f;
        for (int i = 0; i < size; i++)
        {
            Vector3 pos = lines[i].transform.position;
            Vector3 scale = lines[i].transform.localScale;
            if (scale.x == 0.1f)
            {
                point1 = new Vector2(pos.x, pos.y + scale.y/2);
                point2 = new Vector2(pos.x, pos.y - scale.y/2);
            }
            if (scale.y == 0.1f) 
            {
                point1 = new Vector2(pos.x + scale.x/2, pos.y);
                point2 = new Vector2(pos.x - scale.x/2, pos.y);
            }

            d = Vector2.Distance(point, point1);
            if (d < min) 
            {
                min = d;
                returnpoint = point1;
            }

            d = Vector2.Distance(point, point2);
            if (d < min)
            {
                min = d;
                returnpoint = point2;
            }
        }
        if (min <= dis) return returnpoint;
        else return point;
        
    }

    // point is the position in the world, distance is the camera position
    // Find the closest wall-line to point (depending on distance). If no found, return null
    public GameObject FindClosestLine(Vector2 point, float distance)
    {
        float min = 100.0f, d = 100.0f ;
        GameObject reo = null;
        float dis = Mathf.Abs(distance / 100) * 2.0f + 0.1f;
        for (int i = 0; i < size; i++)
        {
            d = 100.0f;
            Vector3 pos = lines[i].transform.position;
            Vector3 scale = lines[i].transform.localScale;
            if (scale.x == 0.1f && point.y < pos.y + scale.y / 2 && point.y > pos.y - scale.y / 2)
                d = Mathf.Abs(point.x - pos.x);
            if (scale.y == 0.1f && point.x < pos.x + scale.x / 2 && point.x > pos.x - scale.x / 2)
                d = Mathf.Abs(point.y - pos.y);

            if (d < min)
            {
                min = d;
                reo = lines[i];
            }

        }
        if (min < dis) return reo; else return null;
    }

    public void SetAllHeight(float h)
    {
        for (int i = 0; i < size; i++)
        {
            Vector3 ls = lines[i].transform.localScale;
            lines[i].transform.localScale = new Vector3(ls.x, ls.y, h);
        }

        return;
    }

    public void SetAllMaterial(Material wallt)
    {
        for (int i = 0; i < size; i++)
        {
            lines[i].GetComponent<Renderer>().material = wallt;
        }

        return;
    }
}
