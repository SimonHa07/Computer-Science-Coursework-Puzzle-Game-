using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int health = 25;
    public float damage = 1;
    public GameObject DeathEffect;
    public bool invincible = false;
    int flash_counter = 0;
    SpriteRenderer this_enemy;
    // Start is called before the first frame update
    void Start()
    {
        this_enemy = gameObject.GetComponent<SpriteRenderer>(); //gets the sprite component from the current object
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Player")
        {
            Player_Manager player = hitInfo.GetComponent<Player_Manager>();
            if (!player.invincible)
            {
                player.TakeDamage(damage);
            }
        }
    }
    public void TakeDamage(int damage, Vector2 direction)
    {
        health -= damage;
        StartCoroutine(Flash(4)); // flashes 2 times total, 2 non colour, 2 colour
        if (health <= 0)
        {
            Death(); //the enemy dies
        }
        Knockback(direction);
    }
    IEnumerator Flash(int count)
    {
        float value = 1f;
        invincible = true;
        flash_counter += 1;
        while (flash_counter < count) // this controls how many times it flashes
        {
            this_enemy.color = new Color(this_enemy.color.r, this_enemy.color.g, this_enemy.color.b, 1 - value);
            yield return new WaitForSeconds(0.15f);
            value = -value;
            flash_counter += 1;
        }
        this_enemy.color = new Color(this_enemy.color.r, this_enemy.color.g, this_enemy.color.b, 1f);
        flash_counter = 0;
        invincible = false;
    }
    public void Knockback(Vector2 direction)
    {
        transform.position = new Vector3(transform.position.x - direction.x, transform.position.y - direction.y, transform.position.z);
    }
    void Death()
    {
        Instantiate(DeathEffect, transform.position, transform.rotation);
        //random chance to drop an arrow
        Destroy(gameObject);
    }
}
