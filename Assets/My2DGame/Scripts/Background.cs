using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 카메라와 배경의 Z값 차이를 이용한 패럴랙스 효과
    /// </summary>
    public class Background : MonoBehaviour
    {
        [SerializeField] private float parallaxFactor = 0.5f; // 0 = 완전고정, 1 = 카메라랑 같이 움직임

        private Camera cam;
        private Vector3 lastCamPos;

        private void Start()
        {
            cam = Camera.main;
            lastCamPos = cam.transform.position;
        }

        private void LateUpdate()
        {
            // 카메라가 이전 프레임에서 얼마나 움직였는지
            Vector3 camDelta = cam.transform.position - lastCamPos;

            // 패럴랙스 비율만큼만 배경 이동
            transform.position += new Vector3(camDelta.x * parallaxFactor, 0f, 0f);

            lastCamPos = cam.transform.position;
        }
    }
}