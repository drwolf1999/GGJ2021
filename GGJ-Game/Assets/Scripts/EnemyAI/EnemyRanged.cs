using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] List<Transform> firePoints;
    [SerializeField] Transform spriteTransform;

    public bool isAwaken = false;

    public string bulletTag = "";

    public float rotateSpeed = 500f;

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
        isAwaken = true;

        timer = 0.0f;
        waitingTime = 2.0f;

        objectPooler = ObjectPooler.Instance;

        //enemyCombat = gameObject.GetComponent<Combat>();
    }

    void Update()
    {
        if(isAwaken)
        {
            //Spin();
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
        for (int i = 0; i < 8; ++i)
        {
            //Vector3 dir = (firePoints[i].position - this.transform.position).normalized;
            GameObject bullet = objectPooler.SpawnFromPool(bulletTag, firePoints[i].position, firePoints[i].rotation);
            bullet.GetComponent<EnemyBulletMove>().SetCombatStats(enemyCombat);
            //rotation.z += 45;
        }

        //Put Animation
    }

    //private void Spin()
    //{
    //    //transform.Rotate(0, 0, rotateSpeed);
    //    //rb.AddTorque(rotateSpeed);

    //    spriteTransform.Rotate(0, 0, rotateSpeed);
    //    Quaternion newRotation = spriteTransform.rotation;
    //    newRotation.z += rotateSpeed*Time.deltaTime;
    //    spriteTransform.rotation = newRotation;
    //}

    public void AwakeEnemy()
    {
        isAwaken = true;
    }

}
