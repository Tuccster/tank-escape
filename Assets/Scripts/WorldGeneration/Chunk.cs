using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    // Get blocks from read file
    // Pass blocks in method to delegate blocks to a corrosponding chucks
    // Build chunk using block data

    MeshFilter _meshFilter;
    MeshRenderer _meshRenderer;

    public Material _material;
    readonly byte _chunkSize = 10;
    public Block[,,] _blocks;

    private List<Vector3> _verts = new List<Vector3>();
    private List<int> _tris = new List<int>();

    public class Block
    {
        //public byte faces;
        //null:null:front:top:right:left:back:bottom

        public bool active = false;
    }

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        _blocks = new Block[_chunkSize, _chunkSize, _chunkSize];
        for (int i = 0; i < _chunkSize; i++)
            for (int j = 0; j < _chunkSize; j++)
                for (int k = 0; k < _chunkSize; k++)
                    _blocks[i, j, k] = new Block();

        AddBlock(new Vector3Int(1, 1, 1));

        Debug.Log($"#of_verts={_verts.Count}|#of_tris={_tris.Count}");

        AddBlock(new Vector3Int(1, 3, 1));

        Debug.Log($"#of_verts={_verts.Count}|#of_tris={_tris.Count}");
        RefreshMesh();
    }

    public void AddBlock(Vector3Int position)
    {
        if (position.x < 0 || position.x >= _chunkSize) return;
        if (position.y < 0 || position.y >= _chunkSize) return;
        if (position.z < 0 || position.z >= _chunkSize) return;

        _blocks[position.x, position.y, position.z].active = true;

        Vector3[] vertices = {
			new Vector3 (0, 0, 0),
			new Vector3 (1, 0, 0),
			new Vector3 (1, 1, 0),
			new Vector3 (0, 1, 0),
			new Vector3 (0, 1, 1),
			new Vector3 (1, 1, 1),
			new Vector3 (1, 0, 1),
			new Vector3 (0, 0, 1),
		};

        for (int i = 0; i < vertices.Length; i++)
        {
            _verts.Add(vertices[i] + position);
        }

        // front
        if (position.x + 1 < _chunkSize)
            if (!_blocks[position.x + 1, position.y, position.z].active)
                AddTriangleArray(new int[] {1, 2, 0, 2, 3, 0});
        // top
        if (position.y + 1 < _chunkSize)
            if (!_blocks[position.x, position.y + 1, position.z].active)
                AddTriangleArray(new int[] {4, 3, 2, 5, 4, 2});
        // right
        if (position.z + 1 < _chunkSize)
            if (!_blocks[position.x, position.y, position.z + 1].active)
                AddTriangleArray(new int[] {5, 2, 1, 6, 5, 1});
        // back
        if (position.x - 1 < _chunkSize)
            if (!_blocks[position.x - 1, position.y, position.z].active)
                AddTriangleArray(new int[] {7, 4, 5, 6, 7, 5});
        // bottom
        if (position.y - 1 < _chunkSize)
            if (!_blocks[position.x, position.y - 1, position.z].active)
                AddTriangleArray(new int[] {7, 6, 0, 6, 1, 0});
        // left
        if (position.z - 1 < _chunkSize)
            if (!_blocks[position.x, position.y, position.z - 1].active)
                AddTriangleArray(new int[] {4, 7, 0, 3, 4, 0});

        RefreshMesh();
    }

    private void AddTriangleArray(int[] triangles)
    {
        for (int i = 0; i < triangles.Length; i++)
            _tris.Add(triangles[i]);
    }

    private void RefreshMesh()
    {
		_meshFilter.mesh = new Mesh();
		_meshFilter.mesh.vertices = _verts.ToArray();
		_meshFilter.mesh.triangles = _tris.ToArray();
		_meshFilter.mesh.Optimize ();
		_meshFilter.mesh.RecalculateNormals ();

        _meshRenderer.material = _material;
    }
}
