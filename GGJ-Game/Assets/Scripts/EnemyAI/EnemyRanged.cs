using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform shootPos;
    [SerializeField] Rigidbody2D rb;

    public bool isAwaken = false;

    public string bulletTag = "";

    public float rotateSpeed;

    private float timer;
    private float waitingTime;

    public float accelerationTime = 2f;
    public float maxSpeed = 7f;
    private Vector2 movement;
    private float moveTimeLeft;

    ObjectPooler objectPooler;

    [SerializeField] Combat enemyCombat;

    void Start()
    {
        isAwaken = false;

        timer = 0.0f;
        waitingTime = 2.0f;

        objectPooler = ObjectPooler.Instance;

        //enemyCombat = gameObject.GetComponent<Combat>();
    }

    void Update()
    {
        if(isAwaken)
        {
            LookAtPlayer();
            timer += Time.deltaTime;
            if (timer > waitingTime)
            {
                Debug.Log(Time.time);
                Attack();
                timer = 0.0f;
            }

            moveTimeLeft -= Time.deltaTime;
            if(moveTimeLeft <= 0)
            {
                movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                moveTimeLeft = accelerationTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isAwaken)
        {
            //rb.AddForce(movement * maxSpeed);
            rb.velocity = movement * maxSpeed;
        }
    }

    private void Attack()
    {
        GameObject bullet = objectPooler.SpawnFromPool(bulletTag, shootPos.position, shootPos.rotation);
        bullet.GetComponent<EnemyBulletMove>().SetCombatStats(enemyCombat);

        //Put Animation
    }

    private void LookAtPlayer()
    {
        //transform.up = playerTransform.position - transform.position;

        //transform.LookAt(playerTransform.position);

        //Vector2 relativePos = playerTransform.position - transform.position;
        Vector2 direction = new Vector2(
                transform.position.x - playerTransform.position.x,
                transform.position.y - playerTransform.position.y
            );
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
        transform.rotation = rotation;
        Debug.Log(transform.position);
    }

    public void AwakeEnemy()
    {
        isAwaken = true;
    }

}
