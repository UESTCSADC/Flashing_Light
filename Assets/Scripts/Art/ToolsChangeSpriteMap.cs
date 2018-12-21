using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ToolsChangeSpriteMap : MonoBehaviour
{

    [MenuItem("Tools/ChangeMap")]
    static void SaveSprite()
    {
        Object[] labels = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);
        Material shader = Resources.Load<Material>("Materials/NewMapMat");
        Texture2D bumpMap = Resources.Load<Texture2D>("DungeonMapN");
        //Texture2D SpriteMap = Resources.Load<Texture2D>("DungeonMap");

        Debug.Log("there are " + labels.GetLength(0).ToString() + " Go have been Loaded");
        foreach (GameObject go in labels)
        {
            SpriteRenderer SR = go.GetComponent<SpriteRenderer>();
            if (SR != null)
            {
                //Sprite sp = Sprite.Create(SpriteMap, SR.sprite.textureRect, new Vector2(0.5f, 0.5f));
                //SR.sprite = sp;
                SR.sharedMaterial = shader;
                SR.sharedMaterial.SetTexture("_BumpMap", bumpMap);
            }
        }
    }
}
