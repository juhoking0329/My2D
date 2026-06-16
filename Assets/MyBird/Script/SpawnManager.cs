using UnityEngine;

namespace MyBird
{
    public class SpawnManager : MonoBehaviour
    {
        [Header("Pipe")]
        [SerializeField] private GameObject pipePrefab;

        [Header("Spawn Time")]
        [SerializeField] private float minInterval = 0.95f;
        [SerializeField] private float maxInterval = 1.05f;

        [Header("Height")]
        [SerializeField] private float minY = -1.5f;
        [SerializeField] private float maxY = 3f;

        private bool isPlaying = false;
        private float timer = 0f;
        private float nextInterval;

        private void OnEnable()
        {
            Player.OnGameStart += StartSpawning;
            Player.OnGameOver += StopSpawning;  // 추가
        }

        private void OnDisable()
        {
            Player.OnGameStart -= StartSpawning;
            Player.OnGameOver -= StopSpawning;  // 추가
        }

        private void StartSpawning()
        {
            isPlaying = true;
            nextInterval = GetRandomInterval();
            timer = 0f;
        }

        private void StopSpawning()
        {
            isPlaying = false;  // 스폰 중단
        }

        private void Update()
        {
            if (!isPlaying) return;

            timer += Time.deltaTime;

            if (timer >= nextInterval)
            {
                SpawnPipe();
                timer = 0f;
                nextInterval = GetRandomInterval();
            }
        }

        private void SpawnPipe()
        {
            if (pipePrefab == null)
            {
                Debug.LogWarning("SpawnManager: pipePrefab이 설정되지 않았습니다.");
                return;
            }

            // SpawnManager 오브젝트의 위치를 기준으로 y만 랜덤
            float randomY = Random.Range(minY, maxY);
            Vector3 spawnPos = new Vector3(transform.position.x, randomY, transform.position.z);
            Instantiate(pipePrefab, spawnPos, Quaternion.identity);
        }

        private float GetRandomInterval()
        {
            return Random.Range(minInterval, maxInterval);
        }
    }
}