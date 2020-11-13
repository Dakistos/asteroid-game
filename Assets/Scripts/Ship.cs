using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    // accelerate / decelerate
    readonly float speed = 10f;
    readonly float drag = 1;
    float thrust;

    // rotation
    readonly float rotationSpeed = 150f;
    float rotation;

    // fire
    public GameObject projectile;
    readonly float projectileSpeed = 4f;

    // firing frequency control
    readonly float fireRate = .30f;
    float nextFire;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = drag; // can be also set on inspector
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.state == GameManager.States.play)
        {
            Move();
            Turn();
            Fire();
        }

    }

    private void FixedUpdate()
    {
        Vector3 force = transform.TransformDirection(0, thrust * speed, 0);
        rb.AddForce(force);
    }

    void Move()
    {
        thrust = Input.GetAxisRaw("Vertical");
        if (thrust < 0) thrust = 0;
    }

    void Turn()
    {
        rotation = Input.GetAxisRaw("Horizontal");
        transform.Rotate(0, 0, rotation * Time.deltaTime * rotationSpeed * -1);
    }

    void Fire()
    {
        nextFire += Time.deltaTime;

        if (Input.GetButton("Fire1") && nextFire > fireRate)
        {
            GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0, projectileSpeed, 0);
            nextFire = 0;
        }
    }
}
