using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
    public class TrailMaker : MonoBehaviour
    {
        public float hp = 100.0f;
        //这个就是拖尾的数组
        private List<Vector4> TrailLine;

        // Use this for initialization
        void Start()
        {
            TrailLine = new List<Vector4>();
            TrailLine.Add(new Vector4(transform.position.x, transform.position.y + 0.001f, transform.position.z, 1));
            TrailLine.Add(new Vector4(transform.position.x, transform.position.y, transform.position.z, 1)); //开始必须设置拖尾队列
        }

        // Update is called once per frame
        void Update()
        {
            TrailCalc();

        }
        public void CleanAll()
        {
            TrailLine.Clear();
            TrailLine.Add(new Vector4(transform.position.x, transform.position.y + 0.001f, transform.position.z, 1));
            TrailLine.Add(new Vector4(transform.position.x, transform.position.y, transform.position.z, 1)); //开始必须设置拖尾队列

        }
        void TrailCalc()
        {
            //需要获取当前的坐标和角色的生命值
            Vector3 Pos = gameObject.transform.position;


            //player类需要维护一个数组TrailLine

            //取出TrailLine[0]和[1]，即最近的一条轨迹
            Vector4 preLineStart = TrailLine[1];
            Vector4 preLineEnd = TrailLine[0];

            if (Pos != (Vector3)preLineEnd)
            {
                //判断目前的位置是否还在轨迹上
                Vector3 preLine = preLineEnd - preLineStart;
                Vector3 thisLine = Pos - (Vector3)preLineEnd;

                if (Vector3.Distance(preLine.normalized, thisLine.normalized) < 0.001f) //在一条线上，修正最后一条轨迹的端点
                {
                    TrailLine[0] = Pos;
                }
                else  //否则添加新的轨迹
                {
                    TrailLine.Insert(0, Pos);
                }

                //遍历轨迹，修正轨迹的位置采样坐标  --可能以后会用到
                float length = hp; //记录现在有多长
                for (int i = 0; i < TrailLine.Count - 1; i++)
                {
                    Vector4 trailEnd = TrailLine[i];
                    Vector4 trailStart = TrailLine[i + 1];

                    //修正终点
                    trailEnd.w = length / hp;

                    //计算距离
                    float dist = Vector3.Distance(trailStart, trailEnd);
                    length -= dist;
                    if (length < 0) //如果线条结束了，修正终点坐标并删去多余的线条
                    {
                        trailStart = trailEnd + (dist + length) * (trailStart - trailEnd) / dist;
                        length = 0;
                        if (TrailLine.Count > i + 2)
                            TrailLine.RemoveRange(i + 2, TrailLine.Count - i - 2);
                    }

                    //修正起点
                    trailStart.w = length / hp;

                    //保存
                    TrailLine[i] = trailEnd;
                    TrailLine[i + 1] = trailStart;
                }

                //设置拖尾渲染器
                LineRenderer LR = this.gameObject.GetComponent<LineRenderer>();
                LR.positionCount = TrailLine.Count;
                Vector2[] EC2DPos = new Vector2[TrailLine.Count];

                for (int i = 0; i < TrailLine.Count; i++)
                {
                    LR.SetPosition(i, TrailLine[i]);
                    EC2DPos[i] = transform.worldToLocalMatrix * (new Vector2(TrailLine[i].x, TrailLine[i].y) - (Vector2)Pos);
                }

                //设置触发器
                EdgeCollider2D EC2D = this.gameObject.GetComponent<EdgeCollider2D>();
                EC2D.points = EC2DPos;

                //最后把数据传给LightManager
                LightManager.Instance.AddLineLight(this.gameObject, Color.blue);
            }
        }
    }
}