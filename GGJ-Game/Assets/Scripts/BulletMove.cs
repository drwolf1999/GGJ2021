using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private Rigidbody2D rigid;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rigid.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
