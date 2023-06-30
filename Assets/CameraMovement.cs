using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float baseSpeed = 2f;
    private int maxZoom = 160;
    private int minZoom = 5;


    private void Start()
    {
        Camera.main.orthographicSize = 20;
    }

    private void Update()
    {
        float speed = baseSpeed * Camera.main.orthographicSize;

        if(Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,
                                    Camera.main.transform.position + new Vector3(-1, 0, 0), speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,
                                    Camera.main.transform.position + new Vector3(1, 0, 0), speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,
                                    Camera.main.transform.position + new Vector3(0, -1, 0), speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,
                                    Camera.main.transform.position + new Vector3(0, 1, 0), speed * Time.deltaTime);
        }

        if(Input.mouseScrollDelta.y > 0 && Camera.main.orthographicSize > minZoom && !MouseOverEditMenu.isOverMenu)
        {
            if((Camera.main.orthographicSize /= 1.1f) < minZoom)
            {
                Camera.main.orthographicSize = minZoom;
            }
        } else if(Input.mouseScrollDelta.y < 0 && Camera.main.orthographicSize < maxZoom && !MouseOverEditMenu.isOverMenu)
        {
            if ((Camera.main.orthographicSize *= 1.1f) > maxZoom)
            {
                Camera.main.orthographicSize = maxZoom;
            }
        }



    }
}
