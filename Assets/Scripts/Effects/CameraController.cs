using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float minFoV = 60;
    private const float maxFoV = 150;
    private const float speed = 1f;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.P))
        {
            Camera.main.fieldOfView -= speed;
            if (Camera.main.fieldOfView <= minFoV)
                Camera.main.fieldOfView = minFoV;
        }
        else
        if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.M))
        {
            Camera.main.fieldOfView += speed;
            if (Camera.main.fieldOfView >= maxFoV)
                Camera.main.fieldOfView = maxFoV;
        }
    }
}
