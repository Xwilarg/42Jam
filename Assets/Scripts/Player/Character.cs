using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    [Tooltip("Damage Display Prefab")]
    private GameObject damagePrefab;

    private int or = 0;

    private void Start()
    {
    }

    public void LooseHp(int value)
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), -1f);
        GameObject go = Instantiate(damagePrefab, pos, Quaternion.identity);
        go.GetComponent<TextMesh>().text = "-" + value;
        hp -= value;
        if (hp <= 0)
            Destroy(gameObject);
    }

    public int GetOr()
        => or;

    public int GainOr(int value)
        => or += value;
}
