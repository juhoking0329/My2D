using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 카메라와 배경의 Z값 차이를 이용한 패럴랙스 효과
    /// </summary>
    public class Background : MonoBehaviour
    {
        private Camera cam;
        private Vector3 lastCamPos;

        [SerializeField] private float parallaxStrength = 1f; // 패럴랙스 강도 (튜닝용)

        private void Start()
        {
            cam = Camera.main;
            lastCamPos = cam.transform.position;
        }

        private void LateUpdate()
        {
            Vector3 camDelta = cam.transform.position - lastCamPos;

            // Z값이 클수록 느리게 움직이도록 반전
            float parallaxFactor = 1f / (transform.position.z + parallaxStrength);

            transform.position += new Vector3(camDelta.x * parallaxFactor, 0f, 0f);

            lastCamPos = cam.transform.position;
        }
    }
}