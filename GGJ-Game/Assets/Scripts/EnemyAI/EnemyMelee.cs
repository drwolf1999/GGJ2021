using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    private StageController stageController;

    private Combat enemyCombat;

	private void Awake()
	{
        enemyCombat = this.GetComponent<EnemyCombat>();
        stageController = GameObject.Find("MapLoader").GetComponent<StageController>();
    }

	private void Start()
	{
        enemyCombat.Health = 60 + 15 * stageController.currentStage;
        enemyCombat.Attack = 20 + 5 * stageController.currentStage;
        enemyCombat.Defense = 0 + 7 * stageController.currentStage;
        enemyCombat.Penetration = 0 + 2 * stageController.currentStage;
        enemyCombat.CriticalRate = 5 + 1 * stageController.currentStage;
        enemyCombat.CriticalDamage = 50 + 2 * stageController.currentStage;
	}


	[SerializeField] private Collider2D myCollider;

    public void SwitchCollider()
    {
        myCollider.enabled = false;
        StartCoroutine("TurnOnCollider");
    }

    IEnumerator TurnOnCollider()
    {
        yield return new WaitForSeconds(2.0f);
        myCollider.enabled = true;
    }
}
