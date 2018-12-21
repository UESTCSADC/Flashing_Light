using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
namespace FlashingLight
{
    public class LightManager : MonoBehaviour
    {
        private static LightManager instance;

        private struct LightSource //光源的基本结构
        {
            public GameObject lightGO;  //光源的实例化对象
            public Vector3 Pos;  //如果是没有实例的光源，则使用Pos作为位置信息
            public Color Color;  //发射光的颜色
            public Vector2 Range; //光线的辐照范围/衰减（从.x开始线性衰减，到.y处为0）
            public float LifeTime; //光源的生命周期，用于绘制Spark效果
            public uint RoomNum; //光源所在房间的编号

            public float flashCount; //用于处理闪烁效果
        };

        public struct LightSourceLine
        {
            public Vector4 StartPos;
            public Vector4 EndPos;
            public Color c;
            public GameObject go;
        }; //线光源的基本结构

        private Camera ShadowCam; // 用于获取渲染阴影的子相机

        private Shader ShadowCaster; //阴影绘制Shader

        private ComputeShader ShadowCS; //阴影计算ComputerShader

        private Material MapMat; //地图材质

        private RenderTexture midRTT; //中间变量
        private RenderTexture Shadow; //阴影纹理
        private RenderTexture ShadowInform; //子相机输出的阴影信息

        private List<LightSource> MainLightSource; //会产生阴影的主要光源 -- 主角默认为0号资源
        private List<LightSource> MiniLightSource; //不会产生阴影的小型光源
        private List<LightSource> Spark; //受到生命周期影响的火花
        private List<LightSourceLine> LineLight; //线光源数组

        private float HeightScale;
        private float NormaScale;
        private float AlbedoScale;
        private float TargetAlbedo;

        public static LightManager Instance
        {
            get
            {
                return instance;
            }

            set
            {
                instance = value;
            }
        }

        // Use this for initialization
        void Start()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
            //设置帧率上限
            //Application.targetFrameRate = 60;

            

            //初始化List
            MainLightSource = new List<LightSource>();
            MiniLightSource = new List<LightSource>();
            Spark = new List<LightSource>();
            LineLight = new List<LightSourceLine>();

            //初始化主角,等别人来分
            SetCharacter(null, Color.black, Vector2.zero, 0);

            //初始化环境系数
            SetCurrent(5.0f, 1.0f);

            //配置各类组件
            ShadowCaster = Resources.Load<Shader>("Materials/ShadowCastShader");
            ShadowCS = Resources.Load<ComputeShader>("Materials/ShaderCasterCS");
            MapMat = Resources.Load<Material>("Materials/NewMapMat");

            //初始化Shader数组
            List<Vector4> list0 = new List<Vector4>();
            for (int i = 0; i < 64; i++)
            {
                Vector4 a = new Vector4(1, 1, 1, 1);
                list0.Add(a);
            }
            MapMat.SetVectorArray("_LightsPosW_End", list0);
            MapMat.SetVectorArray("_LightsColor_Start", list0);
            MapMat.SetVectorArray("_LineStart", list0);
            MapMat.SetVectorArray("_LineEnd", list0);
            MapMat.SetVectorArray("_LineColor", list0);



            //初始化渲染用的各种纹理
            midRTT = new RenderTexture(1280, 20, 0);
            midRTT.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
            midRTT.format = RenderTextureFormat.ARGB32;
            midRTT.volumeDepth = 8;
            midRTT.name = "midRTT";
            midRTT.enableRandomWrite = true;
            midRTT.filterMode = FilterMode.Trilinear;
            midRTT.Create();

            Shadow = new RenderTexture(400, 300, 0);
            Shadow.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
            Shadow.volumeDepth = 8;
            Shadow.name = "Shadow";
            Shadow.format = RenderTextureFormat.R8;
            Shadow.enableRandomWrite = true;
            Shadow.filterMode = FilterMode.Trilinear;
            Shadow.Create();

