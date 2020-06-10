using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureFollow : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lineOfSight;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < lineOfSight)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Timestop>().StopTime(0.5f);
        StartCoroutine(WaitForKnockBack());
    }


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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
    }
}
