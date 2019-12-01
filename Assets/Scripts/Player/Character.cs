using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int hp;
    private int maxHp;

    [SerializeField]
    [Tooltip("Damage Display Prefab")]
    private GameObject damagePrefab;

    [SerializeField]
    private GameObject fireballPrefab;

    private int or = 0;
    private float swordReloadTimer;
    private const float swordRange = 1f;
    private const int swordDamage = 10;
    private const float swordReloadRef = 1f;
    private GameObject healthBar = null;
    private const float fireReloadRef = 10f;
    private const float fireForce = 10f;
    private const int fireDamage = 25;
    private float fireReloadTimer;

    private Character player;

    private void Start()
    {
        swordReloadTimer = 0f;
        fireReloadTimer = 0f;
        maxHp = hp;
        healthBar = Resources.Load<GameObject>("HealthBar");
        healthBar = Instantiate(healthBar, new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), Quaternion.identity, transform);
        foreach (Transform child in healthBar.transform) {
            if (child.name == "Foreground") {
                healthBar = child.gameObject;
            }
        }
        if (CompareTag("Hero"))
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        else
            player = null;
    }

    private void Update()
    {
        swordReloadTimer -= Time.deltaTime;
        fireReloadTimer -= Time.deltaTime;
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K) && gameObject.tag == "Player")
        {
            LooseHp(maxHp);
        }
#endif
    }

    public void SwordAttack(Vector3 left, int layer)
    {
        if (swordReloadTimer < 0f)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f, left, swordRange, layer);
            hit.collider?.GetComponent<Character>()?.LooseHp(swordDamage);
            swordReloadTimer = swordReloadRef;
        }
    }

    public void Fireball(Vector3 left)
    {
        if (fireReloadTimer < 0f)
        {
            GameObject go = Instantiate(fireballPrefab, transform.position + left / 2f, Quaternion.identity);
            go.transform.rotation = transform.rotation;
            go.GetComponent<Rigidbody2D>().AddForce(left * fireForce, ForceMode2D.Impulse);
            go.GetComponent<Bullet>().SetDamage(fireDamage);
            fireReloadTimer = fireReloadRef;
        }
    }

    public void LooseHp(int value)
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), -1f);
        GameObject go = Instantiate(damagePrefab, pos, Quaternion.identity);
        go.GetComponent<TextMesh>().text = "-" + value;

        healthBar.transform.localScale = new Vector3(Mathf.InverseLerp(0, maxHp, hp), healthBar.transform.localScale.y, healthBar.transform.localScale.z);

        hp -= value;
        if (hp <= 0) {
            if (gameObject.tag == "Player") {
                GameObject.Find("Dungeon").GetComponent<GameOver>().Over();
                return;
            }
            if (player != null)
            {
                player.GainOr(or);
                GameObject.Find("GoldText").GetComponent<Text>().text = "Gold: " + player.GetOr();
            }
            Destroy(gameObject);
        }
    }

    public int GetHp()
        => hp;

    public int GetOr()
        => or;

    public int GainOr(int value)
    {
        or += value;
        return or;
    }
}
