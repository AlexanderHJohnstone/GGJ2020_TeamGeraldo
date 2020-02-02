using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleController : MonoBehaviour
{
    public PlayerMovementController _movementController;

    private void OnTriggerEnter(Collider other)
    {
        _movementController.AttachGrapple(new Vector3(transform.position.x, transform.position.y, 0f));
        if (other.gameObject.TryGetComponent<NailController>(out var nailController))
        {
            nailController._cmCam.Priority = 10;
            CameraController._instance._cmCamMain.Priority = 0;
        }

        if (other.gameObject.TryGetComponent<ScrewController>(out var screwController))
        {
            screwController.OnPlayerLatch();
        }

        if(other.gameObject.TryGetComponent<CheckpointController>(out var checkpointController))
        {
            checkpointController.Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<NailController>(out var nailController))
        {
            nailController._cmCam.Priority = 0;
            CameraController._instance._cmCamMain.Priority = 10;
        }

        if (other.gameObject.TryGetComponent<ScrewController>(out var screwController))
        {
            screwController.OnPlayerDetach();
        }
    }
}
