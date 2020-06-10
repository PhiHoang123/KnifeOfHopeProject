using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float speed;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        //get player.
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        
        //get direction from player to bullet
        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        //move to player position
        rb.velocity = new Vector2(moveDir.x, moveDir.y);
        Destroy(this.gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
