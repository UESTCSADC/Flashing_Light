using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using System.Linq;
using System.Text;

namespace FlashingLight
{
    public class Map
    {
        static Map instance;
        public GameObject[] floor;
        public List<Room> roomList = new List<Room>();
        public HashSet<Vector2> bar = new HashSet<Vector2>();
        GameObject map;
        public Map()
        {
            instance = this;
            floor = LoadSprite.Instance.floor;
            Dungeon level = new Dungeon();
            map = new GameObject("map");
            //初始化迷宫
            level.InitializeDungeon(200, 200);
            //创建第一个房间
            level.CreateFirstRoom();
            //p为房间数目，循环创建10个1房间
            for (int p = 0; p < 10; p++)
            {            
                    level.CreateNextRoom();
            }
            roomList = level.roomList;
            //CreatCandlestick(new Vector2(100, 100), 1, 1, type2);
            int bulletNum = 0;
            int itemNum = 0;
            foreach (var Rm in level.roomList)
            {
                CreatNormalRoom(Rm);
                CreatItem(Rm, LoadSprite.Instance.HPPre, Random.Range(0,2));
                if (Rm.RoomID == 0)
                    ;
                else if (Rm.RoomID == 5)

                    CreatObject(new Vector2(Rm.x_Point+7,Rm.y_Point+9), LoadSprite.Instance.MiniGame);
                else
                {
                    CreatDecoration(Rm);
                    CreatNormalMonster(Rm, Random.Range(0, 3));
                    int num = Random.Range(0, 2);
                    if (num == 0)
                    {
                        if (bulletNum <= 2)
                        {
                            CreatItem(Rm, LoadSprite.Instance.BulletPre, 1);
                            bulletNum++;
                        }
                    }
                    else if (num == 1)
                    {
                        if (itemNum <= 3)
                        {
                            CreatItem(Rm, LoadSprite.Instance.ItemPre, 1);
                            bulletNum++;
                        }
                    }
                }
                Rm.InsertNodeList();

            }
            Room newone = new Room();
            newone.x_H_Length = 25; newone.y_H_Length = 25;
            newone.x_Point = -60;newone.y_Point = -60;
            level.roomList.Add(newone);
            Room newone1 = new Room();
            newone1.x_H_Length = 25; newone1.y_H_Length = 25;
            newone1.x_Point = -111; newone1.y_Point = -60;
            level.roomList.Add(newone1);
            Room newone2 = new Room();
            newone2.x_H_Length = 25; newone2.y_H_Length = 25;
            newone2.x_Point = -60; newone2.y_Point = -111;
            level.roomList.Add(newone2);


        }
         void CreatNormalRoom(Room Rm)
        {
            Rm.map = new int[Rm.x_H_Length * 2 + 1, Rm.y_H_Length * 2 + 1];
            CreatArea(new Vector2(Rm.x_Point, Rm.y_Point), Rm.x_H_Length-3, Rm.y_H_Length-3, type1);
            for (int i = Rm.x_Point - Rm.x_H_Length; i <= Rm.x_Point + Rm.x_H_Length; i++)
            {
                for (int j = Rm.y_Point - Rm.y_H_Length; j <= Rm.y_Point + Rm.y_H_Length; j++)
                {

                    if (i == Rm.x_Point - Rm.x_H_Length || i == Rm.x_Point + Rm.x_H_Length || j == Rm.y_Point - Rm.y_H_Length || j == Rm.y_Point + Rm.y_H_Length)
                    {
                        Rm.map[i - Rm.x_Point + Rm.x_H_Length, j - Rm.y_Point + Rm.y_H_Length] = 11;
                        CreatObject(new Vector2(i, j), floor[4]);

                    }
                    //左边外墙一层
                    else if (i == Rm.x_Point - Rm.x_H_Length + 1)
                    {
                        Rm.map[i - Rm.x_Point + Rm.x_H_Length, j - Rm.y_Point + Rm.y_H_Length] = 1;
                        if (Rm.left && j == Rm.y_Point - 2)
                            CreatObject(new Vector2(i, j), floor[162]);
                        else if (Rm.left && (j == Rm.y_Point || j == Rm.y_Point - 1))
                            CreatObject(new Vector2(i, j), floor[146]);
                        else if (Rm.left && j == Rm.y_Point + 1)
                            CreatObject(new Vector2(i, j), floor[130]);
                        //外墙
                        else if (j == Rm.y_Point - Rm.y_H_Length + 1)
                        {
                            CreatObject(new Vector2(i, j), floor[111]);
                        }
                        else if (j == Rm.y_Point + Rm.y_H_Length - 1)
                        {
                            CreatObject(new Vector2(i, j), floor[16]);
                        }
                        else if (j == Rm.y_Point - Rm.y_H_Length + 2)
                        {
                            CreatObject(new Vector2(i, j), floor[95]);
                        }
                        else if (j == Rm.y_Point + Rm.y_H_Length - 2)
                        {
                            CreatObject(new Vector2(i, j), floor[32]);
                        }
                        else
                            CreatObject(new Vector2(i, j), floor[80]);
                    }
                    //右边外墙一层
                    else if (i == Rm.x_Point + Rm.x_H_Length - 1)
                    {
                        Rm.map[i - Rm.x_Point + Rm.x_H_Length, j - Rm.y_Point + Rm.y_H_Length] = 1;
                        if (Rm.right && j == Rm.y_Point - 2)
                            CreatObject(new Vector2(i, j), floor[165]);
                        else if (Rm.right && (j == Rm.y_Point || j == Rm.y_Point - 1))
                            CreatObject(new Vector2(i, j), floor[149]);
                        else if (Rm.right && j == Rm.y_Point + 1)
                            CreatObject(new Vector2(i, j), floor[133]);
                        else if (j == Rm.y_Point - Rm.y_H_Length + 1)
                        {
                            CreatObject(new Vector2(i, j), floor[117]);
                        }
                        else if (j == Rm.y_Point + Rm.y_H_Length - 1)
                        {
                            CreatObject(new Vector2(i, j), floor[22]);
                        }
                        else if (j == Rm.y_Point - Rm.y_H_Length + 2)
                        {
                            CreatObject(new Vector2(i, j), floor[101]);
                        }
                        else if (j == Rm.y_Point + Rm.y_H_Length - 2)
                        {
                            CreatObject(new Vector2(i, j), floor[38]);
                        }
                        else
                            CreatObject(new Vector2(i, j), floor[86]);
                    }
                    //上边外墙一层
                    else if (j == Rm.y_Point + Rm.y_H_Length - 1)
                    {
                        Rm.map[i - Rm.x_Point + Rm.x_H_Length, j - Rm.y_Point + Rm.y_H_Length] = 1;

                        if (Rm.up && i == Rm.x_Point - 2)
                            CreatObject(new Vector2(i, j), floor[127]);
                        else if (Rm.up && (i == Rm.x_Point || i == Rm.x_Point - 1))
                            CreatObject(new Vector2(i, j), floor[128]);
                        else if (Rm.up && i == Rm.x_Point + 1)
                            CreatObject(new Vector2(i, j), floor[129]);
                        //外墙
                        else if (i == Rm.x_Point - Rm.x_H_Length + 2)
                        {
                            CreatObject(new Vector2(i, j), floor[17]);
                        }
                        else if (i == Rm.x_Point + Rm.x_H_Length - 2)
                        {
                            CreatObject(new Vector2(i, j), floor[21]);
                        }
                        else
                            CreatObject(new Vector2(i, j), floor[20]);
                    }
                    //下边外墙一层
                    else if (j == Rm.y_Point - Rm.y_H_Length + 1)
                    {
                        Rm.map[i - Rm.x_Point + Rm.x_H_Length, j - Rm.y_Point + Rm.y_H_Length] = 1;

                        if (Rm.down && i == Rm.x_Point - 2)
                            CreatObject(new Vector2(i, j), floor[174]);
                        else if (Rm.down && (i == Rm.x_Point || i == Rm.x_Point - 1))
                            CreatObject(new Vector2(i, j), floor[175]);
                        else if (Rm.down && i == Rm.x_Point + 1)
                            CreatObject(new Vector2(i, j), floor[176]);
                        //外墙
                        else if (i == Rm.x_Point - Rm.x_H_Length + 2)
                        {
                            CreatObject(new Vector2(i, j), floor[112]);
                        }
                        else if (i == Rm.x_Point + Rm.x_H_Length - 2)
                        {
                            CreatObject(new Vector2(i, j), floor[116]);
                        }
                        else
                            CreatObject(new Vector2(i, j), floor[115]);
                    }
                    //左边外墙二层
                    else if (i == Rm.x_Point - Rm.x_H_Length + 2)
                    {
                        Rm.map[i - Rm.x_Point + Rm.x_H_Length, j - Rm.y_Point + Rm.y_H_Length] = 1;
                        if (Rm.left && j == Rm.y_Point - 2)
                            CreatObject(new Vector2(i, j), floor[163]);
                        else if (Rm.left && (j == Rm.y_Point || j == Rm.y_Point - 1))
                            CreatObject(new Vector2(i, j), floor[5]);
                        else if (Rm.left && j == Rm.y_Point + 1)
                            CreatObject(new Vector2(i, j), floor[131]);
                        else if (j == Rm.y_Point - Rm.y_H_Length + 2)
                        {
                            CreatObject(new Vector2(i, j), floor[96]);
                        }
                        else if (j == Rm.y_Point + Rm.y_H_Length - 2)
                        {
                            CreatObject(new Vector2(i, j), floor[33]);
                        }
                        else
                            CreatObject(new Vector2(i, j), floor[81]);
                    }
                    //右边外墙二层
                    else if (i == Rm.x_Point + Rm.x_H_Length - 2)
                    {
                        Rm.map[i - Rm.x_Point + Rm.x_H_Length, j - Rm.y_Point + Rm.y_H_Length] = 1;
                        if (Rm.right && j == Rm.y_Point - 2)
                            CreatObject(new Vector2(i, j), floor[164]);
                        else if (Rm.right && (j == Rm.y_Point || j == Rm.y_Point - 1))
                            CreatObject(new Vector2(i, j), floor[5]);
                        else if (Rm.right && j == Rm.y_Point + 1)
                            CreatObject(new Vector2(i, j), floor[132]);
                        else if (j == Rm.y_Point - Rm.y_H_Length + 2)
                        {
                            CreatObject(new Vector2(i, j), floor[100]);
                        }
                        else if (j == Rm.y_Point + Rm.y_H_Length - 2)
                        {
                            CreatObject(new Vector2(i, j), floor[37]);
                        }
                        else
                            CreatObject(new Vector2(i, j), floor[85]);
                    }
                    //上边外墙二层
                    else if (j == Rm.y_Point + Rm.y_H_Length - 2)
                    {
                        Rm.map[i - Rm.x_Point + Rm.x_H_Length, j - Rm.y_Point + Rm.y_H_Length] = 1;

                        if (Rm.up && i == Rm.x_Point - 2)
                            CreatObject(new Vector2(i, j), floor[143]);
                        else if (Rm.up && (i == Rm.x_Point || i == Rm.x_Point - 1))
                            CreatObject(new Vector2(i, j), floor[5]);
                        else if (Rm.up && i == Rm.x_Point + 1)
                            CreatObject(new Vector2(i, j), floor[145]);
                        //外墙
                        else
                            CreatObject(new Vector2(i, j), floor[36]);
                    }
                    //下边外墙二层
                    else if (j == Rm.y_Point - Rm.y_H_Length + 2)
                    {
                        Rm.map[i - Rm.x_Point + Rm.x_H_Length, j - Rm.y_Point + Rm.y_H_Length] = 1;
                        if (Rm.down && i == Rm.x_Point - 2)
                            CreatObject(new Vector2(i, j), floor[159]);
                        else if (Rm.down && (i == Rm.x_Point || i == Rm.x_Point - 1))
                            CreatObject(new Vector2(i, j), floor[5]);
                        else if (Rm.down && i == Rm.x_Point + 1)
                            CreatObject(new Vector2(i, j), floor[161]);
                        //外墙
                        else
                            CreatObject(new Vector2(i, j), floor[97]);
                    }
                    else
                    {
                        Rm.map[i - Rm.x_Point + Rm.x_H_Length, j - Rm.y_Point + Rm.y_H_Length] = 12;
                    }
                }
            }            
        }
        /// <summary>
        /// 为房间创建装饰
        /// </summary>
        /// <param name="Rm"></param>
        void CreatDecoration(Room Rm)
        {
            int num = Random.Range(0, 3);
            if (num == 0)
                CreatAreaWithCandlestick(new Vector2(Rm.x_Point, Rm.y_Point), Random.Range(2, 7), Random.Range(5, 7), carpet, Rm);
            else if (num == 1)
            {
                int width = Random.Range(2, 4);
                int height = Random.Range(5, 7);
                CreatArea(new Vector2(Rm.x_Point - 6, Rm.y_Point), width, height, carpet);
                CreatArea(new Vector2(Rm.x_Point + 6, Rm.y_Point), width, height, carpet);
                CreatCandlestick(new Vector2(Rm.x_Point, Rm.y_Point - 6), Rm);
                CreatCandlestick(new Vector2(Rm.x_Point, Rm.y_Point + 6), Rm);
                CreatCandlestick(new Vector2(Rm.x_Point - 10, Rm.y_Point - 6), Rm);
                CreatCandlestick(new Vector2(Rm.x_Point - 10, Rm.y_Point + 6), Rm);
                CreatCandlestick(new Vector2(Rm.x_Point + 10, Rm.y_Point - 6), Rm);
                CreatCandlestick(new Vector2(Rm.x_Point + 10, Rm.y_Point + 6), Rm);
            }
            else
            {
                int width = Random.Range(2, 4);
                int height = Random.Range(4, 5);
                CreatArea(new Vector2(Rm.x_Point, Rm.y_Point - 5 + width + height), width, height, type2);
                CreatArea(new Vector2(Rm.x_Point, Rm.y_Point - 6), height, width, type2);

                CreatCandlestick(new Vector2(Rm.x_Point - 7, Rm.y_Point - 6), Rm);
                CreatCandlestick(new Vector2(Rm.x_Point + 7, Rm.y_Point - 6), Rm);
                CreatCandlestick(new Vector2(Rm.x_Point, Rm.y_Point - 4 + width + height * 2), Rm);
                CreatCandlestick(new Vector2(Rm.x_Point - 4, Rm.y_Point - 5 + width + height), Rm);
                CreatCandlestick(new Vector2(Rm.x_Point + 4, Rm.y_Point - 5 + width + height), Rm);
            }
        }
        public static Map Instance
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
        public bool isBar(Vector2 pos)
        {
            return bar.Contains(pos);
        }
        public Room Pos2Room(Vector2 pos)
        {
            Room room = null;
            foreach (var item in roomList)
            {
                if (item.x_Point - item.x_H_Length < pos.x && item.x_Point + item.x_H_Length > pos.x &&
                    item.y_Point - item.y_H_Length < pos.y && item.y_Point + item.y_H_Length > pos.y)
                    room=item;
            }
            return room;
        }
        GameObject CreatObject(Vector2 pos,GameObject obj)
        {
            Transform go = Object.Instantiate(obj).transform;
            go.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            go.position = pos;
            go.SetParent(map.transform);
            return go.gameObject;
        }

