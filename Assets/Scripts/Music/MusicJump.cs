using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace FlashingLight { 
public class MusicJump : MonoBehaviour {
    private Material MAT;
    public LineRenderer lr;
    TrailRenderer Tr;
    public float radius = 0.15f;
    bool istrue = true;
	// Use this for initialization
	void Start () {
        MAT = LoadSprite.Instance.MusicMat;

        lr = GetComponent<LineRenderer>();
        lr.material = MAT;
        lr.startColor = Color.red;
        lr.endColor = Color.blue;
        lr.numCornerVertices = 5;
        lr.positionCount=255;
        lr.startWidth = 0.015f;
        lr.endWidth = 0.015f;
            lr.sortingOrder = (int)RenderLayer.player;
        lr.useWorldSpace = false;
        Gradient a = new Gradient();
        GradientColorKey[] b = new GradientColorKey[3];
        b[0] = new GradientColorKey(Color.red, 0);
        b[1] = new GradientColorKey(Color.blue, 0.5f);
        b[2] = new GradientColorKey(Color.green, 1);
        a.colorKeys = b;
        lr.colorGradient = a;
    }
	
	// Update is called once per frame
	void Update () {
        //transform.position = new Vector2(transform.position.x, transform.position.y);
        float[] spectrum = new float[256];

        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);
        if (istrue == true)
        {
            for (int i = 0; i < 128 - 1; i++)//
            {
                //Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
                //Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
                //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
                //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
                lr.SetPosition(i * 2 + 1, new Vector3(Mathf.Cos((i * 2 + 1) * 2 * Mathf.PI / 253) * radius, Mathf.Sin((i * 2 + 1) * 2 * Mathf.PI / 253) * radius, 0));
                float Deat = Mathf.Log(spectrum[i+127]) * 0.051f + 0.4f;
                if (Deat < -0.3f)
                    Deat = -0.3f;
                //Deat = 0;
                lr.SetPosition(i * 2, new Vector3(Mathf.Cos(i * 2 * Mathf.PI * 2 / 253) * (radius - Deat), Mathf.Sin(i * 2 * Mathf.PI * 2 / 253) * (radius - Deat), 0));
                // 
            }
                //for (int i = 0; i < spectrum.Length- 1; i++)//
                //{
                //    //Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
                //    //Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
                //    //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
                //    //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
                //   // lr.SetPosition(i * 2 + 1, new Vector3(Mathf.Cos((i * 2 + 1) * 2 * Mathf.PI / 253) * radius, Mathf.Sin((i * 2 + 1) * 2 * Mathf.PI / 253) * radius, 0));
                //    float Deat = Mathf.Log(spectrum[i]) * 0.051f + 0.3f;
                //    if (Deat < -0.5f)
                //        Deat = -0.5f;
                //    //Deat = 0;
                //    lr.SetPosition(i, new Vector3(Mathf.Cos(i * Mathf.PI * 2 / 253) * (radius - Deat), Mathf.Sin(i * Mathf.PI * 2 / 253) * (radius - Deat), 0));
                //    // 
                //}
                lr.SetPosition(254, new Vector3(Mathf.Cos(0) * (radius - Mathf.Log(spectrum[0]) * 0.051f - 0.3f), Mathf.Sin(0) * (radius - Mathf.Log(spectrum[0]) * 0.051f - 0.3f), 0));
            // Color col = new Color(spectrum[0], spectrum[1], spectrum[2])*5;
            //Debug.Log(spectrum[0]);
            // MAT.color = col;
            istrue = false;
        }
        else
            istrue = true;
    }
}
}