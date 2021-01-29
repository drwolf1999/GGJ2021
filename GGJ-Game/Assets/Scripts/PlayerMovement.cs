using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Vector2 movement;
    private Vector2 target, mouse;
    private float angle;

    public float speed;
    public float attackSpeed;

    Combat playerCombat;
    private float waitingTime;
    private bool isShootable = true;

    ObjectPooler objectPooler;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerCombat = GetComponent<Combat>();
        waitingTime = 1.0f / (float)playerCombat.AttackSpeed;

        objectPooler = ObjectPooler.Instance;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        PlayerLook();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + movement * speed * Time.fixedDeltaTime);
        Shoot();
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
        if (Input.GetKey(KeyCode.Space) && isShootable)
        {
            objectPooler.SpawnFromPool("PlayerBullet", transform.position, Quaternion.identity);
            isShootable = false;
            WaitForShoot();
        }
    }

    IEnumerator WaitForShoot()
    {
        yield return new WaitForSeconds(waitingTime);
        isShootable = true;
    }
}
