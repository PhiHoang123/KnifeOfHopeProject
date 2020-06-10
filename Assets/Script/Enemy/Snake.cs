using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public GameObject[] wayPoints;
    public float moveSpeed;
    private Player player;

    int nextPoint = 0;
    float distToPoint; // distance between enemy and waypoint

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        //return distance from snake to wayPoint.
        distToPoint = Vector2.Distance(transform.position, wayPoints[nextPoint].transform.position);
        //move towards waypoint.
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[nextPoint].transform.position, moveSpeed * Time.deltaTime);

        //if distance from snake to wayPoint less than 0.1f,it will turn the way.
        if(distToPoint < 0.1f)
        {
            TakeTurn();
        }
    }

    void TakeTurn()
    {
        //access transform.rotate.z of snake to change the value of z.
        Vector3 currRot = transform.eulerAngles;
        currRot.z += wayPoints[nextPoint].transform.eulerAngles.z;
        transform.eulerAngles = currRot;
        ChooseNextPoint();
    }

    void ChooseNextPoint()
    {
        nextPoint++;

        if(nextPoint == wayPoints.Length)
        {
            nextPoint = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Timestop>().StopTime(0.5f);
            StartCoroutine(WaitForKnockBack());
        }
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


}
