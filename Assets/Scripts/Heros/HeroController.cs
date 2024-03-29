﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroController : MonoBehaviour
{
    [SerializeField]
    private TextMesh infos;

    [SerializeField]
    private Sprite mageSprite;

    private SpriteRenderer sr; // Move sr.transform instead of transform
    private Rigidbody2D rb;
    private Character charac;
    private Transform player;
    private HeroClass heroClass;
    private string heroName;
    private Node[] path;
    private Node objective; // Destination the heroes need to reach
    private int index;

    private const float minDistNode = .5f;
    private float speed = 4f;
    private const int avoidHeroLayer = ~(1 << 10);
    private const int avoidPlayerLayer = ~(1 << 8 | 1 << 11 | 1 << 10);

    private Transform target;

    public void Init(string nameValue, HeroClass heroValue, Node[] pathValue)
    {
        heroName = nameValue;
        heroClass = heroValue;
        path = pathValue;
        charac = GetComponent<Character>();
        infos.text = "Name: " + heroName + "\nClass: " + heroClass + "\nHP: " + charac.GetHp();
        sr = GetComponentInChildren<SpriteRenderer>();
        if (heroClass == HeroClass.Mage)
        {
            sr.sprite = mageSprite;
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        objective = GetClosestNode<Node>(player.position, "Node");
        rb = GetComponent<Rigidbody2D>();
        index = 1;
        charac.GainOr(Random.Range(10, 21));
    }

    private void FixedUpdate()
    {
        if (infos.gameObject.activeInHierarchy)
            infos.text = "Name: " + heroName + "\nClass: " + heroClass + "\nHP: " + charac.GetHp();
        Transform t = null;
        if (heroClass == HeroClass.Mage) // Can see player, battle mode
        {
            if (!Physics2D.Linecast(transform.position, player.position, avoidPlayerLayer))
                t = player;
            else
            {
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (!Physics2D.Linecast(transform.position, go.transform.position, avoidPlayerLayer))
                    {
                        t = go.transform;
                        break;
                    }
                }
            }
            if (t != null)
            {
                var finalPos = t.position - transform.position;
                charac.Fireball(-rb.transform.right);
            }
        }
        if (target != null)
        {
            rb.velocity = Vector2.zero;
            if (heroClass == HeroClass.Warrior)
                charac.SwordAttack(-rb.transform.right, avoidHeroLayer);
        }
        else if (t == null)
        {
            int x = 0, y = 0;
            var node = path[index];
            Vector2? dir = null;

            if (transform.position.x + minDistNode < node.transform.position.x)
            {
                x = 1;
                dir = Vector2.right;
            }
            else if (transform.position.x - minDistNode > node.transform.position.x)
            {
                x = -1;
                dir = Vector2.left;
            }
            if (transform.position.y + minDistNode < node.transform.position.y)
            {
                y = 1;
                dir = Vector2.up;
            }
            else if (transform.position.y - minDistNode > node.transform.position.y)
            {
                y = -1;
                dir = Vector2.down;
            }

            if (x == 0 && y == 0)
            {
                if (index < path.Length)
                    index++;
                rb.velocity = Vector2.zero; 
            }
            else
            {
                if (dir == null || !Physics.Linecast((Vector2)transform.position + dir.Value, (Vector2)transform.position + dir.Value * 2)) {
                    rb.velocity = new Vector2(x, y) * speed;
                }
                else {
                    rb.velocity = Vector2.zero;
                }
            }
        }
        float spriteAngle = 180.0f;
        if (heroClass == HeroClass.Warrior) {
            spriteAngle = -90.0f;
        }
        sr.transform.rotation = Quaternion.Euler(0f, 0f, -(Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.x, rb.velocity.y)) + spriteAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
            target = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
            target = null;
    }

    private void OnMouseEnter()
    {
        infos.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        infos.gameObject.SetActive(false);
    }

    public static T GetClosestNode<T>(Vector2 pos, string tag)
    {
        float dist = float.MaxValue;
        GameObject closest = null;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag))
        {
            float currDist = Vector2.Distance(go.transform.position, pos);
            if (closest == null || currDist < dist)
            {
                closest = go;
                dist = currDist;
            }
        }
        return closest.GetComponent<T>();
    }

    public enum HeroClass
    {
        Warrior,
        //Healer,
        //Rogue,
        //Archer,
        Mage
    }

    public void MudSlow(float time)
    {
        StartCoroutine(slowTime(time));
    }

    IEnumerator slowTime(float time)
    {
        speed = 2;
        yield return new WaitForSeconds(time);
        speed = 4;
    }
}
