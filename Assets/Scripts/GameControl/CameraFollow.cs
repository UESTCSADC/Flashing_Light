using UnityEngine;
using System.Collections;
namespace FlashingLight
{
    public class CameraFollow : MonoBehaviour
    {
        private const float Speed = 5.0f;
        public Transform m_Target;

        private float defaultZ;
        // Use this for initialization
        void Start()
        {
            defaultZ = transform.position.z;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (m_Target == null)
                return;
            Vector3 targetPosition = m_Target.position;
            targetPosition.z = defaultZ;
            transform.position = targetPosition;
        }
    }
}
