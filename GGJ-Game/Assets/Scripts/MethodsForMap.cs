using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodsForMap
{
	public static bool IsWall(int r, int c, int row, int col)
	{
		return r == 0 || r == row - 1 || c == 0 || c == col - 1;
	}
}