            ShadowInform = new RenderTexture(400, 300, 0);
            ShadowInform.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
            ShadowInform.name = "ShadowInform";
            ShadowInform.format = RenderTextureFormat.ARGB32;
            ShadowInform.enableRandomWrite = true;
            ShadowInform.filterMode = FilterMode.Trilinear;
            ShadowInform.Create();

            //配置子相机
            GameObject camGO = GameObject.Find("ShadowCamera");
            ShadowCam = camGO.GetComponent<Camera>();
            ShadowCam.SetReplacementShader(ShadowCaster, "RenderType");
            ShadowCam.targetTexture = ShadowInform;
        }

        // Update is called once per frame
        void Update()
        {
            //处理Flesh
            DealFlesh();
            //处理边界
            InLightChange();
            //阴影计算
            ShadowCalc();
            //提交光照信息
            Illumination();
            IlluminationLine();
 
            //Trace.Assert(NumLightSourceRoom() < 9);

        }

        // 计算阴影的函数
        private void ShadowCalc()
        {
            //配置计算资源
            int kernelHandle1 = ShadowCS.FindKernel("ShadowInformCalc");
            ComputeBuffer CB = new ComputeBuffer(8, 4 * sizeof(float));
            ShadowCS.SetTexture(kernelHandle1, "Ray_Dist_Height_Gradient_Hardness", midRTT);
            ShadowCS.SetTexture(kernelHandle1, "Inform", ShadowInform);
            ShadowCS.SetBuffer(kernelHandle1, "LightPos_HeightScale", CB);

            int kernelHandle2 = ShadowCS.FindKernel("ShadowCast");
            ShadowCS.SetTexture(kernelHandle2, "Ray_Dist_Height_Gradient_Hardness", midRTT);
            ShadowCS.SetTexture(kernelHandle2, "Inform", ShadowInform);
            ShadowCS.SetTexture(kernelHandle2, "Shadow", Shadow);
            ShadowCS.SetBuffer(kernelHandle2, "LightPos_HeightScale", CB);

            int kernelHandle3 = ShadowCS.FindKernel("ShadowClear");
            ShadowCS.SetTexture(kernelHandle3, "Shadow", Shadow);
            ShadowCS.Dispatch(kernelHandle3, 50, 75, 8);

            int kernelHandle4 = ShadowCS.FindKernel("ShadowGauss");
            ShadowCS.SetTexture(kernelHandle4, "Shadow", Shadow);

            float HeightScale = MapMat.GetFloat("_HeightScale");

            Vector4[] lights = new Vector4[MainLightSource.Count];
            //首先输入相机位置
            for (int i = 0; i < MainLightSource.Count; i++)
            {
                //获取光源参数
                LightSource Ls = MainLightSource[i];
                if (Ls.RoomNum == MainLightSource[0].RoomNum) // 只有和主角在一个房间内的光源会被渲染
                {
                    Vector4 lightpos = new Vector4(0, 0, 0, HeightScale);
                    if (Ls.lightGO != null)
                    {
                        Vector4 posCam = ShadowCam.WorldToViewportPoint(Ls.lightGO.transform.position);
                        lightpos = new Vector4(posCam.x, posCam.y, Ls.lightGO.transform.position.z / HeightScale, HeightScale);
                    }
                    else
                    {
                        Vector4 posCam = ShadowCam.WorldToViewportPoint(Ls.Pos);
                        lightpos = new Vector4(posCam.x, posCam.y, Ls.Pos.z / HeightScale, HeightScale);

                    }
                    lights[i] = lightpos;
                }
            }
            //设置buffer
            CB.SetData(lights);

            //计算阴影信息
            ShadowCS.Dispatch(kernelHandle1, 160, 5, 8);

            //计算阴影
            ShadowCS.Dispatch(kernelHandle2, 50, 75, 8);

            //高斯模糊
            ShadowCS.Dispatch(kernelHandle4, 50, 75, 8);

            //释放资源
            CB.Release();
            //取出输出纹理

            MapMat.SetTexture("_ShadowMap", Shadow);
        }