        void CreatArea(Vector2 Pos,int width,int height,int[,] type)
        {

           for(int i= (int)(Pos.x - width);i<= Pos.x + width;i++)
                for (int j = (int)(Pos.y - height); j <= Pos.y + height; j++)
                {

                    if(i== Pos.x - width&&j== Pos.y - height)
                    {
                        if (type[2, 0] >= 0)
                            CreatObject(new Vector2(i,j), floor[type[2, 0]]);
                    }
                    else if (i == Pos.x - width && j == Pos.y + height)
                    {
                        if(type[2,2]>=0)
                        CreatObject(new Vector2(i, j), floor[type[0, 0]]);
                    }
                    else if (i == Pos.x +width && j == Pos.y - height)
                    {
                        if (type[0, 0] >= 0)
                            CreatObject(new Vector2(i, j), floor[type[2, 2]]);
                    }
                    else if (i == Pos.x + width && j == Pos.y + height)
                    {
                        if (type[0, 2] >= 0)
                            CreatObject(new Vector2(i, j), floor[type[0, 2]]);
                    }
                    else if (i == Pos.x - width)
                    {
                        if (type[1, 0] >= 0)
                            CreatObject(new Vector2(i, j), floor[type[1,0]]);
                    }
                    else if (i == Pos.x + width)
                    {
                        if (type[1, 2] >= 0)
                            CreatObject(new Vector2(i, j), floor[type[1, 2]]);
                    }
                    else if (j == Pos.y - height)
                    {
                        if (type[2, 1] >= 0)
                            CreatObject(new Vector2(i, j), floor[type[2, 1]]);
                    }
                    else if (j == Pos.y+ height)
                    {
                        if (type[0, 1] >= 0)
                            CreatObject(new Vector2(i, j), floor[type[0, 1]]);
                    }
                            if (type[1, 1] >= 0)
                                CreatObject(new Vector2(i, j), floor[type[1, 1]]);
                }
        }
        int[,] type1 = new int[3, 3]{{13,14,15},{29,0,31},{45,46,47 }};
        int[,] type2 = new int[3, 3] { { 61, 62, 63 }, { 77, 2, 79 }, { 92, 93, 94 } };
        int[,] type3 = new int[3, 3] { { 13, 14, 15 }, { 29, -1, 31 }, { 45, 46, 47 } };
        int[,] type4 = new int[3, 3] { { 61, 62, 63 }, { 77, -1, 79 }, { 92, 93, 94 } };
        int[,] carpet= new int[3, 3] { { 108,109, 110 }, { 124, 125, 126 }, { 140, 141, 142 } };
        /// <summary>
        /// 创建烛台
        /// </summary>
        /// <param name="Pos"></param>
        void CreatCandlestick(Vector2 Pos,Room Rm)
        {
            int type = Random.Range(0, 3);
            if (type == 1)
            {
                CreatObject(new Vector2(Pos.x, Pos.y + 1), floor[78]).GetComponent<RoomLiaght>().SetRoomLight(new Color(0.3f, 0.3f, 0.5f), new Vector2(0, 15.0f), (uint)Rm.RoomID);
                Rm.map[(int)Pos.x - Rm.x_Point + Rm.x_H_Length, (int)Pos.y + 1 - Rm.y_Point + Rm.y_H_Length] = 1;
            }
            else
            {
            CreatObject(new Vector2(Pos.x, Pos.y+1), floor[91]);
            CreatObject(new Vector2(Pos.x, Pos.y), floor[106]);
            Rm.map[(int)Pos.x - Rm.x_Point + Rm.x_H_Length, (int)Pos.y + 1 - Rm.y_Point + Rm.y_H_Length] = 1;
            Rm.map[(int)Pos.x - Rm.x_Point + Rm.x_H_Length, (int)Pos.y - Rm.y_Point + Rm.y_H_Length] = 1;
            }
        }
        void CreatAreaWithCandlestick(Vector2 Pos, int width, int height, int[,] type,Room Rm)
        {
            CreatArea(Pos, width, height, type);
            CreatCandlestick(new Vector2(Pos.x - width,Pos.y-height),Rm);
            CreatCandlestick(new Vector2(Pos.x + width, Pos.y - height), Rm);
            CreatCandlestick(new Vector2(Pos.x - width, Pos.y + height), Rm);
            CreatCandlestick(new Vector2(Pos.x + width, Pos.y + height), Rm);
        }

