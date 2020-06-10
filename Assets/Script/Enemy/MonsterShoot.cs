using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShoot : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] float startTimeBtwShoot;
    private float timeBtwShoot;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBtwShoot <= 0)
        {
            timeBtwShoot = startTimeBtwShoot;
            Instantiate(bullet, shootPos.position, Quaternion.identity);
        }
        else
        {
            timeBtwShoot -= Time.deltaTime;
        }
    }
}
