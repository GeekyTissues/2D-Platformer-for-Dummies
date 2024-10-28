using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firepoint : MonoBehaviour
{
    /// <summary>
    /// Script for the firepoint of the arrows for the ranged enemy. Ensures the firepoint is facing the same direction as the ranged enemy
    /// </summary>


    [SerializeField] private Transform enemy;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = enemy.localScale;
    }
}
