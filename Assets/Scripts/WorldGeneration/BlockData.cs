using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockData
{
    public static readonly Vector3[] vertices = {
		new Vector3 (0, 0, 0),
		new Vector3 (1, 0, 0),
		new Vector3 (1, 1, 0),
		new Vector3 (0, 1, 0),
		new Vector3 (0, 1, 1),
		new Vector3 (1, 1, 1),
		new Vector3 (1, 0, 1),
		new Vector3 (0, 0, 1),
	};

	public static readonly int[] triangles = {
		1, 2, 0, //face front
		2, 3, 0,
		4, 3, 2, //face top
		5, 4, 2,
		5, 2, 1, //face right
		6, 5, 1,
		4, 7, 0, //face left
		3, 4, 0,
		7, 4, 5, //face back
		6, 7, 5,
		7, 6, 0, //face bottom
		6, 1, 0
	};

    public static readonly Vector3[] faceChecks = {

		new Vector3(0, 0, -1),
		new Vector3(0, 0, 1),
		new Vector3(0, 1, 0),
		new Vector3(0, -1, 0),
		new Vector3(-1, 0, 0),
		new Vector3(1, 0, 0)

	};

	public static readonly Vector2[] uvs = {
		new Vector2 (0, 0),
		new Vector2 (0, 1),
		new Vector2 (1, 0),
		new Vector2 (1, 0),
		new Vector2 (0, 1),
		new Vector2 (1, 1)
	};
}
