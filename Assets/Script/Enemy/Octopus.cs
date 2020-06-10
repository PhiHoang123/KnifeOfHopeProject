using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{

    [SerializeField] float speed;
    private GameObject target;
    [SerializeField] float lineOfSight;

    [SerializeField] float shootingRange;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootPoint;
    [SerializeField] float fireRate;
    [SerializeField] float nextFireTime;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);//get distance to player

        /* check if distanceToPlayer < lineofSine and distanceToPlayer > shootingRange , enemy will move toward player.
        if distanceToPlayer < shooting Range or lineofSight it will stop*/

        if (distanceToPlayer < lineOfSight && distanceToPlayer > shootingRange) 
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        //shoot player
        if (distanceToPlayer <= shootingRange && nextFireTime < Time.time)
        {
            
            Instantiate(bullet, shootPoint.transform.position, Quaternion.identity);
            
            //check time to limit the shooting
            nextFireTime = Time.time + fireRate;
            Debug.Log(Time.time);
        }

        //enemy will move back if player move close them.
        if(distanceToPlayer <= shootingRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, -speed * Time.deltaTime);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
