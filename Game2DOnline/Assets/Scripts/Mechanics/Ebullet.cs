using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ebullet : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector2 tager;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tager = new Vector2(player.position.x, player.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, tager, speed * Time.deltaTime);
        if(transform.position.x == tager.x && transform.position.y==tager.y)
        {
            desTroyBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {

        }
    }

    void desTroyBullet()
    {
        Destroy(gameObject);
    }
}
