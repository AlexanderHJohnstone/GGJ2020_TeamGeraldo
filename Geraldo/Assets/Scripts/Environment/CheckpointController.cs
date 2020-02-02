using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public void Activate()
    {
        PlayerMovementController._instance.SetCheckpoint(gameObject);
    }
}
