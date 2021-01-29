using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Vector2 movement;
    private Vector2 target, mouse;
    private float angle;
    private int time=0;
    private bool shooting = false;

    public float speed;
    public float attackSpeed;
    

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        PlayerLook();
    }

    private void FixedUpdate()
    {
        time++;
        if (time % 60 == 0)
            time = 0;

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
        if (Input.GetKey(KeyCode.Space))
        {
            if((time * attackSpeed) % 30 ==0 && !shooting)
            {
                var bullet = ObjectPool.GetObject();

                bullet.transform.position = transform.position;
            }
            
        }
    }
}
