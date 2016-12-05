
using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
public class HueLight : MonoBehaviour
{

    public static GameObject[] huelights = new GameObject[30];
    public static LightKind[] lightkinds = new LightKind[30];
    public static LightParameter[] lightpara = new LightParameter[30];
    public static int size = 0;

    // Return length of array
    public int GetSize() { return size; }

    // Add GameObject Wall to array
    public void AddLight(GameObject cl, LightKind lk)
    {
        // If created, it cannot be cancelled when scene is changed
        // Meanwhile, set as static so that data won't change
        huelights[size] = cl;
        lightkinds[size] = lk;

        if (lightpara[size] == null) lightpara[size] = new LightParameter();
        lightpara[size].SetLight(cl);
        Object.DontDestroyOnLoad(huelights[size]);
        size += 1;
        if (size >= 30) Debug.Log("There are too many hue lights!");
    }

    public bool DeleteLight(ref GameObject huelight)
    {
        if (huelight == null) return true;
        int pos = 0; bool find = false;
        for (int i = 0; i < size; i++)
            if (huelights[i].Equals(huelight))
            {
                pos = i; find = true;
                break;
            }

        if (!find)
        {
            Debug.Log("Cannot find the targeted wall line!");
            return false;
        }

        huelights[pos] = huelights[size - 1];
        huelights[size - 1] = null;
        lightkinds[pos] = lightkinds[size - 1];

        lightpara[pos].copy(lightpara[size - 1]);
        //lightpara[pos] = lightpara[size - 1];
        lightpara[size - 1].SetLight(null);

        size -= 1;
        huelight.SetActive(false);
        return true;
    }

    public GameObject ReturnILight(int i)
    {
        if (i < size) return huelights[i];
        else return null;
    }

