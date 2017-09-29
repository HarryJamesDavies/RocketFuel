using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Serializable]
    public class Section
    {
        public Section(int _index, int _width, int _height, GameObject[,] _content, Transform _parent)
        {
            Grid = _content;
            Width = _width - 1;
            Height = _height - 1;
            Holder = new GameObject("Section " + _index + " Holder: ").transform;
            Holder.SetParent(_parent);
        }

        public void SpawnSection(float _cellWidth, float _cellHeight, Vector3 _origin)
        {
            for (int y = 0; y <= Height; y++)
            {
                for (int x = 0; x <= Width; x++)
                {
                    Vector3 pos = new Vector3(_origin.x + (_cellWidth * x) + (_cellWidth / 2.0f), 
                        _origin.y + (_cellHeight * y) + (_cellHeight / 2.0f), 0.0f);
                    Grid[x, y] = Instantiate(Grid[x, y], pos, Quaternion.identity);
                    Grid[x, y].transform.SetParent(Holder);
                }
            }
        }

        public void ClearGrid()
        {
            for (int y = 0; y <= Height; y++)
            {
                for (int x = 0; x <= Width; x++)
                {
                    Destroy(Grid[x, y]);
                }
            }
        }

        public GameObject[,] Grid;

        public int Width;
        public int Height;
        private Transform Holder;
    }

    public enum Openings
    {
        Left,
        Middle,
        Right
    }

    [Serializable]
    public class Chunk
    {
        public Texture2D Texture;
        public List<Openings> Entrances;
        public List<Openings> Exits;
    }

    [Serializable]
    public class Cell
    {
        public Color Identifier;
        public GameObject Prefab;
    }

    public Chunk m_spawn;
    public Chunk m_separator;
    public List<Chunk> m_chunkData;

    public GameObject m_defaultTemplate;
    public List<Cell> m_cellTemplates;

    public int m_currentSection = 0;
    private bool m_currentBuffer = false;
    private Section m_sectionA;
    private Section m_sectionB;
    private int m_sectionsGenerated = -1;

    private float m_cellWidth;
    private float m_cellHeight;
    private Vector3 m_nextOrigin;

    public void InitaliseLevel(int _chunks, float _cellWidth, float _cellHeight, Vector3 _origin)
    {
        m_cellWidth = _cellWidth;
        m_cellHeight = _cellHeight;
        m_nextOrigin = _origin;

        List<Texture2D> startTextures = new List<Texture2D>();
        startTextures.Add(m_spawn.Texture);
        startTextures.Add(m_separator.Texture);
        m_sectionA = GenerateSection(startTextures);
        m_sectionA.SpawnSection(m_cellWidth, m_cellHeight, m_nextOrigin);
        m_nextOrigin.y = m_nextOrigin.y + ((m_sectionA.Height + 1) * m_cellHeight);

        m_sectionB = GenerateSection(SelectChunks(_chunks));
        m_sectionB.SpawnSection(m_cellWidth, m_cellHeight, m_nextOrigin);
        m_nextOrigin.y = m_nextOrigin.y + ((m_sectionB.Height + 1) * m_cellHeight);

        m_currentBuffer = false;
    }

    private Section GenerateSection(Texture2D _chunk)
    {
        int width = _chunk.width;
        int height = _chunk.height;
        GameObject[,] grid = new GameObject[width, height];

        Color[] pixels = _chunk.GetPixels();
        for (int y = 0; y <= _chunk.height - 1; y++)
        {
            for (int x = 0; x <= _chunk.width - 1; x++)
            {
                grid[x, y] = GetCell(pixels[(y * width) + x]);
            }
        }

        m_sectionsGenerated++;
        return new Section(m_sectionsGenerated, width, height, grid, transform);
    }

    private Section GenerateSection(List<Texture2D> _chunks)
    {
        int width = 0;
        int height = 0;

        foreach (Texture2D chunk in _chunks)
        {
            if(chunk.width > width)
            {
                width = chunk.width;
            }
            height += chunk.height;
        }

        height -= _chunks.Count - 1;

        GameObject[,] grid = new GameObject[width, height];
        for (int y = 0; y <= height - 1; y++)
        {
            for (int x = 0; x <= width - 1; x++)
            {
                grid[x, y] = m_defaultTemplate;
            }
        }

        int index = 0;
        foreach (Texture2D chunk in _chunks)
        {
            Color[] pixels = chunk.GetPixels();
            for (int y = 0; y <= chunk.height - 1; y++)
            {
                for (int x = 0; x <= chunk.width - 1; x++)
                {
                    grid[x, (y + index)] = GetCell(pixels[(y * chunk.width) + x]);
                }
            }

            index += (chunk.height - 1);
        }

        m_sectionsGenerated++;
        return new Section(m_sectionsGenerated, width, height, grid, transform);
    }

    private GameObject GetCell(Color _identifier)
    {
        foreach(Cell template in m_cellTemplates)
        {
            if(template.Identifier == _identifier)
            {
                return template.Prefab;
            }
        }

        Debug.Log(_identifier);
        return m_defaultTemplate;
    }

    private List<Texture2D> SelectChunks(int _chunks)
    {
        List<Texture2D> chunks = new List<Texture2D>();

        int prevIndex = GetNextChunk(m_separator.Exits);
        chunks.Add(m_chunkData[prevIndex].Texture);

        for (int iter = 1; iter <= _chunks - 3; iter++)
        {
            prevIndex = GetNextChunk(m_chunkData[prevIndex].Exits);
            chunks.Add(m_chunkData[prevIndex].Texture);
        }

        prevIndex = GetNextChunk(m_chunkData[prevIndex].Exits, m_separator.Entrances);
        chunks.Add(m_chunkData[prevIndex].Texture);

        chunks.Add(m_separator.Texture);

        return chunks;
    }

    public int GetNextChunk(List<Openings> _exits)
    {
        int startingIndex = UnityEngine.Random.Range(0, m_chunkData.Count);

        for(int index = startingIndex; index <= m_chunkData.Count - 1; index++)
        {
            if(CompareOpenings(m_chunkData[index].Entrances, _exits))
            {
                return index;
            }
        }

        for (int index = 0; index <= startingIndex - 1; index++)
        {
            if (CompareOpenings(m_chunkData[index].Entrances, _exits))
            {
                return index;
            }
        }

        Debug.Log("Couldn't find any chunks that matched");
        return -1;
    }

    public int GetNextChunk(List<Openings> _exits, List<Openings> _entrances)
    {
        int startingIndex = UnityEngine.Random.Range(0, m_chunkData.Count);

        for (int index = startingIndex; index <= m_chunkData.Count - 1; index++)
        {
            if (CompareOpenings(m_chunkData[index].Entrances, _exits))
            {
                if (CompareOpenings(m_chunkData[index].Exits, _entrances))
                {
                    return index;
                }
            }
        }

        for (int index = 0; index <= startingIndex - 1; index++)
        {
            if (CompareOpenings(m_chunkData[index].Entrances, _exits))
            {
                if (CompareOpenings(m_chunkData[index].Exits, _entrances))
                {
                    return index;
                }
            }
        }

        Debug.Log("Couldn't find any chunks that matched");
        return -1;
    }

    public bool CompareOpenings(List<Openings> _entrances, List<Openings> _exits)
    {
        foreach(Openings entrance in _entrances)
        {
            foreach(Openings exit in _exits)
            {
                if(entrance == exit)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
