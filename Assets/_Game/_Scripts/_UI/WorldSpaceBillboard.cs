using UnityEngine;

public class WorldSpaceBillboard : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        transform.rotation =
            Quaternion.LookRotation(
                transform.position -
                cameraTransform.position
            );
    }
}