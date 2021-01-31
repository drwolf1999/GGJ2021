using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
	[SerializeField] private Sprite tile;

	private GameObject player;
	[SerializeField] private StageController stageController;
	private GameObject cameraBound;
	private Vector2Int lastPlayerPosition;

	// Start is called before the first frame update
	void Awake()
	{
		GenerateMinimap();
		ResetMinimap();
		cameraBound = GameObject.Find("CameraBound");
		player = GameObject.Find("Player");
	}

	// Update is called once per frame
	void Update()
	{
		UpdateMinimap();
	}

	private string GetTileName(int r, int c)
	{
		return "miniTile_" + r + "_" + c;
	}

	public Vector2Int PlayerPosition
	{
		get
		{
			int mapRow = stageController.mapRow, mapCol = stageController.mapCol;
			Vector2 position = player.transform.position;
			int roomRow = stageController.roomRow, roomCol = stageController.roomCol;
			Vector2Int pPosition = lastPlayerPosition;
			if (!IsPlayerWall())
			{
				pPosition.x = mapRow - 1 - (int)(position.y / roomRow);
				pPosition.y = (int)(position.x / roomCol);
				pPosition.x = Methods.Clamp(pPosition.x, 0, mapCol - 1);
				pPosition.y = Methods.Clamp(pPosition.y, 0, mapRow - 1);
			}
			return pPosition;
		}
	}

	private bool IsPlayerWall()
	{
		int roomRow = stageController.roomRow, roomCol = stageController.roomCol;
		Vector2 playerPos = player.transform.position;
		Debug.Log(((int)playerPos.x % roomCol) + " XY " + ((int)playerPos.y % roomRow));
		if ((int)playerPos.x % roomCol == 0 ||
			(int)playerPos.x % roomCol == roomCol - 1 ||
			(int)playerPos.y % roomRow == 0 ||
			(int)playerPos.y % roomRow == roomRow - 1)
		{
			return true;
		}
		return false;
	}

	public void UpdateMinimap()
	{
		int mapRow = stageController.mapRow, mapCol = stageController.mapCol;
		Vector2 position = player.transform.position;
		int roomRow = stageController.roomRow, roomCol = stageController.roomCol;
		Vector2Int playerPosition = PlayerPosition;
		bool isPlayerWall = IsPlayerWall();
		// update past location
		if (lastPlayerPosition.x != -100 && lastPlayerPosition.y != -100)
		{
			transform.Find(GetTileName(lastPlayerPosition.x, lastPlayerPosition.y)).GetComponent<Image>().color = Color.red;
			if (lastPlayerPosition != playerPosition)
			{
				stageController.createdEnemy[lastPlayerPosition.x, lastPlayerPosition.y] = true;
			}
		}
		// update current location
		transform.Find(GetTileName(playerPosition.x, playerPosition.y)).GetComponent<Image>().color = Color.blue;
		cameraBound.transform.position = new Vector2(playerPosition.y * roomCol, (mapRow - 1 - playerPosition.x) * roomRow);
		if (!stageController.createdEnemy[playerPosition.x, playerPosition.y])
		{
			if (lastPlayerPosition != playerPosition)
			{
				// create enemy
				stageController.SpawnEnemy(playerPosition.x, playerPosition.y);
			}
		}
		Debug.Log(playerPosition + " COOR " + lastPlayerPosition);
		lastPlayerPosition = playerPosition;
		// end
	}

	private void SetMinimap(int r, int c)
	{
		string updateName = GetTileName(r, c);
		foreach (Transform t in transform)
		{
			Color color = Color.white;
			Image image = t.gameObject.GetComponent<Image>();
			if (t.gameObject.name.Equals(updateName))
			{
				color = Color.blue;
			}
			else
			{
				color = Color.white;
				if (image.color == Color.red || image.color == Color.blue)
				{
					color = Color.red;
				}
			}
			image.color = color;
		}
	}

	private void GenerateMinimap()
	{
		int row = stageController.mapRow, col = stageController.mapCol;
		for (int r = 0; r < row; r++)
		{
			for (int c = 0; c < col; c++)
			{
				GameObject image = new GameObject();
				image.name = GetTileName(r, c);
				Image img = image.AddComponent<Image>();
				img.sprite = tile;
				RectTransform rt = image.GetComponent<RectTransform>();
				rt.parent = this.transform;
				rt.localPosition = new Vector2(-col * 50 + c * 50, -r * 50);
				rt.sizeDelta = new Vector2(50, 50);
				image.SetActive(true);
			}
		}
	}

	public void ResetMinimap()
	{
		lastPlayerPosition = new Vector2Int(-100, -100);
		int row = stageController.mapRow, col = stageController.mapCol;
		for (int r = 0; r < row; r++)
		{
			for (int c = 0; c < col; c++)
			{
				transform.Find(GetTileName(r, c)).GetComponent<Image>().color = Color.white;
			}
		}
	}
}
