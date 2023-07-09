using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform spawnPoint;
    private bool _nearDoor;
    [SerializeField] private bool spawnEnemy;
    private BoxCollider2D coll;
    private int howManyTimes;

    private void Start()
    {
        howManyTimes = 0;
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        OpenDoor();
    }

    void OpenDoor()
    {
        if (_nearDoor && Input.GetKey(KeyCode.E))
        {
            coll.enabled = false;

            if (howManyTimes < 1 && spawnEnemy == true)
                enemy.transform.position = spawnPoint.position;
                howManyTimes++;
        }
        else
        {
            coll.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _nearDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _nearDoor = false;
        }
    }
}
