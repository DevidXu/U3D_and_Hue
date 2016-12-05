using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Para_Apply : MonoBehaviour {


	// Use this for initialization
	void Start () {
        ground = GameObject.Find("Ground");
        canvas = GameObject.Find("Canvas");
        lightswitch = GameObject.Find("Switch");
        isopen = false;
        choosecolor = false;
        open = false;
        single = false;

        for (int i=0;i<6;i++)
        {
            string s = "board" + i;
            board[i] = GameObject.Find(s);
            colors[i] = Color.white;
        }
	}

    public bool choosecolor, finishchoose;

    GameObject ground, canvas, lightswitch;
    GameObject[] board = new GameObject[6];  // This is the color choose button
    public bool isopen, open, single;
    public float intensity, pace;
    public int currentcolor;  // the array position in the panel to give color to
    public Color[] colors = new Color[6];

	// Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR_WIN
        if (choosecolor && Input.GetMouseButtonDown(0))
        {
            ResignColor(Input.mousePosition);
            choosecolor = false;
        }
#endif

#if UNITY_ANDROID
        if (choosecolor && Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            ResignColor(Input.touches[0].position);
            choosecolor = false;
        }
#endif

        int rank = ground.GetComponent<Light_Parameter>().rank;
        if (!isopen)
        {
            open = ground.GetComponent<Light_Parameter>().huelights.GetIOpen(rank);
            intensity = ground.GetComponent<Light_Parameter>().huelights.GetIIntensity(rank);
            single = ground.GetComponent<Light_Parameter>().huelights.GetISingle(rank);
            pace = ground.GetComponent<Light_Parameter>().huelights.GetIPace(rank);
            ground.GetComponent<Light_Parameter>().huelights.GetIColor(rank, ref colors[0], ref colors[1], ref colors[2], ref colors[3], ref colors[4], ref colors[5]);
            for (int k = 0; k < 6; k++)
                board[k].GetComponent<Image>().color = colors[k];
        }
        isopen = this.isActiveAndEnabled;
    }


    public void OnClick()
    {
        if (choosecolor) return;

        int rank = ground.GetComponent<Light_Parameter>().rank;
        ground.GetComponent<Light_Parameter>().huelights.SetIOpen(rank, open);
        ground.GetComponent<Light_Parameter>().huelights.SetIIntensity(rank, intensity);
        ground.GetComponent<Light_Parameter>().huelights.SetISingle(rank, single);
        ground.GetComponent<Light_Parameter>().huelights.SetIPace(rank, pace);
        ground.GetComponent<Light_Parameter>().huelights.SetIColor(rank, colors[0], colors[1], colors[2], colors[3], colors[4], colors[5]);

        ground.GetComponent<Light_Parameter>().rank = 100;
        isopen = false;
        canvas.SetActive(false);

    }

    public GUIContent colorcircle;
    void OnGUI()
    {
        if (choosecolor)
            GUI.Button(NewPos(0.0f, 0.0f, 1.0f, 1.0f), colorcircle);
    }

    Rect NewPos(float beginx, float beginy, float lx, float ly)
    {
        return new Rect(Screen.width * beginx, Screen.height * beginy, Screen.width * lx, Screen.height * ly);
    }

    // Give the color to the current select mode
    void ResignColor(Vector2 pos)
    {
        Color color = new Color(1.0f, 1.0f, 1.0f);
        Vector2 colorpos = new Vector2(pos.x-Screen.width/2,pos.y-Screen.height/2);
        float h = Mathf.Acos(colorpos.x / colorpos.magnitude);
        if (colorpos.y < 0) h = 2.0f * 3.14f - h;
        h = h / 3.14f * 180f;
        colorpos = new Vector2(colorpos.x / (Screen.width * 0.25f), colorpos.y / (Screen.height * 0.44f));
        float s = colorpos.magnitude;
        float v = intensity;
        if (s<=1.0f)
            hsvTorgb(h, s, v, ref color);
        board[currentcolor].GetComponent<Image>().color = color;
        colors[currentcolor] = color;
    }

    void hsvTorgb(float H, float S, float V, ref Color color)
    {
        float R = 0.37f, G = 0.37f, B = 0.37f, f, a, b, c;
         if (S==0.0f)   R=G=B=V; else H /= 60; 
        int i=(int)H;
        f = H - i;
        a = V * ( 1 - S );
        b = V * ( 1 - S * f );
        c = V * ( 1 - S * (1 - f ) );
        switch (i)
        {
            case 0: R = V; G = c; B = a; break;
            case 1: R = b; G = V; B = a; break;
            case 2: R = a; G = V; B = c; break;
            case 3: R = a; G = b; B = V; break;
            case 4: R = c; G = a; B = V; break;
            case 5: R = V; G = a; B = b; break;
            default: break;
        }
        color = new Color(R, G, B);
}
}
