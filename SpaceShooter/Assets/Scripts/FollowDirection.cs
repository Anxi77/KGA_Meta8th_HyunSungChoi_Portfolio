using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDirection : MonoBehaviour
{
    public GameObject Player;

    private void Update()
    {
        Vector3 directionToPlayer = Player.transform.position - transform.position;
        directionToPlayer.z = 0; // Ignore z-axis difference for 2D player

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);
        transform.rotation = targetRotation;
    }
}