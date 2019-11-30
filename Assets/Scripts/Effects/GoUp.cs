using UnityEngine;

public class GoUp : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private void FixedUpdate()
    {
        transform.Translate(0f, speed, 0f);
    }
}
