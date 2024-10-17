using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SpaceShooter
{
    public class PlayerScript : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float boundaryXMinSize = -15.22f;
        public float boundaryXMaxSize = 15.22f;
        public float boundaryYMinSize = -18.62f;
        public float boundaryYMaxSize = 18.62f;
        public GameObject gameOverMessage;
        public Renderer target;
        public float rotationSpeed = 5f; // 회전 속도 조절을 위한 변수 추가

        private void Awake()
        {
            target = GetComponentInChildren<Renderer>();
        }

        void Update()
        {
            PlayerMove();
            PlayerRotate();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                //print("Game Over");
                //GameOver();
                Destroy(collision.gameObject);
            }
        }

        public void GameOver()
        {
            gameOverMessage.SetActive(true);
            Time.timeScale = 0;
        }

        public void PlayerMove()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            // 이동 처리
            Vector3 movement = new Vector3(x, y, 0);
            transform.position += movement * moveSpeed * Time.deltaTime;

            //// X축 경계 처리
            //transform.position = new Vector3(
            //    Mathf.Clamp(transform.position.x, boundaryXMinSize, boundaryXMaxSize),
            //    transform.position.y,
            //    transform.position.z
            //);

            //// Y축 경계 처리
            //transform.position = new Vector3(
            //    transform.position.x,
            //    Mathf.Clamp(transform.position.y, boundaryYMinSize, boundaryYMaxSize),
            //    transform.position.z
            //);
        }

        public void PlayerRotate()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            if (x != 0 || y != 0) // 입력이 있을 때만 회전
            {
                // 입력 방향을 기반으로 회전 각도 계산
                float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg - 90f;
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

                //transform.rotation = transform.rotation * Quaternion.Euler(0f,-90f, 0f); // 회전회오리
                // 부드러운 회전 적용
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
