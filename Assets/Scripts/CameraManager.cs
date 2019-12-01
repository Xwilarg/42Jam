using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCamera, placementCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (playerCamera.activeInHierarchy)
            {
                playerCamera.SetActive(false);
                placementCamera.SetActive(true);
            }
            else
            {
                playerCamera.SetActive(true);
                placementCamera.SetActive(false);
            }
        }
    }
}
