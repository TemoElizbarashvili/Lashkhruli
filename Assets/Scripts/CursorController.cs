using UnityEngine;

public class CursorController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Update cursor position to follow the mouse
        var mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Set a distance from the camera
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);

        // Optionally, you can add functionality to unlock the cursor with a key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
