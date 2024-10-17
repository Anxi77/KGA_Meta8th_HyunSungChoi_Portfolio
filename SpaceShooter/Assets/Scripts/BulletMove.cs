using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float velocity = 10f;
    public float lifeTime = 2f;
    private GameObject player;

    private Vector3 direction;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    public void Start()
    {
    
        Vector3 playerDirection = player.transform.up;
        direction = playerDirection.normalized;

        // �Ѿ��� ȸ���� �÷��̾��� ȸ���� ��ġ��Ŵ
        transform.rotation = player.transform.rotation * Quaternion.Euler(-90f, 0f, 0f);

        Destroy(gameObject, lifeTime);
    }


    private void Update()
    {
        
        transform.Translate(direction * velocity * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DestroyImmediate(gameObject);
    }
}