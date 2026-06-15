using UnityEngine;

namespace MyBird
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField] private bool autoCalculateOffset = true;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

        [Header("Follow")]
        [SerializeField] private bool followX = true;
        [SerializeField] private bool followY = false;
        [SerializeField] private float smoothSpeed = 5f;


        private void Start()
        {
            if (target == null)
            {
                // 태그가 설정된 플레이어를 자동으로 찾기 시도
                var go = GameObject.FindWithTag("Player");
                if (go != null) target = go.transform;
            }

            if (target != null)
            {
                if (autoCalculateOffset)
                {
                    offset = transform.position - target.position;
                }
            }
            else
            {
                Debug.LogWarning("CameraController: target이 설정되지 않았습니다.");
            }
        }

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desired = transform.position;
            if (followX) desired.x = target.position.x + offset.x;
            if (followY) desired.y = target.position.y + offset.y;
            desired.z = target.position.z + offset.z; // 카메라의 z는 offset 기준 유지

            transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        }
    }
}