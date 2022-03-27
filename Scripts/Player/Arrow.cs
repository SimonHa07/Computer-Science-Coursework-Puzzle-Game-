using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    float shot_speed = 10f;
    int damage = 20;
    public Rigidbody2D arrow;
    public GameObject impactEffect;
    // Start is called before the first frame update
    void Start()
    {
        arrow.velocity = transform.up * shot_speed;
        //gameObject.GetComponent<NetworkObject>().Spawn(); // added this
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 2f);
    }
    void OnTriggerEnter2D(Collider2D hitinfo)
    {
        if (hitinfo.tag != "Player" && hitinfo.tag != "Arrow" && hitinfo.tag != "Enemy")
        {
            Instantiate(impactEffect, transform.position + (transform.up / 1.2f), transform.rotation);
            //effect.GetComponent<NetworkObject>().Spawn();
            Destroy(gameObject, 0.4f);
        }
        if (hitinfo.tag == "Enemy")
        {
            Enemy enemy = hitinfo.GetComponent<Enemy>();
            if (!enemy.invincible)
            {
                Instantiate(impactEffect, transform.position + (transform.up / 1.2f), transform.rotation);
                Vector2 dir = transform.position - enemy.transform.position;
                enemy.TakeDamage(damage, dir);
                Destroy(gameObject);
                Debug.Log("hit");
            }
        }
    }
}
