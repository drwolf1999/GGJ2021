using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Vector2 movement;
    private Vector2 target, mouse;
    private float angle;
    private Animator animator;

    public float speed;

    [SerializeField] Combat playerCombat;
    private float waitingTime = 1.0f;
    private bool isShootable = true;
    public Transform firePoint;

    ObjectPooler objectPooler;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        //playerCombat = GetComponent<Combat>();
        objectPooler = ObjectPooler.Instance;
        ApplyPlayerStats();
    }

    public void ApplyPlayerStats()
    {
        if (playerCombat.AttackSpeed <= 0.0f)
        {
            waitingTime = 1.0f;
        }
        else
        {
            waitingTime = 1.0f / (float)playerCombat.AttackSpeed;
        }

        if (playerCombat.MovementSpeed <= 0.0f)
        {
            speed = 7.0f;
        }
        else
        {
            speed = playerCombat.MovementSpeed;
        }
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if(movement.x != 0.0f || movement.y != 0.0f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        PlayerLook();
        Shoot();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + movement * speed * Time.fixedDeltaTime);
    }

    void PlayerLook()
    {
        target = transform.position;
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    void Shoot()
    {
        if (Input.GetKey(KeyCode.Mouse0) && isShootable)
        {
            GameObject bullet = objectPooler.SpawnFromPool("PlayerBullet", firePoint.position, firePoint.rotation);
            bullet.GetComponent<BulletMove>().SetCombatStats(playerCombat);
            isShootable = false;
            StartCoroutine("WaitForShoot");
        }
    }

    IEnumerator WaitForShoot()
    {
        yield return new WaitForSeconds(waitingTime);
        isShootable = true;
        //Debug.Log(isShootable);
    }
}
