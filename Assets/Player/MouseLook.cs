using UnityEngine;

public class MouseLook : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] float sensitivityX = 8f;
    [SerializeField] float sensitivityY = 0.5f;
    float MouseX, MouseY;

    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f;
    float xRotation = 0f;


    private void Update ()
    {
        transform.Rotate(Vector3.up, MouseX * Time.deltaTime);

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
    }

    public void RecieveInput (Vector2 mouseInput) 
    { 
        MouseX = mouseInput.x * sensitivityX; 
        MouseY = mouseInput.y * sensitivityY;
    }
}
