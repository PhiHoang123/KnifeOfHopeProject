using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rb;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.down * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Timestop>().StopTime(0.5f);
            StartCoroutine(WaitForKnockBack());

        }

        if (collision.gameObject.CompareTag("ground"))
        {
            Destroy(this.gameObject);
        }
    }

    //waiting when time is resume, starting knockback
    IEnumerator WaitForKnockBack()
    {

        while (Time.timeScale != 1.0f)
        {
            yield return null;
        }
        player.knockbackCount = player.knockbackLength;


        if (player.transform.position.x < transform.position.x)
        {
            player.knockbackFromRight = true;
        }
        else
        {
            player.knockbackFromRight = false;
        }
        Destroy(this.gameObject);
    }
}
