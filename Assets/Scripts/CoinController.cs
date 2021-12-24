using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float timeBetweenPositions;
    
    private Vector3 begin;
    private float elapsedTime;
    private Vector3 tempEndPosition;

    private void Start()
    {
        begin = transform.position;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        float percentageComplete = elapsedTime / timeBetweenPositions;

        transform.position = Vector3.Lerp(begin, endPosition, Mathf.SmoothStep(0, 1, percentageComplete));
        
       if(transform.position == endPosition)
        {
            elapsedTime = 0f;
            tempEndPosition = endPosition;
            endPosition = begin;
            begin = tempEndPosition;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().SetHasCoin(true);
            Destroy(gameObject);
        }
    }
}
