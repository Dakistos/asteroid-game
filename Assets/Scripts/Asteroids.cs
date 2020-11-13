using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    readonly float initialSpeed = 5f;
    Vector2 speed;

    readonly float initialRotation = 100f;
    float rotation;

    public int points = 10;
    public GameObject[] divisions;

    Rigidbody2D rb;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        rotation = Random.Range(-initialRotation, initialRotation);

        // x / y speed determination
        float x = Random.Range(-initialSpeed, initialSpeed);
        float y = Random.Range(-initialSpeed, initialSpeed);
        speed = new Vector2(x,y);

        // applicate velocity
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = speed;
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.KillPlayer();
        }
        else if (collision.CompareTag("Bullet"))
        {
            // destroy bullet and asteroid
            Destroy(collision.gameObject);
            Destroy(gameObject);

            // divide asteroids
            foreach (GameObject enemy in divisions)
            {
                Instantiate(enemy, transform.position, Quaternion.identity);
            }

            // score
            gameManager.AddScore(points);
        }
    }
}
