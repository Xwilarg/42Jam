using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCamera, placementCamera;
    [SerializeField]
    private GameObject shopUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (playerCamera.activeInHierarchy)
            {
                playerCamera.SetActive(false);
                placementCamera.SetActive(true);
                shopUI.SetActive(true);
            }
            else
            {
                playerCamera.SetActive(true);
                placementCamera.SetActive(false);
                shopUI.SetActive(false);
            }
        }
    }
}
