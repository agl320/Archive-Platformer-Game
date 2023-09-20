using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player_2_Collision : MonoBehaviour
{
    // Gun 

    int gunCooldown = 30, gunCooldownMax = 30;

    string gunType;

    [SerializeField] GameObject pistolPrefab;
    [SerializeField] GameObject riflePrefab;
    [SerializeField] GameObject sniperPrefab;
    GameObject gun;

    bool createGun = false, haveGun = true, throwGun = false;

    int ammo = 5;

    // Bullet 
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 30.0f, throwableSpeed = 5.0f;

    // Throwable 
    [SerializeField] GameObject throwablePrefab;

    // Other
    float gunDistance = 1.2f;
    float direction_multiplier = 1;
    Rigidbody2D rb;
    int health = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gun = Instantiate(sniperPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
        Destroy(gun.GetComponent<EdgeCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        gunCooldown -= 1;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction_multiplier = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction_multiplier = 1;
        }

        if (createGun == true)
        {
            if (gunType == "Pistol")
            {
                gun = Instantiate(pistolPrefab,
                    transform.position,
                    Quaternion.identity) as GameObject;

                gunDistance = 0.5f;
            }
            else if (gunType == "Rifle")
            {
                gun = Instantiate(riflePrefab,
                    transform.position,
                    Quaternion.identity) as GameObject;

                gunDistance = 1;
            }
            else if (gunType == "Sniper")
            {
                gun = Instantiate(sniperPrefab,
                    transform.position,
                    Quaternion.identity) as GameObject;

                gunDistance = 1.2f;
            }
            createGun = false;
            Destroy(gun.GetComponent<EdgeCollider2D>());
        }

        if (ammo > 0 && haveGun == true)
        {
            Debug.Log("Have ammo and gun");
            gun.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            gun.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gun.GetComponent<Rigidbody2D>().angularVelocity = 0;
            gun.GetComponent<Rigidbody2D>().transform.rotation = Quaternion.identity;
            gun.GetComponent<Rigidbody2D>().transform.position = new Vector3(transform.position.x + (direction_multiplier * gunDistance), transform.position.y, transform.position.z);
            throwGun = false;
        }
        else if (ammo <= 0 && haveGun == true)
        {
            Debug.Log("No ammo but gun");
            throwGun = true;
        }

        if (throwGun == true && haveGun == true)
        {
            Debug.Log("Threw gun");
            haveGun = false;
            throwGun = false;
            gun.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            gun.GetComponent<Rigidbody2D>().gravityScale = 2;
            gun.GetComponent<Rigidbody2D>().velocity = new Vector2(5 * direction_multiplier, 5);
            gun.GetComponent<Rigidbody2D>().AddTorque(-15 * direction_multiplier, ForceMode2D.Impulse);
            Destroy(this.gun, 5);
        }

        // Shooting
        if (Input.GetKey(KeyCode.Keypad2) && haveGun == true && gunCooldown <= 0)
        {
            gunCooldown = gunCooldownMax;
            ammo -= 1;
            // Prefab, position, rotation
            GameObject bullet = Instantiate(bulletPrefab,
              new Vector3(transform.position.x + (2 * direction_multiplier), transform.position.y, transform.position.z),
              Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * direction_multiplier, 0);
        }

        // Bomb
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            // Prefab, position, rotation
            GameObject throwable = Instantiate(throwablePrefab,
                transform.position, Quaternion.identity) as GameObject;
            throwable.GetComponent<Rigidbody2D>().velocity = new Vector2(rb.velocity.x, throwableSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Weapon" && ammo <= 0 && haveGun == false)
        {
            gunType = collision.gameObject.name.ToString();
            Debug.Log(gunType);

            Destroy(collision.gameObject);

            createGun = true;

            haveGun = true;

            if(collision.gameObject.name == "Pistol")
            {
                ammo = 10;
                bulletSpeed = 20.0f;
                gunCooldownMax = 25;
            }
            else if (collision.gameObject.name == "Rifle")
            {
                ammo = 15;
                bulletSpeed = 30.0f;
                gunCooldownMax = 5;
                
            }
            else if (collision.gameObject.name == "Sniper")
            {
                ammo = 5;
                bulletSpeed = 60.0f;
                gunCooldownMax = 50;
            }
        }

        if (collision.gameObject.tag == "Explosion")
        {
            // Calculate angle between collision point and player

            Vector2 dir = collision.contacts[0].point - new Vector2(transform.position.x,transform.position.y);
            dir = -dir.normalized;
            rb.velocity = new Vector2(dir.x * 100, dir.y * 100);
        }   

        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);
        }

    }
}
