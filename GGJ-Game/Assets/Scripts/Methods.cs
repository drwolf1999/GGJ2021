using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Methods
{
	public static bool IsWall(int r, int c, int row, int col)
	{
		return r == 0 || r == row - 1 || c == 0 || c == col - 1;
	}

	public static int Clamp(int x, int min, int max)
	{
		if (x < min) return min;
		if (max < x) return max;
		return x;
	}

	public static int Min(int x, int y)
	{
		if (x < y) return x;
		return y;
	}

	public static int Max(int x, int y)
	{
		if (x < y) return y;
		return x;
	}
}
