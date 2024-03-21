using UnityEngine;

// Handles scrolling the map left/right
public class CameraController : MonoBehaviour
{
    // Variables
    [SerializeField] float scrollSpeed = 4;
    [SerializeField] float leftBound = -15f; // The world coordinates of most left the camera can display
    [SerializeField] float rightBound = 15f; // The world coordinates of most right the camera can display
    
    // Update is called once per frame
    void Update()
    {
        float leftPosition = Camera.main.ScreenToWorldPoint(Vector2.zero).x; // The world coordinates of the left side of screen
        float rightPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x; // The world coordinates of the right side of screen
        if (Input.GetKey(KeyCode.LeftArrow)) // Moves the camera left when left arrow is held down
        {            
            if (leftPosition > leftBound)
            {
                transform.Translate(-scrollSpeed * Time.deltaTime, 0, 0);
            }
              
            
        } else if(Input.GetKey(KeyCode.RightArrow))
        {

            if (rightPosition < rightBound)
            {
                transform.Translate(scrollSpeed * Time.deltaTime, 0, 0);
            }
        
        }
    }
}
