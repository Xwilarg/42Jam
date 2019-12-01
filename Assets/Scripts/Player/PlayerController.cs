using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Player Speed")]
    private float speed;

    [SerializeField]
    private GameObject iceSpearPrefab;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Character charac;

    private const float iceReloadRef = 5f;
    private const float iceForce = 20f;
    private const int iceDamage = 40;
    private const int avoidPlayerLayer = ~(1 << 8);
    private Vector3 initialPos;

    private float iceReloadTimer;

    private GameObject swordSkill;
    private GameObject fireBallSkill;
    private GameObject iceSpearSkill;
    private GameObject teleportSkill;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        charac = GetComponent<Character>();
        iceReloadTimer = 0f;
        initialPos = transform.position;

        GameObject skillBar = GameObject.Find("SkillBar");
        foreach (Transform child in skillBar.transform)
        {
            if (child.name == "Sword")
            {
                swordSkill = child.gameObject;
                foreach (Transform text in swordSkill.transform)
                {
                    if (text.name == "ReloadTime")
                    {
                        text.gameObject.GetComponent<Text>().text = charac.getSwordReload().ToString() + "s";
                    }
                }
            }
            else if (child.name == "FireBall")
            {
                fireBallSkill = child.gameObject;
                foreach (Transform text in fireBallSkill.transform)
                {
                    if (text.name == "ReloadTime")
                    {
                        text.gameObject.GetComponent<Text>().text = charac.getFireReload().ToString() + "s";
                    }
                }
            }
            else if (child.name == "IceSpear")
            {
                iceSpearSkill = child.gameObject;
                foreach (Transform text in iceSpearSkill.transform)
                {
                    if (text.name == "ReloadTime")
                    {
                        text.GetComponent<Text>().text = iceReloadRef.ToString() + "s";
                    }
                }
            }
            else if (child.name == "Teleport")
            {
                teleportSkill = child.gameObject;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;

        // Set player orientation
        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
        {
            if (rb.velocity.x > 0f)
                sr.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            else if (rb.velocity.x < 0f)
                sr.transform.rotation = Quaternion.identity;
        }
        else
        {
            if (rb.velocity.y > 0f)
                sr.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            else if (rb.velocity.y < 0f)
                sr.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
    }

    private void Update()
    {
        // Attacks
        if (Input.GetKeyDown(KeyCode.Z)) // Sword attack
        {
            StartCoroutine(changeImageColor(swordSkill, charac.getSwordReload()));
            charac.SwordAttack(-sr.transform.right, avoidPlayerLayer);
        }
        else if (Input.GetKeyDown(KeyCode.X) && iceReloadTimer < 0f) // Ice spear
        {
            StartCoroutine(changeImageColor(iceSpearSkill, iceReloadRef));
            GameObject go = Instantiate(iceSpearPrefab, transform.position, Quaternion.identity);
            go.transform.rotation = transform.rotation;
            go.GetComponent<Rigidbody2D>().AddForce(-sr.transform.right * iceForce, ForceMode2D.Impulse);
            go.GetComponent<Bullet>().SetDamage(iceDamage);
            iceReloadTimer = iceReloadRef;
        }
        else if (Input.GetKeyDown(KeyCode.C)) // Fireball
        {
            StartCoroutine(changeImageColor(fireBallSkill, charac.getFireReload()));
            charac.Fireball(-sr.transform.right);
        }
        else if (Input.GetKeyDown(KeyCode.V)) // Teleportation to spawn
        {
            transform.position = initialPos;
        }

        // Reload time
        iceReloadTimer -= Time.deltaTime;
    }

    private IEnumerator changeImageColor(GameObject obj, float time)
    {
        Color color = obj.GetComponent<Image>().color;
        obj.GetComponent<Image>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds(time);
        obj.GetComponent<Image>().color = color;
    }
}
