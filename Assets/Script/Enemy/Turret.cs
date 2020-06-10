using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    //[SerializeField] Transform shootPos;
    [SerializeField] float timeBtwShoot;
    [SerializeField] float startTimeBtwShoot;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootPos;
    public bool startShooting;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startShooting == true)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if(timeBtwShoot <= 0)
        {
            timeBtwShoot = startTimeBtwShoot;
            Instantiate(bullet, shootPos.transform.position, Quaternion.identity);
        }
        else
        {
            timeBtwShoot -= Time.deltaTime;
        }
    }
}
