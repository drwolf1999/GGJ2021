using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private Collider2D collider;

    public void SwitchCollider()
    {
        collider.enabled = false;
        StartCoroutine("TurnOnCollider");
    }

    IEnumerator TurnOnCollider()
    {
        yield return new WaitForSeconds(2.0f);
        collider.enabled = true;
    }
}
