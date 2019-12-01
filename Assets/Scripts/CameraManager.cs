using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCamera, placementCamera;
    [SerializeField]
    private GameObject shopUI;

    public void EnableShop()
    {
        playerCamera.SetActive(false);
        placementCamera.SetActive(true);
    }

    public void DisableShop()
    {
        playerCamera.SetActive(true);
        placementCamera.SetActive(false);
    }
}