        //将照明参数整理并提交到shader的函数
        private void Illumination()
        {
            //依次获取照明信息
            List<Vector4> lightsPos = new List<Vector4>();
            List<Vector4> lightsColor = new List<Vector4>();

            //输入主光源
            for (int i = 0; i < MainLightSource.Count; i++)
            {
                if (lightsPos.Count == 64)  // 渲染的光源数目应该小于64
                    break;

                LightSource LS = MainLightSource[i];
                if (LS.RoomNum == MainLightSource[0].RoomNum)  //只有和主角在同一个房间的主要光源会被渲染
                {
                    Vector4 lightpos_end;
                    Vector4 lightcolor_start;

                    float flash = 1.0f - Mathf.Abs(LS.flashCount - 1.0f);

                    
                    if (InLightRange() == false)
                    {
                        flash = flash * (Mathf.Sin(Time.time * 2.0f) + 2) / 3.0f;
                    }

                   if (LS.lightGO != null)
                        lightpos_end = new Vector4(LS.lightGO.transform.position.x, LS.lightGO.transform.position.y, LS.lightGO.transform.position.z, LS.Range.y);
                    else
                        lightpos_end = new Vector4(LS.Pos.x, LS.Pos.y, LS.Pos.z, LS.Range.y);

                    lightcolor_start = new Vector4(LS.Color.r * flash, LS.Color.g * flash, LS.Color.b * flash, LS.Range.x);

                    lightsPos.Add(lightpos_end);
                    lightsColor.Add(lightcolor_start);

                }
            }

            //输入次要光源
            for (int i = 0; i < MiniLightSource.Count; i++)
            {
                if (lightsPos.Count == 64)  // 渲染的光源数目应该小于64
                    break;

                LightSource LS = MiniLightSource[i];
                if (LS.RoomNum == MiniLightSource[0].RoomNum)  //只有和主角在同一个房间的主要光源会被渲染
                {
                    Vector4 lightpos_end;
                    Vector4 lightcolor_start;
                    float flash = 1 - Mathf.Abs(LS.flashCount - 1);

                    if (LS.lightGO != null)
                        lightpos_end = new Vector4(LS.lightGO.transform.position.x, LS.lightGO.transform.position.y, LS.lightGO.transform.position.z, LS.Range.y);
                    else
                        lightpos_end = new Vector4(LS.Pos.x, LS.Pos.y, LS.Pos.z, LS.Range.y);

                    lightcolor_start = new Vector4(LS.Color.r * flash, LS.Color.g * flash, LS.Color.b * flash, LS.Range.x);

                    lightsPos.Add(lightpos_end);
                    lightsColor.Add(lightcolor_start);
                }
            }

            //输入Spark
            for (int i = 0; i < Spark.Count; i++)
            {
                if (lightsPos.Count == 64) // 渲染的光源数目应该小于64
                    break;

                LightSource LS = Spark[i];
                if (LS.RoomNum == Spark[0].RoomNum)  //只有和主角在同一个房间的主要光源会被渲染
                {
                    Vector4 lightpos_end;
                    Vector4 lightcolor_start;
                    float flash = 1 - Mathf.Abs(LS.flashCount - 1);

                    if (LS.lightGO != null)
                        lightpos_end = new Vector4(LS.lightGO.transform.position.x, LS.lightGO.transform.position.y, LS.lightGO.transform.position.z, LS.Range.y);
                    else
                        lightpos_end = new Vector4(LS.Pos.x, LS.Pos.y, LS.Pos.z, LS.Range.y);

                    lightcolor_start = new Vector4(LS.Color.r * flash, LS.Color.g * flash, LS.Color.b * flash, LS.Range.x);

                    lightsPos.Add(lightpos_end);
                    lightsColor.Add(lightcolor_start);
                }
            }

            Matrix4x4 wTcM = ShadowCam.projectionMatrix * ShadowCam.worldToCameraMatrix;
            MapMat.SetFloat("_LightsNum", (float)lightsPos.Count);
            if (MainLightSource.Count > 0)
            {
                MapMat.SetVectorArray("_LightsPosW_End", lightsPos);
                MapMat.SetVectorArray("_LightsColor_Start", lightsColor);
                MapMat.SetFloat("_MainNum", MainLightSource.Count);
            }

            MapMat.SetMatrix("_CamMatrix", wTcM);
            MapMat.SetFloat("_BumpScale", NormaScale);
            MapMat.SetFloat("_HeightScale", HeightScale);
            MapMat.SetFloat("_Albedo", AlbedoScale);
        }

