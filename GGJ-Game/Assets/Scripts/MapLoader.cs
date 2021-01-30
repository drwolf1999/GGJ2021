using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapLoader : MonoBehaviour
{
	private Vector2Int start, end;
	private RoomDesign roomDesign;
	private Minimap minimap;
	private Dictionary<char, GameObject> tileMap;
	private List<string[]> roomTypes;
	private GameObject player;
	public List<GameObject>[,] doors;

	[SerializeField] private GameObject door;

	private class DIR
	{
		public static int UP { get => 0; }
		public static int RIGHT { get => 1; }
		public static int DOWN { get => 2; }
		public static int LEFT { get => 3; }
		public static int reverse(int type)
		{
			if (type == UP) return DOWN;
			else if (type == DOWN) return UP;
			else if (type == RIGHT) return LEFT;
			else return RIGHT;
		}

		public static void Go(int type, ref int r, ref int c)
		{
			if (type == UP) r--;
			else if (type == RIGHT) c++;
			else if (type == DOWN) r++;
			else c--;
		}
	}

	private void Awake()
	{
		ReadResources();
		player = GameObject.Find("Player");
		roomDesign = GetComponent<RoomDesign>();
		minimap = GameObject.Find("Minimap").GetComponent<Minimap>();
	}

	// Start is called before the first frame update
	void Start()
	{
		/*		GenerateStage(stageController.mapRow, stageController.mapCol);*/
	}

	// Update is called once per frame
	void Update()
	{

	}

	private bool IsCenter(int x, int size)
	{
		return size / 2 - 1 <= x && x <= size / 2;
	}

	/// <summary>
	/// generate room (size: row X col)
	/// </summary>
	/// <param name="row">row size</param>
	/// <param name="col">col size</param>
	/// <param name="position">pivot position</param>
	/// <param name="type">Room type bit (up, right, down, left)</param>
	private void GenerateRoom(int row, int col, Vector2 position, int type, int startOrEndOrNo, ref GameObject room, string[] roomType, ref bool[][] available, ref List<GameObject> roomDoors)
	{
		Vector3 doorScale = new Vector3(0.44f, 0.22f, 1f);
		/// ln : 상하 통로
		GameObject obj, instance;
		for (int r = 0; r < row; r++)
		{
			for (int c = 0; c < col; c++)
			{
				Vector3 scale = Vector3.one;
				Vector3 rotation = Vector3.zero;
				Vector3 spawnPosition = position + new Vector2(c, row - 1 - r);
				bool isGoal = false;
				if (((type & (1 << DIR.UP)) > 0 && (0 <= r && r <= 2) && IsCenter(c, col)))
				{
					if (r == 0)
					{
						if (c == col / 2) continue;
						obj = door;
						spawnPosition.x += 0.5f;
						scale = doorScale;
					}
					else if (c == col / 2 - 1)
					{
						obj = tileMap['l'];
					}
					else if (c == col / 2)
					{
						obj = tileMap['n'];
					}
					else
					{
						obj = tileMap[roomType[r][c]];
					}
				}
				else if (((type & (1 << DIR.RIGHT)) > 0 && IsCenter(r, row) && c == col - 1))
				{
					if (r == row / 2) continue;
					obj = door;
					rotation.z -= 90f;
					spawnPosition.y -= 0.5f;
					scale = doorScale;
				}
				else if (((type & (1 << DIR.DOWN)) > 0 || startOrEndOrNo == 2) && r == row - 1 && IsCenter(c, col))
				{
					if (c == col / 2) continue;
					isGoal = startOrEndOrNo == 2;
					obj = door;
					rotation.z -= 2 * 90f;
					spawnPosition.x += 0.5f;
					scale = doorScale;
				}
				else if ((type & (1 << DIR.LEFT)) > 0 && IsCenter(r, row) && c == 0)
				{
					if (r == row / 2) continue;
					obj = door;
					rotation.z -= 3 * 90f;
					spawnPosition.y -= 0.5f;
					scale = doorScale;
				}
				/*else if (r == 0 || r == row - 1 || c == 0 || c == col - 1)
				{
					obj = wall;
				}
				else if (startOrEndOrNo != 0 && r == (row - 1) / 2 && c == (col - 1) / 2)
				{
					obj = floor;
				}*/
				/*else if (r == 0 || r == row - 1 || c == 0 || c == col - 1)
				{
					obj = null;
				}*/
				else
				{
					if (tileMap.ContainsKey(roomType[r][c]))
					{
						obj = tileMap[roomType[r][c]];
					}
					else
					{
						Debug.Log(roomType[r][c]);
						obj = null;
					}
				}
				if (obj)
				{
					instance = Instantiate(obj, spawnPosition, Quaternion.identity);
					instance.transform.parent = room.transform;
					instance.transform.Rotate(rotation);
					instance.transform.localScale = scale;
					available[r][c] = false;

					///
					if (isGoal)
					{
						EdgeCollider2D edge = instance.AddComponent<EdgeCollider2D>();
						edge.offset = new Vector2(0, -1f);
						edge.isTrigger = true;
					}
					///
					if (!instance.CompareTag("Obstacle") && obj != door)
					{
						if (startOrEndOrNo == 1) // start position
						{
							player.transform.position = spawnPosition;
						}
						available[r][c] = true;
					}
					else if (obj == door)
					{
						roomDoors.Add(instance);
					}
				}
			}
		}
	}

	/// <summary>
	/// generate stage (size: row X col)
	/// </summary>
	/// <param name="row">row size</param>
	/// <param name="col">col size</param>
	public void GenerateStage(int row, int col, StageController stageController)
	{
		int[,] shape = GenerateShape(row, col);
		doors = new List<GameObject>[row, col];
		/*		/// DEBUG
				string s = "";
				for (int i = 0; i < row; i++)
				{
					for (int j = 0; j < col; j++)
					{
						s += shape[i, j];
					}
					s += "\n";
				}
				Debug.Log(s);
				/// END DEBUG*/
		GameObject room;
		for (int r = 0; r < row; r++)
		{
			for (int c = 0; c < col; c++)
			{
				doors[r, c] = new List<GameObject>();
				Vector2 v = new Vector2(r, c);
				room = new GameObject();
				room.transform.parent = stageController.mapParent;
				room.name = "room_" + r + "_" + c;
				Vector2 spawnPosition = new Vector2(stageController.roomCol * c, stageController.roomRow * (row - 1 - r));
				GenerateRoom(stageController.roomRow, stageController.roomCol, spawnPosition,
							shape[r, c], (v == start ? 1 : v == end ? 2 : 0), ref room,
							roomTypes[Random.Range(0, roomTypes.Count - 1)], ref stageController.available[r][c],
							ref doors[r, c]);
			}
		}
		// A*
		AstarPath.active.Scan();
	}

	private int[,] GenerateShape(int row, int col)
	{
		int[,] ret = new int[row, col];
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				ret[i, j] = 0;
			}
		}

		start.x = 0;
		start.y = Random.Range(0, col - 1);
		end.x = row - 1;
		end.y = Random.Range(0, col - 1);

		List<int> l = new List<int>();
		// -1 : go left, 1 : go right
		// -2 : go down, 2 : go up
		Vector2Int cur = start;
		int lr;
		while (cur.x < row)
		{
			if (Random.Range(0, 2) == 0)
			{
				lr = -1;
			}
			else
			{
				lr = 1;
			}
			if (cur.x == row - 1)
			{
				if (cur.y < end.y)
				{
					while (cur.y + 1 <= end.y)
					{
						cur.y += 1;
						l.Add(DIR.RIGHT);
					}
				}
				else
				{
					while (cur.y - 1 >= end.y)
					{
						cur.y -= 1;
						l.Add(DIR.LEFT);
					}
				}
				break;
			}
			if (lr == -1)
			{
				for (int i = Random.Range(0, cur.y + 1); i > 0; i--)
				{
					cur.y -= 1;
					l.Add(DIR.LEFT);
				}
			}
			else
			{
				for (int i = Random.Range(0, col - 1 - cur.y); i > 0 && cur.y + 1 <= col - 1; i--)
				{
					cur.y += 1;
					l.Add(DIR.RIGHT);
				}
			}
			cur.x += 1;
			l.Add(DIR.DOWN);
		}
		Vector2Int v = start;
		for (int i = 0; i < l.Count; i++)
		{
			ret[v.x, v.y] |= 1 << l[i];
			if (l[i] == DIR.UP)
			{
				v.x -= 1;
			}
			else if (l[i] == DIR.RIGHT)
			{
				v.y += 1;
			}
			else if (l[i] == DIR.DOWN)
			{
				v.x += 1;
			}
			else // case left
			{
				v.y -= 1;
			}
			ret[v.x, v.y] |= 1 << DIR.reverse(l[i]);
		}

		for (int r = 0; r < row; r++)
		{
			for (int c = 0; c < col; c++)
			{
				if (ret[r, c] == 0)
				{
					FillDFS(r, c, row, col, ref ret);
				}
			}
		}

		return ret;
	}

	private void FillDFS(int r, int c, int row, int col, ref int[,] ret)
	{
		List<int> dir = new List<int> { DIR.UP, DIR.RIGHT, DIR.DOWN, DIR.LEFT };
		dir = dir.OrderBy(a => System.Guid.NewGuid()).ToList();
		bool isRet = false;
		for (int i = 0; i < dir.Count; i++)
		{
			int nr = r, nc = c;
			DIR.Go(dir[i], ref nr, ref nc);
			if (nr < 0 || row - 1 < nr || nc < 0 || col - 1 < nc) continue;
			ret[r, c] |= 1 << dir[i];
			if (ret[nr, nc] > 0) isRet = true;
			ret[nr, nc] |= 1 << DIR.reverse(dir[i]);
			if (isRet) return;
			FillDFS(nr, nc, row, col, ref ret);
		}
	}

	private void ReadResources()
	{
		tileMap = new Dictionary<char, GameObject>();
		// Read Tiles
		GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/Tiles");
		foreach (GameObject gameObject in gameObjects)
		{
			tileMap[gameObject.name[0]] = gameObject;
		}

		// Read Rooms
		roomTypes = new List<string[]>();
		TextAsset[] txts = Resources.LoadAll<TextAsset>("Prefabs/Rooms");
		foreach (TextAsset txt in txts)
		{
			roomTypes.Add(txt.text.Split('\n'));
		}
	}
}