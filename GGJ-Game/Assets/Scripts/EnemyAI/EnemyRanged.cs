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

    [SerializeField] Combat enemyCombat;

    void Start()
    {
        isAwaken = false;
        ChangePosition();

        timer = 0.0f;
        waitingTime = 2.0f;

        objectPooler = ObjectPooler.Instance;

        //enemyCombat = gameObject.GetComponent<Combat>();
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
                Debug.Log(Time.time);
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
        GameObject bullet = objectPooler.SpawnFromPool(bulletTag, shootPos.position, shootPos.rotation);
        bullet.GetComponent<EnemyBulletMove>().SetCombatStats(enemyCombat);

        //Put Animation
    }

    private void LookAtPlayer()
    {
        transform.up = playerTransform.position - transform.position;
    }

    public void AwakeEnemy()
    {
        isAwaken = true;
    }

}
