using UnityEngine;

namespace My2DGame
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;  // 플레이어
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -10f);

        private void LateUpdate()
        {
            if (target == null) return;

            // Y축은 고정, X축만 따라가기
            Vector3 desired = new Vector3(target.position.x + offset.x, offset.y, offset.z);
            transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        }
    }
}