        void CreatItem(Room Rm, List<GameObject> item,int num)
        {
            if (item == null && item.Count < 1)
                return;
            for (int i = 0; i < num; i++)
            {
                
                int posx = Random.Range(Rm.x_Point - Rm.x_H_Length + 4, Rm.x_Point + Rm.x_H_Length - 3);
                int posy = Random.Range(Rm.y_Point - Rm.y_H_Length + 4, Rm.y_Point + Rm.y_H_Length - 3);
                while(Rm.map[posx- Rm.x_Point + Rm.x_H_Length, posy- Rm.y_Point + Rm.y_H_Length] <10)
                {
                    posx = Random.Range(Rm.x_Point - Rm.x_H_Length + 3, Rm.x_Point + Rm.x_H_Length - 3);
                    posy = Random.Range(Rm.y_Point - Rm.y_H_Length + 3, Rm.y_Point + Rm.y_H_Length - 3);
                }
                Rm.map[posx - Rm.x_Point + Rm.x_H_Length, posy - Rm.y_Point + Rm.y_H_Length] = 1;
                int number = Random.Range(0, item.Count);
                MonoBehaviour.Instantiate(item[number], new Vector2(posx,posy), Quaternion.identity);
            }
        }
        void CreatNormalMonster(Room Rm, int num)
        {
            for (int i = 0; i < num; i++)
            {
                Rm.roomStyle = "Fighting";
                int posx = Random.Range(Rm.x_Point - Rm.x_H_Length + 4, Rm.x_Point + Rm.x_H_Length - 3);
                int posy = Random.Range(Rm.y_Point - Rm.y_H_Length + 4, Rm.y_Point + Rm.y_H_Length - 3);
                while (Rm.map[posx - Rm.x_Point + Rm.x_H_Length, posy - Rm.y_Point + Rm.y_H_Length] < 10)
                {
                    posx = Random.Range(Rm.x_Point - Rm.x_H_Length + 4, Rm.x_Point + Rm.x_H_Length - 3);
                    posy = Random.Range(Rm.y_Point - Rm.y_H_Length + 4, Rm.y_Point + Rm.y_H_Length - 3);
                }
                Rm.map[posx - Rm.x_Point + Rm.x_H_Length, posy - Rm.y_Point + Rm.y_H_Length] = 9;
                int type = Random.Range(0, LoadSprite.Instance.normalMonster.Count);
                //GameObject go=MonoBehaviour.Instantiate(LoadSprite.Instance.normalMonster[type], new Vector2(posx,posy), Quaternion.identity);
                //go.GetComponent<NoMoveMonster>().monsterAttributes.RoomID = Rm.RoomID;
                //Rm.monster.Add(go);
                Rm.monster.Add(MonoBehaviour.Instantiate(LoadSprite.Instance.normalMonster[type], new Vector2(posx, posy), Quaternion.identity));
            }
        }
    }
    public class Room
    {
        public int x_H_Length;
        public int y_H_Length;
        public int x_Point;
        public int y_Point;
        public string roomStyle;
        public bool up=false;
        public bool down=false;
        public bool right = false;
        public bool left = false;
        public List<GameObject> itemPool = new List<GameObject>();
        public int[,] map;
        public int RoomID;
        public List<GameObject> monster = new List<GameObject>();

