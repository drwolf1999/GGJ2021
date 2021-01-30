using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDesign : MonoBehaviour
{
    /*[SerializeField] private GameObject enemy;*/
	private StageController stageController;

	private void Awake()
	{
		stageController = GetComponent<StageController>();
	}

	private void Update()
	{
		//
	}

	public void SpawnEnemy(int row, int col, Vector2 position)
	{
		/*for (int r = 0; r < row; r++)
		{
			for (int c = 0; c < col; c++)
			{
				if (MethodsForMap.IsWall(r, c, row, col)) continue;
				if (Random.Range(1, 13 * 13) > 4) continue;
				GameObject e = Instantiate(enemy, position + new Vector2(c, row - 1 - r), Quaternion.identity);
				e.transform.parent = stageController.enemyParent;
			}
		}*/
	}
}
