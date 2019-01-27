﻿using UnityEngine;

namespace GGJ
{
    public class CheckPointTicket : MonoBehaviour
    {
        [SerializeField]
        int checkPointID;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) {
                if (PlayerController.CheckpointID <= checkPointID)
                    PlayerController.CheckpointID = checkPointID;
            }
        }
    }
}
