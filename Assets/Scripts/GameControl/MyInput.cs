using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
    [RequireComponent(typeof(Player))]
    public class MyInput : MonoBehaviour
    {



        private const float SlideDistance = 0.5f;

        private Player m_Player;
        // Use this for initialization
        void Start()
        {
            m_Player = GetComponent<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            Direction direction = Direction.None;


#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                direction = Direction.Left;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                direction = Direction.Up;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                direction = Direction.Right;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                direction = Direction.Down;
            if (Input.GetKeyDown(KeyCode.Space))
                LightManager.Instance.AddLight(m_Player.transform.position, new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f)), new Vector2(0,25), true, (uint)m_Player.RoomID());
            
            if (Input.GetKeyUp(KeyCode.Z))
                m_Player.Fire();

            if (Input.GetKeyDown(KeyCode.X))
            {
                m_Player.UseItem();
            }

#endif

            /*
    #elif UNITY_ANDROID || UNITY_IPHONE
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                float absX = Mathf.Abs(mouseX);
                float absY = Mathf.Abs(mouseY);
                if (absX > SlideDistance || absY > SlideDistance)
                {
                    if (absX > absY)
                        m_Player.SetChangeDirection(mouseX > 0f ? Direction.Right : Direction.Left);
                    else
                        m_Player.SetChangeDirection(mouseY > 0f ? Direction.Up : Direction.Down);
                }
            }
     */
            if (direction != Direction.None)
                m_Player.attributes.SetDirection(direction);
        }

    }
}