        public void SetMonsterActive(bool isActive)
        {
            foreach (var item in monster)
            {
                if(item!=null)
                item.SetActive(isActive);
            }
        }

		public Dictionary<Vector2, Node> nodeDictionary = new Dictionary<Vector2, Node>();

		public void InsertNodeList()
		{
			for (int i = x_Point - x_H_Length; i <= x_Point + x_H_Length; i++)
			{
				for (int j = y_Point - y_H_Length; j <= y_Point + y_H_Length; j++)
				{
					int mapX = i - x_Point + x_H_Length;
					int mapY = j - y_Point + y_H_Length;
					Node nodeTemp = new Node((map[mapX, mapY] > 9), new Vector2(i, j), mapX, mapY);
					//nodeList.Add(nod);
					nodeDictionary[new Vector2(i,j)] = nodeTemp;
				}
			}
		}

    }
    public class NormalRoom : Room
    {
        public NormalRoom()
        {
            x_H_Length = y_H_Length = 15;
            roomStyle = "NormalRoom";
        }
    }
    public class BossRoom : Room
    {
        public BossRoom()
        {
            x_H_Length = y_H_Length = 15;
            roomStyle = "NormalRoom";
        }
    }
    //可以自己创建需要的房间类如：商店 boss房等，这里我只用到普通房间为例子
    public enum Mark
    {
        floor = '#',
        door = 'D',
        wall = 'm',
        chests = 'c'

    }
    public class Dungeon
    {
        //存储房间的数组
        public List<Room> roomList = new List<Room>();
        int roomListLength = 0;

