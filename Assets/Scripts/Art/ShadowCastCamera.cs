using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCastCamera : MonoBehaviour {
    public Shader ShadowCaster;
    public ComputeShader CS;
    public GameObject LightSource;
    public Material MapMat;

    RenderTexture midRTT;
    RenderTexture Shadow;
    RenderTexture ShadowInform;

    Camera cameraC;

    // Use this for initialization
    void Start () {
        cameraC = this.GetComponent<Camera>();
        cameraC.SetReplacementShader(ShadowCaster, "RenderType");

        midRTT = new RenderTexture(640, 20, 0);
        midRTT.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
        midRTT.format = RenderTextureFormat.ARGB32;
        //midRTT.volumeDepth = 4;
        midRTT.name = "midRTT";
        midRTT.enableRandomWrite = true;
        midRTT.Create();

        Shadow = new RenderTexture(400, 300, 0);
        Shadow.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
        Shadow.name = "Shadow";
        Shadow.format = RenderTextureFormat.R8;
        Shadow.enableRandomWrite = true;
        Shadow.Create();
        //

        ShadowInform = new RenderTexture(400, 300, 0);
        ShadowInform.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
        ShadowInform.name = "ShadowInform";
        ShadowInform.format = RenderTextureFormat.ARGB32;
        ShadowInform.enableRandomWrite = true;
        ShadowInform.Create();
        cameraC.targetTexture = ShadowInform;
    }
	
	// Update is called once per frame
	void Update () {
        int kernelHandle = CS.FindKernel("ShadowInformCalc");
        //kh = kernelHandle;

        //获取光源参数
        Vector3 lightPosScreen = cameraC.WorldToViewportPoint(LightSource.transform.position);
        Vector4 light0 = new Vector4(lightPosScreen.x, lightPosScreen.y, LightSource.transform.position.z, 5.0f);
        Vector4[] light = { light0 };

        ////创建资源
        ComputeBuffer CB = new ComputeBuffer(1, 4 * sizeof(float));
        CB.SetData(light);

        //计算阴影信息
        CS.SetTexture(kernelHandle, "Ray_Dist_Height_Gradient_Hardness", midRTT);
        CS.SetTexture(kernelHandle, "Inform", ShadowInform);
        CS.SetBuffer(kernelHandle, "LightPos_HeightScale", CB);
        CS.Dispatch(kernelHandle, 80, 5, 1);

        //计算阴影
        kernelHandle = CS.FindKernel("ShadowCast");
        CS.SetTexture(kernelHandle, "Ray_Dist_Height_Gradient_Hardness", midRTT);
        CS.SetTexture(kernelHandle, "Inform", ShadowInform);
        CS.SetTexture(kernelHandle, "Shadow", Shadow);
        CS.SetBuffer(kernelHandle, "LightPos_HeightScale", CB);
        CS.Dispatch(kernelHandle, 50, 75, 1);

        //释放资源
        CB.Release();

        //取出输出纹理
        MapMat.SetTexture("_ShadowMap", Shadow);
    }

    //当该相机渲染完成后执行运算操作
    //private void OnRenderImage(RenderTexture source, RenderTexture destination)
    //{
       
    //}
}
