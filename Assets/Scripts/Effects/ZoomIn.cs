using UnityEngine;

public class ZoomIn : MonoBehaviour
{
    [SerializeField]
    private float timeBeforeStart;
    [SerializeField]
    [Tooltip("Tag of the object to aim")]
    private string toAim;
    private Transform toAimGo;

    private const float speed = 2f;
    private const float minFoV = 40f;
    private float baseFov;
    private float maxFov;
    private Vector3 basePos;
    private int index;

    private void Start()
    {
        baseFov = Camera.main.fieldOfView;
        basePos = transform.position;
        maxFov = Camera.main.fieldOfView;
        toAimGo = GameObject.FindGameObjectWithTag(toAim).transform;
        index = 0;
    }

    private void FixedUpdate()
    {
        timeBeforeStart -= Time.deltaTime;
        if (timeBeforeStart < 0f)
        {
            Camera.main.fieldOfView -= speed;
            index += (int)speed;
            transform.position = Vector2.Lerp(basePos, toAimGo.position, 1 / (maxFov - (minFoV - 1)) * index);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
            if (Camera.main.fieldOfView <= minFoV)
                Destroy(this);
        }
    }
}
