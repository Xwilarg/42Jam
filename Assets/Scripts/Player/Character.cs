using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    [Tooltip("Damage Display Prefab")]
    private GameObject damagePrefab;

    private int or = 0;
    private float swordReloadTimer;
    private const float swordRange = 1f;
    private const int swordDamage = 10;
    private const float swordReloadRef = 1f;

    private void Start()
    {
        swordReloadTimer = 0f;
    }

    private void Update()
    {
        swordReloadTimer -= Time.deltaTime;
    }

    public void SwordAttack(Vector3 left, int layer)
    {
        if (swordReloadTimer < 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, left, swordRange, layer);
            hit.collider?.GetComponent<Character>()?.LooseHp(swordDamage);
            swordReloadTimer = swordReloadRef;
        }
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
    {
        or += value;
        return or;
    }
}
