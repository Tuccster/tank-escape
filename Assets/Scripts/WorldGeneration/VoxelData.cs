using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelData
{
    /* public static readonly Vector3[] verts = {
		new Vector3 (0, 0, 0),
		new Vector3 (1, 0, 0),
		new Vector3 (1, 1, 0),
		new Vector3 (0, 1, 0),
		new Vector3 (0, 1, 1),
		new Vector3 (1, 1, 1),
		new Vector3 (1, 0, 1),
		new Vector3 (0, 0, 1),
	};

	public static readonly int[] tris = {
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

	public static readonly Vector2[] uvs = {
		new Vector2 (0, 0),
		new Vector2 (0, 1),
		new Vector2 (1, 0),
		new Vector2 (1, 0),
		new Vector2 (0, 1),
		new Vector2 (1, 1)
	}; */

    public static readonly int ChunkWidth = 10;
	public static readonly int ChunkHeight = 10;

	public static readonly Vector3[] voxelVerts = new Vector3[8] {

		new Vector3(0.0f, 0.0f, 0.0f),
		new Vector3(1.0f, 0.0f, 0.0f),
		new Vector3(1.0f, 1.0f, 0.0f),
		new Vector3(0.0f, 1.0f, 0.0f),
		new Vector3(0.0f, 0.0f, 1.0f),
		new Vector3(1.0f, 0.0f, 1.0f),
		new Vector3(1.0f, 1.0f, 1.0f),
		new Vector3(0.0f, 1.0f, 1.0f),

	};

	public static readonly Vector3[] faceChecks = new Vector3[6] {

		new Vector3(0.0f, 0.0f, -1.0f),
		new Vector3(0.0f, 0.0f, 1.0f),
		new Vector3(0.0f, 1.0f, 0.0f),
		new Vector3(0.0f, -1.0f, 0.0f),
		new Vector3(-1.0f, 0.0f, 0.0f),
		new Vector3(1.0f, 0.0f, 0.0f)

	};

	public static readonly int[,] voxelTris = new int[6,4] {

		// 0 1 2 2 1 3
		{2, 1, 3, 0}, // Back Face
		{7, 4, 6, 5}, // Front Face
		{6, 2, 7, 3}, // Top Face
		{4, 0, 5, 1}, // Bottom Face
		{3, 0, 7, 4}, // Left Face
		{6, 5, 2, 1} // Right Face

	};

	public static readonly Vector2[] voxelUvs = new Vector2[4] {

		new Vector2 (0.0f, 0.0f),
		new Vector2 (0.0f, 1.0f),
		new Vector2 (1.0f, 0.0f),
		new Vector2 (1.0f, 1.0f)

	};

}
