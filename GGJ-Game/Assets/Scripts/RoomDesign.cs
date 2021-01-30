﻿using System.Collections;
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

	public void SpawnEnemy(int playerRow, int playerCol, in bool[][] available)
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
				if (Random.Range(1, 13 * 13) > 4) continue;
				GameObject e = Instantiate(enemy[Random.Range(0, enemy.Length)], position + new Vector2(c, row - 1 - r), Quaternion.identity);
				e.transform.parent = stageController.enemyParent;
			}
		}
	}
}
