using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
	[SerializeField] private Sprite tile;

	private GameObject player;
	[SerializeField] private StageController stageController;
	private Vector2Int lastPlayerPosition;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.Find("Player");
		lastPlayerPosition = new Vector2Int(-100, -100);
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

	public void UpdateMinimap()
	{
		int mapRow = stageController.mapRow, mapCol = stageController.mapCol;
		Vector2 position = player.transform.position;
		int roomRow = stageController.roomRow, roomCol = stageController.roomCol;
		Vector2Int playerPosition = new Vector2Int();
		playerPosition.x = mapRow - 1 - (int)(position.y / roomRow);
		playerPosition.y = (int)(position.x / roomCol);
		playerPosition.x = MethodsForMap.Clamp(playerPosition.x, 0, mapCol - 1);
		playerPosition.y = MethodsForMap.Clamp(playerPosition.y, 0, mapRow - 1);
		// update past location
		if (lastPlayerPosition.x != -100 && lastPlayerPosition.y != -100)
		{
			transform.Find(GetTileName(lastPlayerPosition.x, lastPlayerPosition.y)).GetComponent<Image>().color = Color.red;
		}
		// update current location
		Debug.Log(transform);
		transform.Find(GetTileName(playerPosition.x, playerPosition.y)).GetComponent<Image>().color = Color.blue;
		// end
		lastPlayerPosition = playerPosition;
	}

	public void GenerateMinimap()
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
}
