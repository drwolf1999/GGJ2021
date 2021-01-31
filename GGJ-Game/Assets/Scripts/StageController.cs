using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
	public int mapRow, mapCol;
	public int roomRow, roomCol;
	public int currentStage = 0;
	private MapLoader mapLoader;
	private RoomDesign roomDesign;

	[SerializeField] private StatPanel statPanel;

	[SerializeField] public Transform mapParent;
	[SerializeField] public Transform enemyParent;
	[SerializeField] public Minimap minimap;

	public bool[,] createdEnemy;
	public bool[][][][] available;

	private void Awake()
	{
		mapLoader = this.GetComponent<MapLoader>();
		roomDesign = this.GetComponent<RoomDesign>();

		createdEnemy = new bool[mapRow, mapCol];
		for (int r = 0; r < mapRow; r++)
		{
			for (int c = 0; c < mapCol; c++)
			{
				createdEnemy[r, c] = false;
			}
		}

		available = new bool[mapRow][][][];
		for (int i = 0; i < mapRow; i++)
		{
			available[i] = new bool[mapCol][][];
			for (int j = 0; j < mapCol; j++)
			{
				available[i][j] = new bool[roomRow][];
				for (int k = 0; k < roomRow; k++)
				{
					available[i][j][k] = new bool[roomCol];
					for (int l = 0; l < roomCol; l++)
					{
						available[i][j][k][l] = false;
					}
				}
			}
		}
	}

	private void ResetArray()
	{
		for (int i = 0; i < mapRow; i++)
		{
			for (int j = 0; j < mapCol; j++)
			{
				for (int k = 0; k < roomRow; k++)
				{
					for (int l = 0; l < roomCol; l++)
					{
						available[i][j][k][l] = false;
					}
				}
			}
		}
		for (int r = 0; r < mapRow; r++)
		{
			for (int c = 0; c < mapCol; c++)
			{
				createdEnemy[r, c] = false;
			}
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		GoNextStage();
	}

	// Update is called once per frame
	void Update()
	{
		if (IsClearCurrentRoom())
		{
			OpenDoor();
		}
	}

	private string EnemyRoomName(int r, int c)
	{
		return "enemy_" + r + "_" + c;
	}

	public void BeforeNextStage()
	{
		statPanel.gameObject.SetActive(true);
	}

	public void GoNextStage() {
		statPanel.gameObject.SetActive(false);
		minimap.ResetMinimap();
		ResetArray();
		foreach (Transform transform in mapParent) Destroy(transform.gameObject);
		foreach (Transform transform in enemyParent) Destroy(transform.gameObject);
		/// create enemy room
		for (int r = 0; r < mapRow; r++)
		{
			for (int c = 0; c < mapCol; c++)
			{
				GameObject eroom = new GameObject();
				eroom.name = EnemyRoomName(r, c);
				eroom.transform.parent = enemyParent;
			}
		}
		/// end
		currentStage++;
		mapLoader.GenerateStage(mapRow, mapCol, this);
		Debug.Log("STAGE: " + currentStage);
	}

	public void SpawnEnemy(int row, int col)
	{
		roomDesign.SpawnEnemy(row, col, available[row][col], enemyParent.Find(EnemyRoomName(row, col)));
		CloseDoor();
	}

	private bool IsClearCurrentRoom()
	{
		Vector2Int playerPosition = minimap.PlayerPosition;
		Transform tf = enemyParent.Find(EnemyRoomName(playerPosition.x, playerPosition.y));
		return tf.childCount == 0;
	}
	
	private void OpenDoor()
	{
		Vector2Int playerPosition = minimap.PlayerPosition;
		ref List<GameObject> doors = ref mapLoader.doors[playerPosition.x, playerPosition.y];
		foreach (GameObject doorGameObject in doors)
		{
			Door door = doorGameObject.GetComponent<Door>();
			door.Open();
			GameObject pair = door.GetAdjacentDoor();
			if (pair != null)
			{
				Door pairDoor = pair.GetComponent<Door>();
				pairDoor.Open();
			}
		}
	}
	private void CloseDoor()
	{
		Vector2Int playerPosition = minimap.PlayerPosition;
		ref List<GameObject> doors = ref mapLoader.doors[playerPosition.x, playerPosition.y];
		foreach (GameObject doorGameObject in doors)
		{
			Door door = doorGameObject.GetComponent<Door>();
			door.Close();
			GameObject pair = door.GetAdjacentDoor();
			if (pair != null)
			{
				Door pairDoor = pair.GetComponent<Door>();
				pairDoor.Close();
			}
		}
	}
}
