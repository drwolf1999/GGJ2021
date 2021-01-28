﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapLoader : MonoBehaviour
{
	private Vector2Int start, end;

	[SerializeField] private Transform mapParent;
	[SerializeField] private GameObject door;
	[SerializeField] private GameObject wall;
	[SerializeField] private GameObject floor;

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

	// Start is called before the first frame update
	void Start()
	{
		GenerateStage(4, 4);
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
	private void GenerateRoom(int row, int col, Vector2 position, int type, int startOrEndOrNo)
	{
		GameObject obj, instance;
		for (int r = 0; r < row; r++)
		{
			for (int c = 0; c < col; c++)
			{
				if (((type & (1 << DIR.UP)) > 0 && r == 0 && IsCenter(c, col)) ||
					((type & (1 << DIR.RIGHT)) > 0 && IsCenter(r, row) && c == col - 1) ||
					((type & (1 << DIR.DOWN)) > 0 && r == row - 1 && IsCenter(c, col)) ||
					((type & (1 << DIR.LEFT)) > 0 && IsCenter(r, row) && c == 0))
				{
					obj = door;
				}
				else if (r == 0 || r == row - 1 || c == 0 || c == col - 1)
				{
					obj = wall;
				}
				else if (startOrEndOrNo != 0 && r == (row - 1) / 2 && c == (col - 1) / 2)
				{
					obj = floor;
				}
				/*else if (r == 0 || r == row - 1 || c == 0 || c == col - 1)
				{
					obj = null;
				}*/
				else
				{
					obj = floor;
				}
				if (obj)
				{
					instance = Instantiate(obj, position + new Vector2(c, row - 1 - r), Quaternion.identity);
					if (startOrEndOrNo > 0) // DEBUG
					{
						instance.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
					}
					instance.transform.parent = mapParent;
				}
			}
		}
	}

	/// <summary>
	/// generate stage (size: row X col)
	/// </summary>
	/// <param name="row">row size</param>
	/// <param name="col">col size</param>
	public void GenerateStage(int row, int col)
	{
		int[,] shape = GenerateShape(row, col);
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
		int roomRow = 16, roomCol = 16;
		for (int r = 0; r < row; r++)
		{
			for (int c = 0; c < col; c++)
			{
				Vector2 v = new Vector2(r, c);
				/*Debug.Log(r + ", " + c + " " + new Vector2(roomCol * c, roomRow * (row - 1 - r)) + " " + shape[r, c]);*/
				GenerateRoom(roomRow, roomCol, new Vector2(roomCol * c, roomRow * (row - 1 - r)), shape[r, c], (v == start ? 1 : v == end ? 2 : 0));
			}
		}
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
}