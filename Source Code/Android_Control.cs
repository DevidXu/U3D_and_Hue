using UnityEngine;
using System.Collections;

public class Android_Control : MonoBehaviour {

	// Use this for initialization
	void Start () {
        fpc = GameObject.Find("Person");
        myCC = this.GetComponent<CharacterController>();
        movepos = new Vector2(0.0f, 0.0f);
        speedunit = 1.0f;
        rotateunit = 2.0f;
        camera = this.GetComponent<Camera>();

        wdh = 1.0f * Screen.height / Screen.width;
        leftx = 0.7f; lefty = 0.6f;
        wheelwidth = 0.48f * wdh; wheelheight = 0.38f;
        wheelRect = NewPos(leftx, lefty, wheelwidth, wheelheight);
	}
    float wdh, leftx, lefty, wheelwidth, wheelheight;

    Camera camera;
    CharacterController myCC;
    GameObject fpc;
    Rect wheelRect;
    float speedunit, rotateunit;  // Control speed under different size

    public Vector2 movepos;
	// Update is called once per frame
	void FixedUpdate () {
        Vector2 pos = new Vector2(0.0f, 0.0f);
#if UNITY_EDITOR_WIN
        if (Input.GetMouseButton(0)) pos = Input.mousePosition;
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1) pos = Input.touches[0].position;
#endif

        Vector2 realpos = new Vector2(pos.x, Screen.height - pos.y);
        if (!wheelRect.Contains(realpos)) return;

        Vector2 movepos = new Vector2(pos.x - (leftx + wheelwidth / 2) * Screen.width,
                -Screen.height + pos.y + (lefty + wheelheight / 2) * Screen.height);


        // This is the angle of camera respect to the y-axis
        float yEulerAngle = fpc.transform.rotation.eulerAngles.y;
        // Position of person with different wheel angle
        Quaternion rotation = Quaternion.Euler(0, yEulerAngle, 0);

        float transformspeed = (1 - Mathf.Abs(movepos.x / movepos.magnitude)) * 0.2f * speedunit;
        //Use the move of rigidbody and avoid skipping collision
        Vector3 move = rotation * movepos * transformspeed;
        move = new Vector3(move.z, 0.0f, move.y);

        // Detect whether collision using ray cast
        Vector3 oldpos = transform.position;
        transform.Translate(move*Time.deltaTime);

        if (DetectCollision())
        {
            transform.position = oldpos*2-transform.position;
            TriggerRotate();
        }


        // Transform the angle of the person camera
        float rotationspeed = (1 - Mathf.Abs(movepos.y / movepos.magnitude)) * 1.5f * rotateunit;
        if (movepos.x < 0) rotationspeed *= -1;
        fpc.transform.Rotate(Vector3.up * rotationspeed);
	}

    bool DetectCollision()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit, 0.5f))
            return true;
        else return false;
    }

    // This is very important in case that the person seized up into the wall
    void OnTriggerEnter(Collider e)
    {
        transform.Rotate(Vector3.up, 30.0f*Time.deltaTime);
    }

    void TriggerRotate()
    {
        transform.Rotate(Vector3.down, 15.0f * Time.deltaTime);
    }


    public GUISkin wc;
    bool move;
    void OnGUI()
    {
        GUI.skin = wc;
        GUI.Button(wheelRect, "Go");
    }

    Rect NewPos(float beginx, float beginy, float lx, float ly)
    {
        return new Rect(Screen.width * beginx, Screen.height * beginy, Screen.width * lx, Screen.height * ly);
    }
}
