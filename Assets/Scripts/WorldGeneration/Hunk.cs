using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Hunk : MonoBehaviour
{
    private Block[,,] blocks;
    private bool buildAdjacentFaces = true;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Create Block array and populate it
        blocks = new Block[BlockData.hunkSize, BlockData.hunkSize, BlockData.hunkSize];
        for (int i = 0; i < BlockData.hunkSize; i++)
            for (int j = 0; j < BlockData.hunkSize; j++)
                for (int k = 0; k < BlockData.hunkSize; k++)
                    blocks[i, j, k] = new Block();

        AddBlock(new Vector3Int(0, 0, 0));
        Debug.Log($"vertices={vertices.Count} | triangles={triangles.Count}");
        AddBlock(new Vector3Int(2, 2, 2));
        Debug.Log($"vertices={vertices.Count} | triangles={triangles.Count}");

        BuildMesh();
    }

    public void AddBlock(Vector3Int pos)
    {
        // Add all 8 verts with the position offset
        for (int i = 0; i < 8; i++)
        {
            vertices.Add(BlockData.vertices[i] + pos);
        }

        if (buildAdjacentFaces)
        {
            for (int i = 0; i < 36; i++)
            {
                triangles.Add(BlockData.triangles[i]);
            }
        }
        else
        {
            // Loop through all 6 faces of the block
            for (int i = 0; i < 6; i++)
            {
                // Check to see if adjacent block is valid given chunk size
                Vector3 b = pos;
                if (ValidBlockPosition(pos + BlockData.faceChecks[i]))
                    b = pos + BlockData.faceChecks[i];

                // Only build face if the particular adjacent block isn't active
                if (!blocks[(int)b.x, (int)b.y, (int)b.z].active)
                {
                    // Add the two triangles for using specific set of six verts
                    int startIndex = i * 6;
                    for(int j = startIndex; j < startIndex + 6; j++)
                    {
                        triangles.Add(BlockData.triangles[j]);
                    }
                }
            }
        }
        // Set the current block to active since we just built it, even if all faces are invisible
        blocks[pos.x, pos.y, pos.z].active = true;
    }

    // Returns whether or not the position will fit within the chunk
    public bool ValidBlockPosition(Vector3 pos)
    {
        if (pos.x < 0 || pos.x > VoxelData.ChunkWidth - 1 || pos.y < 0 || pos.y > VoxelData.ChunkHeight - 1 || pos.z < 0 || pos.z > VoxelData.ChunkWidth - 1)
            return false;
        return true;
    }

    public void BuildMesh()
    {
        meshFilter.mesh = new Mesh();
		meshFilter.mesh.vertices = vertices.ToArray();
		meshFilter.mesh.triangles = triangles.ToArray();
		meshFilter.mesh.Optimize ();
		meshFilter.mesh.RecalculateNormals ();

        //_meshRenderer.material = _material;
    }
}
