using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Script controls the camera that provides view of the scene. Camera follows the player given them view of the area in front of them
    /// </summary>
    /// 

    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;

    private float lookAhead;

    private void Update()
    {
        //Camera focuses on the player and moves slightly ahead of the player in the direction the player is facing
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        //Camera focuses on the area ahead of the player
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed); 
    }
}
