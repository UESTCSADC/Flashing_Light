using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InitShader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Material MapMat = Resources.Load<Material>("Materials/NewMapMat");
        Shader Shader64 = Resources.Load<Shader>("Materials/MapShader32");
        MapMat.shader = Shader64;
        List<Vector4> list0 = new List<Vector4>(), list1 = new List<Vector4>();
        for (int i = 0; i < 64; i++)
        {
            Vector4 a = new Vector4(1, 1, 1, 1);
            list0.Add(a);
            list1.Add(a);
        }
        MapMat.SetVectorArray("_LightsPosW_End", list0);
        MapMat.SetVectorArray("_LightsColor_Start", list1);
    }
}
