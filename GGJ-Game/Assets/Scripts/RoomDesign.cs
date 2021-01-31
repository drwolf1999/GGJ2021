using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDesign : MonoBehaviour
{
	[SerializeField] private GameObject[] enemy;
	private StageController stageController;

	private void Awake()
	{
		stageController = GetComponent<StageController>();
	}

	private void Update()
	{
		//
	}

	public void SpawnEnemy(int playerRow, int playerCol, in bool[][] available, Transform eParent)
	{
		int row = stageController.roomRow, col = stageController.roomCol;
		int mapRow = stageController.mapRow, mapCol = stageController.mapCol;
		Vector2 position = new Vector2(playerCol * col, (mapRow - 1 - playerRow) * row);
		Debug.Log(position + " " + playerRow + " " + playerCol);
		for (int r = 0; r < row; r++)
		{
			for (int c = 0; c < col; c++)
			{
				if (!available[r][c]) continue;
				if (Random.Range(1, 20 * 16) > 4) continue;
				GameObject e = Instantiate(enemy[Random.Range(0, enemy.Length)], position + new Vector2(c, row - 1 - r), Quaternion.identity);
				EnemyMelee enemyMelee = e.GetComponent<EnemyMelee>();
				if (enemyMelee == null)
				{
					EnemyRanged enemyRanged = e.GetComponent<EnemyRanged>();
					enemyRanged.InitialSetting();
				}
				else
				{
					enemyMelee.InitialSetting();
				}
				e.transform.parent = eParent;
			}
		}
	}
}
