using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OutputClicedMapSpritePerfab : MonoBehaviour {
    //用于批量构造SpritePrefab的函数
    [MenuItem("Tools/CreatPrefab")]
    static void CreatPrefab()
    {
        string loadPath = "DungeonMap";

        Sprite[] sprites = Resources.LoadAll<Sprite>(loadPath);
        if (sprites.Length > 0)
        {
            // 创建导出文件夹
            string outPath = Application.dataPath + "/outSpritePerfab/" + loadPath;
            System.IO.Directory.CreateDirectory(outPath);

            int i = 0;
            foreach (Sprite sprite in sprites)
            {
                // 创建空prefab
                string name = "perfab" + i;
                Object tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/Resources/SpriteMapPrefab/" + name + ".prefab");

                //构造GameObject
                GameObject prefabGO = new GameObject();
                prefabGO.AddComponent<SpriteRenderer>();
                SpriteRenderer prefabSR = prefabGO.GetComponent<SpriteRenderer>();
                prefabSR.sprite = sprite;
                prefabSR.material = Resources.Load<Material>("Materials/MapMat");

                // 输出
                PrefabUtility.ReplacePrefab(prefabGO, tempPrefab);

                i++;
            }
            Debug.Log("SaveSprite to " + outPath);
        }
        else if (sprites.Length == 0)
        {
            Debug.Log("Load Err");
        }

        Debug.Log("SaveSprite Finished");
    }
}
