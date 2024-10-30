using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        if (player != null )
        {
            transform.position = player.position + offset;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            //Calculate the desired position based on player's position
            Vector3 desiredPosition = player.position + offset;
            //Clamp the camera position to keep it within bounds
            ClampCameraPosition(ref desiredPosition);
            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            //Debug log
            Debug.Log($"Desired Position: {desiredPosition}");
        }
    }

    void ClampCameraPosition(ref Vector3 desiredPosition)
    {
        //Get the camera bounds
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        //Calculate camera boundaries (Adjust based on the stage)
        float minX = -6.5f + cameraWidth / 2;
        float maxX = 22f - cameraWidth / 2;
        float minY = -5f + cameraHeight / 2;
        float maxY = 6f - cameraHeight / 2;

        //Clamp the desired position
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
    }

}