        //定义地牢的整体大小
        int sizex, sizey;
        public char[,] map;
        //创建地图元素的枚举这里只用到了地板和门：#和D根据需要来创建这些枚举
        int doorTempx, doorTempy, areaTempx, areaTempy;
        public void InitializeDungeon(int sizex, int sizey)
        {
            this.sizex = sizex;
            this.sizey = sizey;
            map = new char[sizex, sizey];

        }

        //创建房间的方法，下面通过CreateFirstRoom和CreateNextRoom将它调用
        void GenerateRoom(Room Rm)
        {
            for (int i = Rm.x_Point - Rm.x_H_Length; i <= Rm.x_Point + Rm.x_H_Length; i++)
            {
                for (int j = Rm.y_Point - Rm.y_H_Length; j <= Rm.y_Point + Rm.y_H_Length; j++)
                {
                   // if (i == Rm.x_Point - Rm.x_H_Length || i == Rm.x_Point + Rm.x_H_Length || j == Rm.y_Point - Rm.y_H_Length || j == Rm.y_Point + Rm.y_H_Length)
                        //map[i, j] = (char)Mark.floor;
                    //else
                        map[i, j] = (char)Mark.floor;
                }
            }
            Rm.RoomID = roomList.Count;
            roomList.Add(Rm);
            roomListLength++;
        }

