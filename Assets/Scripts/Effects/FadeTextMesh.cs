using UnityEngine;

public class FadeTextMesh : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private TextMesh text;

    private void Start()
    {
        text = GetComponent<TextMesh>();
    }

    private void FixedUpdate()
    {
        float newAlpha = text.color.a - speed;
        if (newAlpha < 0f)
            Destroy(gameObject);
        else
            text.color = new Color(text.color.r, text.color.g, text.color.b, newAlpha);
    }
}
