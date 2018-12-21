using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
    public class MINIMapCamera : MonoBehaviour
    {
        Camera camera;
        Material Mini;
        public Shader mapshader;
        // Use this for initialization
        void Start()
        {
            camera = GetComponent<Camera>();
            //mapshader = LoadSprite.Instance.mapMat.shader;
            Mini = new Material(mapshader);
            Mini.SetFloat("_AlbedoScale111", 0.5f);
            camera.SetReplacementShader(Mini.shader, "RenderType");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
