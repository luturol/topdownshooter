using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision arrow tag " + other.gameObject.tag);
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().LoseHitPoints();                        
            Destroy(gameObject);
        }
        
        if(other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
