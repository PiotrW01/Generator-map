using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float baseSpeed = 2f;

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

        if(Input.mouseScrollDelta.y > 0 && Camera.main.orthographicSize > 5)
        {
            if((Camera.main.orthographicSize /= 1.1f) < 5f)
            {
                Camera.main.orthographicSize = 5;
            }
        } else if(Input.mouseScrollDelta.y < 0 && Camera.main.orthographicSize < 100)
        {
            Camera.main.orthographicSize *= 1.1f;
        }



    }
}
