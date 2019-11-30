using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowIA : MonoBehaviour
{
    public float triggerDistance = 5.0f;
    public float speed = 1.0f;
    private Rigidbody2D rb;
    private Collider2D collider2d;
    private bool startMoving = false;
    private GameObject target = null;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    void Update()
    {
        float minDistance = triggerDistance;
        GameObject[] heros = GameObject.FindGameObjectsWithTag("Hero");
        for (int i = 0; i < heros.Length; i++) {
            float distance = collider2d.Distance(heros[i].GetComponent<Collider2D>()).distance;
            if (distance <= minDistance) {
                minDistance = distance;
                target = heros[i];
            }
        }

        if (target != null) {
            Vector2 velocity = target.transform.position - transform.position;
            velocity.Normalize();
            velocity *= speed;
            rb.velocity = velocity;
        }
    }
}