    public LightKind ReturnIKind(int i)
    {
        if (i < size) return lightkinds[i];
        else return LightKind.none;
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
    // Find the closest two-side points of huelights to point (depending on distance)
    public int FindClosestLight(Vector2 point, float distance)
    {
        float dis = Mathf.Abs(distance / 100) * 2.0f + 0.1f;
        GameObject returnlight = null;
        float min = 100.0f, d = 100.0f;
        int rank = 100;
        for (int i = 0; i < size; i++)
        {
            Vector2 pos = huelights[i].transform.position;
            d = Vector2.Distance(point, pos);
            if (d < min)
            {
                min = d;
                returnlight = huelights[i];
                rank = i;
            }
        }
        if (min <= dis) return rank;
        else return 100;

    }
    public void SetLightKind(GameObject cl, LightKind lk)
    {
        Vector3 pos = cl.transform.position;
        for (int i = 0; i < size; i++)
            if (huelights[i].Equals(cl))
            {
                if (lightkinds[i] == lk) return;
                DeleteLight(ref cl);
                if (lk == LightKind.bulb)
                {
                    cl = Instantiate(Resources.Load("bulb") as GameObject);
                    AddLight(cl, LightKind.bulb);
                }
                if (lk == LightKind.lamp)
                {
                    cl = Instantiate(Resources.Load("lamp") as GameObject);
                    AddLight(cl, LightKind.lamp);
                }
                if (lk == LightKind.pendant)
                {
                    cl = Instantiate(Resources.Load("pendant") as GameObject);
                    AddLight(cl, LightKind.pendant);
                }
                if (lk == LightKind.shade)
                {
                    cl = Instantiate(Resources.Load("shade") as GameObject);
                    AddLight(cl, LightKind.shade);
                }
                
                cl.transform.position = pos;

                break;
            }
    }

    public void SetHeight(GameObject cl, float height)
    {
        Vector3 pos = cl.transform.position;

        for (int i = 0; i < size; i++)
            if (huelights[i].Equals(cl))
            {
                cl.transform.position = new Vector3(cl.transform.position.x, cl.transform.position.y, height);
                break;
            }
    }

    public void SetRealSize(bool set)
    {
        if (!set)
        {
            for (int i = 0; i < size; i++)
                huelights[i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            for (int i = 0; i < size; i++) 
            {
                if (lightkinds[i] == LightKind.bulb)
                    huelights[i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * 0.3f;
                if (lightkinds[i] == LightKind.lamp)
                    huelights[i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * 0.9f;
                if (lightkinds[i] == LightKind.pendant)
                    huelights[i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * 1.0f;
                if (lightkinds[i] == LightKind.shade)
                    huelights[i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * 0.5f;
            }
        }
    }

    public bool GetIOpen(int i)
    {
        if (i >= size) return false;
        else return lightpara[i].open;
    }

    public float GetIIntensity(int i)
    {
        if (i >= size) return 0.0f;
        else return lightpara[i].intensity;
    }

    public bool GetISingle(int i)
    {
        if (i >= size) return false;
        else return lightpara[i].single;

    }

    public float GetIPace(int i)
    {
        if (i >= size) return 0.1f;
        else return lightpara[i].pace;
    }

    public void GetIColor(int i, ref Color c0, ref Color c1, ref Color c2, ref Color c3, ref Color c4, ref Color c5)
    {
        if (i >= size) return;
        else
        {
            c0 = lightpara[i].color[0];
            c1 = lightpara[i].color[1];
            c2 = lightpara[i].color[2];
            c3 = lightpara[i].color[3];
            c4 = lightpara[i].color[4];
            c5 = lightpara[i].color[5];
        }
    }

    public void SetIOpen(int i, bool o)
    {
        if (i >= size) return;
        else lightpara[i].open = o;
    }

    public void SetIIntensity(int i, float intensity)
    {
        if (i >= size) return;
        else lightpara[i].intensity = intensity;
    }

    public void SetISingle(int i, bool s)
    {
        if (i >= size) return;
        else lightpara[i].single = s;
    }

    public void SetIPace(int i, float p)
    {
        if (i >= size) return;
        else lightpara[i].pace = p;
    }

    public void SetIColor(int i, Color c0, Color c1, Color c2, Color c3, Color c4, Color c5)
    {
        if (i >= size) return;
        else
        {
            lightpara[i].color[0] = c0;
            lightpara[i].color[1] = c1;
            lightpara[i].color[2] = c2;
            lightpara[i].color[3] = c3;
            lightpara[i].color[4] = c4;
            lightpara[i].color[5] = c5;
            lightpara[i].SetHSV();
        }
    }

    public void UpdateColor(float time)
    {
        for (int i = 0; i < size; i++)
            lightpara[i].UpdateColor(time);
    }

    public void UploadData(float time)
    {
        for (int i = 0; i < size; i++)
            lightpara[i].SendData(time);
    }
}



public class LightParameter
{
    public GameObject light;
    public int hue;
    public bool open, single;    // open : whether the light is open;   single: the mode ->single or transition
    public float intensity, pace;   // the intensity of color currently you will choose;  pace: the change rate
    public Color[] color = new Color[6];
    public bool connect = false;
    public static int[] HueNum = new int[10];

    HSV[] hsv = new HSV[6];

    public void copy(LightParameter lp)
    {
        light = lp.light;
        hue = lp.hue;
        open = lp.open; single = lp.single;
        intensity = lp.intensity; pace = lp.pace;
        for (int i = 0; i < 6; i++)
        {
            color[i] = new Color(lp.color[i].r, lp.color[i].g, lp.color[i].b);
            hsv[i] = new HSV(lp.hsv[i].h, lp.hsv[i].s, lp.hsv[i].v);
        }
    }

    public void SetLight(GameObject l)
    {
        light = l;
        intensity = 1.0f;
        pace = 0.1f;
        for (int i = 0; i < 6; i++) 
            color[i] = new Color(1.0f, 1.0f, 1.0f);
        single = true;
        open = true;

        step = 0.0f;
        current = 1;
        past = 1;

        if (l == null)
        {
            HueNum[hue] = 0;
            hue = 9;
        }
        else
        {
            int k = 0;
            for (k = 0; k < 3; k++)
                if (HueNum[k] == 0)
                {
                    hue = k;
                    HueNum[k] = 1;
                    break;
                }
            if (k == 3)
            {
                hue = 9;
                Debug.Log("There are more than 3 Hue Lights!");
            }
        }
    }
    public void SetHSV()
    {
        for (int i = 0; i < 6; i++)
            hsv[i].RGBtoHSV(color[i].r, color[i].g, color[i].b);
    }

    // Pay attention: h: 0-360   s: 0.0-1.0   v:0.0-1.0
    public void SetColor(Color nc){
        light.GetComponentInChildren<Light>().color = nc;
    
    }

    int current, past;
    float step;     //linear interpolation
    Color nowcolor;
    public void UpdateColor(float time)
    {
        
        if (!open)
        {
            SetColor(new Color(0.0f, 0.0f, 0.0f)); 
            return;
        }
        
        if (single)
        {
            SetColor(color[0]);
            return;
        }

        step += time / (30 - 30*Mathf.Sqrt(pace) + 1);    //  pace: 0--1 is corresponding to 1s--60s
        if (step>1)
        {
            past = current;
            while (color[current] == new Color(1.0f, 1.0f, 1.0f) || current == past)
            {
                current += 1;
                if (current > 5) current = 1;
                if (current == past) break;
            }
            step = 0.0f;
        }
        Interpolate();
        SetColor(nowcolor);

    }

    public void Interpolate()
    {
        float h, s, v;
        h = (1 - step) * hsv[past].h + step * hsv[current].h;
        s = (1 - step) * hsv[past].s + step * hsv[current].s;
        v = (1 - step) * hsv[past].v + step * hsv[current].v;
        hsvTorgb(h, s, v, ref nowcolor);
    }

    void hsvTorgb(float H, float S, float V, ref Color color)
    {
        float R = 0.37f, G = 0.37f, B = 0.37f, f, a, b, c;
        if (S == 0.0f) R = G = B = V; else H /= 60;
        int i = (int)H;
        f = H - i;
        a = V * (1 - S);
        b = V * (1 - S * f);
        c = V * (1 - S * (1 - f));
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

    HttpWebRequest request;
    float timer;
    public void SendData(float time)
    {
        timer += time;
        if (timer < 0.3f)
        {
            timer += time;
            return;
        }
        else timer = 0.0f;

        int num = hue + 1;
        string url = "http://192.168.8.101/api/G8YjHCZ7YZlA0a9d0n3tj8UcQhCem-FaKKdGvJVO/lights/"+num+"/state";
        request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        connect = true;

        float fx = 0.0f, fy = 0.0f;
        nowcolor = light.GetComponentInChildren<Light>().color;
        toXY(nowcolor.r, nowcolor.g, nowcolor.b, ref fx, ref fy);

        string data = "{\"xy\":[" + fx + "," + fy + "]}";

        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.WriteLine(data);
        }

        WebResponse response = request.GetResponse();
        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {

        }

    }

    void toXY(float red, float green, float blue, ref float fx, ref float fy)
    {
    //Gamma correctie
                    red = (red > 0.04045f) ? Mathf.Pow((red + 0.055f) / (1.0f + 0.055f), 2.4f) : (red / 12.92f);
                    green = (green > 0.04045f) ? Mathf.Pow((green + 0.055f) / (1.0f + 0.055f), 2.4f) : (green / 12.92f);
                    blue = (blue > 0.04045f) ? Mathf.Pow((blue + 0.055f) / (1.0f + 0.055f), 2.4f) : (blue / 12.92f);
                    
                    //Apply wide gamut conversion D65
                    float X = red * 0.664511f + green * 0.154324f + blue * 0.162028f;
                    float Y = red * 0.283881f + green * 0.668433f + blue * 0.047685f;
                    float Z = red * 0.000088f + green * 0.072310f + blue * 0.986039f;
                    
                    fx = X / (X + Y + Z);
                    fy = Y / (X + Y + Z);
                    return ;
    }

}

public struct HSV
{
    public HSV(float gh, float gs, float gv)
    {
        h = gh; s = gs; v = gv;
    }
    public float h, s, v;
    public void RGBtoHSV(float R, float G, float B)
    {
        float max=ThreeMax(R,G,B);
        float min=ThreeMin(R,G,B);
        if (R==max) h = (G-B)/(max-min) ;
        if (G==max) h = 2 + (B-R)/(max-min);
        if (B==max) h = 4 + (R-G)/(max-min);
        h = h * 60;
        if (h<0) h+=360.0f;
        v=ThreeMax(R,G,B);
        s = (max - min) / max;
    }

    float ThreeMax(float a, float b, float c)
    {
        if (a > b && a > c) return a;
        else if (b > c) return b;
        else return c;
    }

    float ThreeMin(float a, float b, float c)
    {
        if (a < b && a < c) return a;
        else if (b < c) return b;
        else return c;
    }
}
