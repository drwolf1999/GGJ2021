using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform shootPos;

    public bool isAwaken = false;

    public float speed = 1.5f;
    public float rotateSpeed = 5.0f;
    public string bulletTag = "";
    
    private Vector2 newPosition;

    private float timer;
    private float waitingTime;

    ObjectPooler objectPooler;

    void Start()
    {
        isAwaken = false;
        ChangePosition();

        timer = 0.0f;
        waitingTime = 2.0f;

        objectPooler = ObjectPooler.Instance;
    }

    void Update()
    {
        if(isAwaken)
        {
            if(Vector2.Distance(transform.position, newPosition) < 1)
            {
                ChangePosition();
            }
            transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * speed);
            LookAtPlayer();

            timer += Time.deltaTime;
            if (timer > waitingTime)
            {
                Attack();
                timer = 0.0f;
            }
        }
    }

    private void ChangePosition()
    {
        newPosition = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
    }

    private void Attack()
    {
        objectPooler.SpawnFromPool(bulletTag, shootPos.position, Quaternion.identity);

        //Put Animation
    }

    private void LookAtPlayer()
    {
        transform.right = playerTransform.position - transform.position;
    }

    public void AwakeEnemy()
    {
        isAwaken = true;
    }

}
