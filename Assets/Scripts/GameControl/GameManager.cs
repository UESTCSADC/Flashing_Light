using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace FlashingLight
{
    public class GameManager : MonoBehaviour
    {

        public GameObject GameOverUI;

        static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        LoadSprite spriteResouse;
        public event Action gameStart;
        public void GameStar()
        {
            //加载贴图资源

            if (gameStart != null)
                gameStart.Invoke();
            spriteResouse = new LoadSprite();
            GameObject player =// Instantiate(LoadSprite.Instance.playerObj);

            new GameObject("Player");
            player.name = "Player";
            player.transform.position =new Vector3(-59, -53, 3);// new Vector3(94, 92, 3);
            player.AddComponent<Player>();//.SetPos(new Vector2(100,100));
            Map map = new Map();
            
        }
        public event Action gameOver;
        public void GameOver()
        {
            if (gameOver != null)
                gameOver.Invoke();
        }
        // Use this for initialization
        void Start()
        {
            gameOver += OverDo;
        }
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            GameStar();
        }
        void OverDo()
        {
            Invoke("ShowOverUI", 1.0F);
        }
        void ShowOverUI()
        {
            GameOverUI.SetActive(true);
            Invoke("LoadScence", 2.0F);
        }
        void LoadScence()
        {
            
            SceneManager.LoadSceneAsync("Menue");
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                LoadScence();
        }

    }
}
