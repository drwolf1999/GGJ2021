using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTextmesh : MonoBehaviour, IPooledObject
{

    private float speed = 1f;
    private bool isSpawn = false;
    private float timer = 3f;


    public void OnObjectSpawn()
    {
        isSpawn = true;
        timer = 1f;
    }

    void Update()
    {
        if(isSpawn)
        {
            transform.Translate(Vector2.up * Time.deltaTime * speed);
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                isSpawn = false;
                gameObject.SetActive(false);
            }
        }
    }
}
