using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField]
    private float timer;

    private void Start()
    {
        Destroy(gameObject, timer);
    }
}