        //将线光源的参数整理并提交到shader的函数
        private void IlluminationLine()
        {
            //遍历数组
            int arrayInit = Mathf.Min(64, LineLight.Count);
            Vector4[] start = new Vector4[arrayInit];
            Vector4[] end = new Vector4[arrayInit];
            Vector4[] color = new Vector4[arrayInit];

            for (int i = 0; i < Mathf.Min(64, LineLight.Count); i++)
            {
                //优先渲染最新的线光源
                LightSourceLine lsl = LineLight[LineLight.Count - i - 1];
                start[i] = lsl.StartPos;
                end[i] = lsl.EndPos;
                color[i] = lsl.c;
            }

            if (arrayInit > 0)
            {
                MapMat.SetFloat("_LineLightsNum", (float)arrayInit);
                MapMat.SetVectorArray("_LineStart", start);
                MapMat.SetVectorArray("_LineEnd", end);
                MapMat.SetVectorArray("_LineColor", color);
            }
        }

        //处理flash效果的类
        private void DealFlesh()
        {
            float dT = Time.deltaTime * 1;
            //处理主光源
            for (int i = 0; i < MainLightSource.Count; i++)
            {
                if (MainLightSource[i].flashCount < 0.99f)
                {
                    LightSource thisLight = MainLightSource[i];
                    thisLight.flashCount += dT;

                    if (thisLight.flashCount > 1.0f)
                        thisLight.flashCount = 1.0f;

                    MainLightSource[i] = thisLight;
                }
                else if (MainLightSource[i].flashCount > 1.01f)
                {
                    LightSource thisLight = MainLightSource[i];
                    thisLight.flashCount += dT;

                    MainLightSource[i] = thisLight;

                    if (thisLight.flashCount >= 2.0f)
                    {
                        MainLightSource.RemoveAt(i);
                        i--;
                    }
                }
            }

            //处理次要光源
            for (int i = 0; i < MiniLightSource.Count; i++)
            {
                if (MiniLightSource[i].flashCount < 0.99f)
                {
                    LightSource thisLight = MiniLightSource[i];
                    thisLight.flashCount += dT;

                    if (thisLight.flashCount > 1.0f)
                        thisLight.flashCount = 1.0f;

                    MiniLightSource[i] = thisLight;
                }
                else if (MiniLightSource[i].flashCount > 1.01f)
                {
                    LightSource thisLight = MiniLightSource[i];
                    thisLight.flashCount += dT;

                    MiniLightSource[i] = thisLight;

                    //执行销毁操作
                    if (thisLight.flashCount >= 2.0f)
                    {
                        MiniLightSource.RemoveAt(i);
                        i--;
                    }
                }
            }

            //处理Spark
            for (int i = 0; i < Spark.Count; i++)
            {
                LightSource thisLight = Spark[i];

                if (thisLight.flashCount > 0.99f)
                    thisLight.LifeTime -= dT * 4;

                //判断spark的生命周期
                if (thisLight.LifeTime < 0 && thisLight.flashCount < 1.01f)
                {
                    thisLight.flashCount = 1.02f;
                }

                //计算光线强度
                if (Spark[i].flashCount < 0.99f)
                {
                    thisLight.flashCount += dT * 4;

                    if (thisLight.flashCount > 1.0f)
                        thisLight.flashCount = 1.0f;
                }
                else if (Spark[i].flashCount > 1.01f)
                {
                    thisLight.flashCount += dT * 4;
                    //执行销毁操作
                    if (thisLight.flashCount >= 2.0f)
                    {
                        Spark.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                Spark[i] = thisLight;
            }
        }

        //处理进入光源范围过渡的类
        private void InLightChange()
        {
            if (InLightRange())
            {
                TargetAlbedo = 0.3f;
            }
            else
            {
                TargetAlbedo = 0.0f;
            }

            if (Mathf.Abs(TargetAlbedo - AlbedoScale) > 0.01f)
                AlbedoScale += (TargetAlbedo - AlbedoScale) * 0.05f;

        }

        //工具函数 ： 获取和主角在一个房间的光源个数
        public int NumLightSourceRoom()
        {
            int numL = 0;
            foreach (LightSource e in MainLightSource)
            {
                if (e.RoomNum == MainLightSource[0].RoomNum)
                    numL++;
            }
            return numL - 1;
        }

        //工具函数 ： 判断主角在不在一个有效光源的范围内
        public bool InLightRange()
        {
            bool Isin = false;
                for (int i = 1; i < MainLightSource.Count; i++)
                {
                    Vector3 posL = new Vector3(0, 0, 0);
                    Vector3 posC = MainLightSource[0].lightGO.transform.position;
                    if (MainLightSource[i].lightGO != null)
                    {
                        posL = MainLightSource[i].lightGO.transform.position;
                    }
                    else
                    {
                        posL = MainLightSource[i].Pos;
                    }

                    float dist = Vector3.Distance(posC, posL);
                    if (dist < MainLightSource[i].Range.y)
                        Isin = true;
                }
            return Isin;
        }

        //设置主角信息的函数
        public void SetCharacter(GameObject IGo, Color IlightColor, Vector2 IRange, uint roomNum)
        {
            //新建光源
            LightSource Chara = new LightSource();
            Chara.Color = IlightColor;
            Chara.lightGO = IGo;
            Chara.Range = IRange;
            Chara.RoomNum = roomNum;
            Chara.flashCount = 1.0f;

            //不用的参数也要赋值
            Chara.LifeTime = 0;
            Chara.Pos = new Vector3(0, 0, 0);

            //保存
            if (MainLightSource.Count == 0)
                MainLightSource.Add(Chara);
            else
                MainLightSource[0] = Chara;
        }

        //设置环境参数
        public void SetCurrent(float heightScale, float normalScale)
        {
            HeightScale = heightScale;
            NormaScale = normalScale;
        }

        //添加光源-GameObject
        public void AddLight(GameObject go, Color lightColor, Vector2 range, bool mainlight, uint roomNum)
        {
            LightSource Thelight = new LightSource();
            Thelight.lightGO = go;
            Thelight.Color = lightColor;
            Thelight.Range = range;
            Thelight.RoomNum = roomNum;

            Thelight.LifeTime = 0;
            Thelight.Pos = new Vector3(0, 0, 0);

            Thelight.flashCount = 0;

            if (mainlight && NumLightSourceRoom() <= 8) //主光源数量最大为8
                MainLightSource.Add(Thelight);
            else
                MiniLightSource.Add(Thelight);
        }

        //添加光源-Pos
        public void AddLight(Vector3 lightPos, Color lightColor, Vector2 range, bool mainlight, uint roomNum)
        {
            LightSource Thelight = new LightSource();
            Thelight.lightGO = null;
            Thelight.Color = lightColor;
            Thelight.Range = range;
            Thelight.RoomNum = roomNum;

            Thelight.LifeTime = 0;
            Thelight.Pos = lightPos;

            Thelight.flashCount = 0;

            if (mainlight && NumLightSourceRoom() <= 8) //一个房间的主光源数量最大为8
            {
                MainLightSource.Add(Thelight);
            }
            else
                MiniLightSource.Add(Thelight);
        }

        //添加Spark 
        public void AddSpark(Vector3 lightPos, Color lightColor, Vector2 range, float liftTime)
        {
            LightSource spark = new LightSource();
            spark.LifeTime = liftTime;
            spark.Pos = lightPos;
            spark.Color = lightColor;
            spark.lightGO = null;
            spark.Range = range;

            spark.flashCount = 0;

            Spark.Add(spark);
        }

        //添加线光源
        public void AddLineLight(GameObject goIn, Color lineColor)
        {
            //首先遍历数组，删除所有来自go的光源
            for (int i = 0; i < LineLight.Count; i++)
            {
                if (LineLight[i].go == goIn)
                {
                    LineLight.RemoveAt(i);
                    i = i - 1;
                }
            }

            //获取对象上的lineRender
            LineRenderer LR = goIn.GetComponent<LineRenderer>();
            //Gradient ligntGradient = LR.colorGradient; // 颜色梯度变化值，暂时用不到
            if (LR != null)
            {
                //添加光线
                for (int i = 0; i < LR.positionCount - 1; i++)
                {
                    LightSourceLine lsl = new LightSourceLine();
                    lsl.StartPos = LR.GetPosition(i);
                    lsl.StartPos.w = 1;                    //纹理采样坐标，暂时也用不到
                    lsl.EndPos = LR.GetPosition(i + 1);
                    lsl.EndPos.w = 1;                      //纹理采样坐标，暂时也用不到
                    lsl.c = lineColor;
                    lsl.go = goIn;

                    LineLight.Add(lsl);
                }
            }



        }

        //删除线光源
        public void DeleteLineLight(GameObject goIn)
        {
            //遍历数组，删除所有来自go的光源
            for (int i = 0; i < LineLight.Count; i++)
            {
                if (LineLight[i].go == goIn)
                {
                    LineLight.RemoveAt(i);
                    i = i - 1;
                }
            }
        }

        //控制光源闪烁的函数
        public void CharacterFlash(float fleshCount=0)
        {
            for (int i = 0; i < MainLightSource.Count; i++)
            {
                LightSource lsl = MainLightSource[i];
                lsl.flashCount = fleshCount;
                MainLightSource[i] = lsl;
            }
        }

        //删除光源
        public void DeleteLight(GameObject go)
        {
            //遍历两个列表，找到符合的光源并删除
            for (int i = 0; i < MainLightSource.Count; i++)
            {
                if (go == MainLightSource[i].lightGO)
                {
                    MainLightSource.RemoveAt(i);
                    break;
                }
            }
            for (int i = 0; i < MiniLightSource.Count; i++)
            {
                if (go == MiniLightSource[i].lightGO)
                {
                    MiniLightSource.RemoveAt(i);
                    break;
                }
            }
        }
        public void DeleteLight(Vector3 Pos)
        {
            //遍历两个列表，找到符合的光源并删除
            for (int i = 0; i < MainLightSource.Count; i++)
            {
                Vector3 lightPos = MainLightSource[i].Pos;
                if (Vector3.Distance(lightPos, Pos) < 0.01f)
                {
                    MainLightSource.RemoveAt(i);
                    break;
                }
            }
            for (int i = 0; i < MiniLightSource.Count; i++)
            {
                Vector3 lightPos = MiniLightSource[i].Pos;
                if (Vector3.Distance(lightPos, Pos) < 0.01f)
                {
                    MiniLightSource.RemoveAt(i);
                    break;
                }
            }
        }

        public void SetCamera(Vector2 CamVec, uint roomNum,float size = 20)
        {
            ShadowCam.transform.position = new Vector3(CamVec.x, CamVec.y, ShadowCam.transform.position.z);
            ShadowCam.orthographicSize = size;
            LightSource Chara = MainLightSource[0];
            Chara.RoomNum = roomNum;
            MainLightSource[0] = Chara;
        }
    }
}