        //创建第一个房间
        public void CreateFirstRoom()
        {
            NormalRoom firstR = new NormalRoom();
            //在中心位置创建出初始房间
            firstR.x_Point = sizex / 2;
            firstR.y_Point = sizey / 2;
            GenerateRoom(firstR);
        }

        //上文提到的CreateNextRoom方法：用于在第一个房间创建完成后继续创建房间。
        public void CreateNextRoom()
        {
            int roomNum;
            int wallPos=4;
            bool CDtemp = true;
            while (CDtemp)
            {
                roomNum = Random.Range(0, roomList.Count);
                wallPos = Random.Range(0,(int) 4); 
                Room m = roomList[roomNum];
                //ChoiceNextArea(m, wallPos);
                if (ChoiceNextArea(m, wallPos))
                {
                    CDtemp = false;
                    switch(wallPos)
                    {
                        case 0:
                            m.right=true;
                            break;
                        case 1:
                            m.up=true;
                            break;
                        case 2:
                            m.left=true;
                            break;
                        case 3:
                            m.down=true;
                            break;

                    }
                }
            }
            map[doorTempx, doorTempy] = (char)Mark.door;
            Room CNRRoom = new NormalRoom();
            CNRRoom.x_Point = areaTempx;
            CNRRoom.y_Point = areaTempy;
            switch (wallPos)
            {
                case 2:
                    CNRRoom.right = true;
                    break;
                case 3:
                    CNRRoom.up = true;
                    break;
                case 0:
                    CNRRoom.left = true;
                    break;
                case 1:
                    CNRRoom.down = true;
                    break;

            }
            GenerateRoom(CNRRoom);
        }
        //该方法用于选择所选定房间的方向
        private bool ChoiceNextArea(Room Room, int Pos)
        {
            bool CDtemp = false;
            switch (Pos)
            {
                case 0:
                    if (Room.x_Point + 3 * Room.x_H_Length + 2 < sizex && map[Room.x_Point + 2 * Room.x_H_Length + 2, Room.y_Point] != '#')
                    {
                        //下方的步骤是为了标记所选方向准备创建的房间中心以及连接两房间门的标记
                        doorTempx = Room.x_Point + Room.x_H_Length + 1;
                        doorTempy = Room.y_Point;
                        areaTempx = Room.x_Point + 2 * Room.x_H_Length + 1;
                        areaTempy = Room.y_Point;
                        CDtemp = true;
                    };
                    break;

                case 1:
                    if (Room.y_Point + 3 * Room.y_H_Length + 2 <sizey && map[Room.x_Point , Room.y_Point +2 * Room.y_H_Length +1] != '#')
                    {
                        doorTempx = Room.x_Point;
                        doorTempy = Room.y_Point +Room.y_H_Length + 1; ;
                        areaTempx = Room.x_Point ;
                        areaTempy = Room.y_Point +2 * Room.y_H_Length + 1;
                        CDtemp = true;
                    };
                    break;
                case 2:
                    if (Room.x_Point -3 * Room.x_H_Length -1 > 0 && map[Room.x_Point - 2 * Room.x_H_Length - 1, Room.y_Point] != '#')
                    {
                        doorTempx = Room.x_Point - Room.x_H_Length - 1;
                        doorTempy = Room.y_Point;
                        areaTempx = Room.x_Point - 2 * Room.x_H_Length - 1;
                        areaTempy = Room.y_Point;
                        CDtemp = true;
                    };
                    break;
                case 3:
                    if (Room.y_Point - 3 * Room.y_H_Length - 1 > 0 && map[Room.x_Point, Room.y_Point - 2 * Room.y_H_Length - 1] != '#')
                    {
                        doorTempx = Room.x_Point;
                        doorTempy = Room.y_Point - Room.y_H_Length - 1;
                        areaTempx = Room.x_Point;
                        areaTempy = Room.y_Point - 2 * Room.y_H_Length - 1;
                        CDtemp = true;
                    };
                    break;
            }
            return CDtemp;
        }

        public static void FinishDungeon()
        {
            Dungeon level = new Dungeon();
            //初始化迷宫
            level.InitializeDungeon(100, 100);
            //创建第一个房间
            level.CreateFirstRoom();
            //p为房间数目，循环创建10个1房间
            for (int p = 0; p < 10; p++)
            {
                level.CreateNextRoom();
            }
        }
    }
}
