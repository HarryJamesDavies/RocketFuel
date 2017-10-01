﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Serializable]
    public class Section
    {
        public Section(int _index, int _width, int _height, GameObject[,] _content, Transform _parent)
        {
            Index = _index;
            Grid = _content;
            Width = _width - 1;
            Height = _height - 1;
            Holder = new GameObject("Section " + _index + " Holder: ").transform;
            Holder.SetParent(_parent);

            SpawnGrid = new bool[_width, _height];
            for (int y = 0; y <= Height; y++)
            {
                for (int x = 0; x <= Width; x++)
                {
                    SpawnGrid[x, y] = false;
                }
            }
        }

        /// <summary>
        /// Instantiates cells based on Grid data
        /// </summary>
        /// <param name="_cellWidth"></param>
        /// <param name="_cellHeight"></param>
        /// <param name="_origin"></param>
        public void SpawnSection(float _cellWidth, float _cellHeight, Vector3 _origin)
        {
            CellWidth = _cellWidth;
            CellHeight = _cellHeight;
            Origin = _origin;

            for (int y = 0; y <= Height; y++)
            {
                for (int x = 0; x <= Width; x++)
                {
                    Vector3 pos = new Vector3(Origin.x + (CellWidth * x) + (CellWidth / 2.0f),
                        Origin.y + (CellHeight * y) + (CellHeight / 2.0f), 0.0f);
                    Grid[x, y] = Instantiate(Grid[x, y], pos, Quaternion.identity);
                    Grid[x, y].transform.SetParent(Holder);
                    Grid[x, y].GetComponent<CellData>().Initialise(Index, new GridCoordinates(x, y));
                }
            }
        }

        public void SpawnLava(GameObject _lava)
        {
            for (int y = 0; y <= Height; y++)
            {
                for (int x = 0; x <= Width; x++)
                {
                    if(SpawnGrid[x, y])
                    {
                        Destroy(Grid[x, y]);

                        Vector3 pos = new Vector3(Origin.x + (CellWidth * x) + (CellWidth / 2.0f),
                            Origin.y + (CellHeight * y) + (CellHeight / 2.0f), 0.0f);
                        Grid[x, y] = Instantiate(_lava, pos, Quaternion.identity);
                        Grid[x, y].transform.SetParent(Holder);
                        Grid[x, y].GetComponent<CellData>().Initialise(Index, new GridCoordinates(x, y), CellData.CellContent.Liquid);

                        SpawnGrid[x, y] = false;
                    }
                }
            }
        }

        /// <summary>
        /// Deletes GameObjects contained in Grid
        /// </summary>
        public void ClearGrid()
        {
            Width = 0;
            Height = 0;

            for (int y = 0; y <= Height; y++)
            {
                for (int x = 0; x <= Width; x++)
                {
                    Destroy(Grid[x, y]);
                }
            }

            for (int y = 0; y <= Height; y++)
            {
                for (int x = 0; x <= Width; x++)
                {
                    SpawnGrid[x, y] = false;
                }
            }

            Destroy(Holder.gameObject);
        }

        public bool CheckCellFilled(GridCoordinates _coords)
        {
            return (CellData.CellContent.Air != Grid[_coords.X, _coords.Y].GetComponent<CellData>().m_content);
        }

        public void AddLava(GridCoordinates _coords)
        {
            SpawnGrid[_coords.X, _coords.Y] = true;
        }

        public GameObject[,] Grid;
        public bool[,] SpawnGrid;

        public int Index;
        public int Width;
        public int Height;

        private Transform Holder;
        private float CellWidth;
        private float CellHeight;
        private Vector3 Origin;
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
    public GameObject m_finishLine;
    public List<Chunk> m_separators;
    public List<Chunk> m_chunkData;

    public GameObject m_lavaTemplate;
    public GameObject m_defaultTemplate;
    public List<Cell> m_cellTemplates;

    private bool m_currentBuffer = true;
    private Section m_sectionA;
    private Section m_sectionB;
    public int m_sectionsGenerated = -1;

    private int m_chunks;
    private float m_cellWidth;
    private float m_cellHeight;
    private Vector3 m_nextOrigin;

    public void TransitionSection()
    {
        if(m_currentBuffer)
        {
            m_sectionA.ClearGrid();

            m_sectionA = GenerateSection(SelectChunks(m_chunks));
            m_sectionA.SpawnSection(m_cellWidth, m_cellHeight, m_nextOrigin);
            m_nextOrigin.y = m_nextOrigin.y + ((m_sectionA.Height + 1) * m_cellHeight);
        }
        else
        {
            m_sectionB.ClearGrid();

            m_sectionB = GenerateSection(SelectChunks(m_chunks));
            m_sectionB.SpawnSection(m_cellWidth, m_cellHeight, m_nextOrigin);
            m_nextOrigin.y = m_nextOrigin.y + ((m_sectionB.Height + 1) * m_cellHeight);
        }

        m_currentBuffer = !m_currentBuffer;
    }

    public void SpawnFinishLine()
    {
        if (m_currentBuffer)
        {
            m_nextOrigin.x = m_sectionA.Width / 2.0f;
        }
        else
        {
            m_nextOrigin.x = m_sectionB.Width / 2.0f;
        }
        GameObject finish = Instantiate(m_finishLine, m_nextOrigin, Quaternion.identity);
        finish.transform.SetParent(transform);
        m_currentBuffer = !m_currentBuffer;
    }

    /// <summary>
    /// Generates Spawn section and 1st section, sets general level parameters
    /// </summary>
    /// <param name="_chunks"></param>
    /// <param name="_cellWidth"></param>
    /// <param name="_cellHeight"></param>
    /// <param name="_origin"></param>
    public void InitaliseLevel(int _chunks, float _cellWidth, float _cellHeight, Vector3 _origin)
    {
        //General level parameters
        m_chunks = _chunks;
        m_cellWidth = _cellWidth;
        m_cellHeight = _cellHeight;
        m_nextOrigin = _origin;

        //Spawn section
        m_sectionA = GenerateSection(m_spawn.Texture);
        m_sectionA.SpawnSection(m_cellWidth, m_cellHeight, m_nextOrigin);
        m_nextOrigin.y = m_nextOrigin.y + ((m_sectionA.Height + 1) * m_cellHeight);

        //1st section
        m_sectionB = GenerateSection(SelectChunks(m_chunks));
        m_sectionB.SpawnSection(m_cellWidth, m_cellHeight, m_nextOrigin);
        m_nextOrigin.y = m_nextOrigin.y + ((m_sectionB.Height + 1) * m_cellHeight);

        m_currentBuffer = true;
    }

    /// <summary>
    /// Generates section froma single chunk image
    /// </summary>
    /// <param name="_chunk"></param>
    /// <returns></returns>
    private Section GenerateSection(Texture2D _chunk)
    {
        int width = _chunk.width;
        int height = _chunk.height;
        GameObject[,] grid = new GameObject[width, height];

        //Converts image's pixels in to equivalent block
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

    /// <summary>
    /// Generates section based on a list of chunk images
    /// </summary>
    /// <param name="_chunks"></param>
    /// <returns></returns>
    private Section GenerateSection(List<Texture2D> _chunks)
    {
        int width = 0;
        int height = 0;

        //Gets overall width and height of section
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

        //Converts image's pixels in to equivalent block
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

    /// <summary>
    /// Finds prefab cell based on the indetifier set in Level inspector
    /// </summary>
    /// <param name="_identifier"></param>
    /// <returns></returns>
    private GameObject GetCell(Color _identifier)
    {
        foreach(Cell template in m_cellTemplates)
        {
            if(template.Identifier == _identifier)
            {
                return template.Prefab;
            }
        }

        return m_defaultTemplate;
    }

    /// <summary>
    /// Generates a list of chunks based on entrances and exits
    /// </summary>
    /// <param name="_chunks"></param>
    /// <returns></returns>
    private List<Texture2D> SelectChunks(int _chunks)
    {
        List<Texture2D> chunks = new List<Texture2D>();

        int separatorIndex = UnityEngine.Random.Range(0, m_separators.Count);
        chunks.Add(m_separators[separatorIndex].Texture);
        int prevIndex = GetNextChunk(m_separators[separatorIndex].Exits);
        chunks.Add(m_chunkData[prevIndex].Texture);

        for (int iter = 1; iter <= _chunks - 3; iter++)
        {
            prevIndex = GetNextChunk(m_chunkData[prevIndex].Exits);
            chunks.Add(m_chunkData[prevIndex].Texture);
        }

        prevIndex = GetNextChunk(m_chunkData[prevIndex].Exits, m_separators[0].Entrances);
        chunks.Add(m_chunkData[prevIndex].Texture);

        return chunks;
    }

    /// <summary>
    /// Gets a suitable next chunk based on the exits from the previous chunk
    /// </summary>
    /// <param name="_exits"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Gets a suitable next chunk based on the exits from the previous chunk and entrance of next chunk
    /// </summary>
    /// <param name="_exits"></param>
    /// <param name="_entrances"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Determines whether chunks have compatible entrances and exits
    /// </summary>
    /// <param name="_entrances"></param>
    /// <param name="_exits"></param>
    /// <returns></returns>
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

    public bool CheckCellFilled(int _sectionIndex, GridCoordinates _coords)
    {
        if (m_sectionA.Index == _sectionIndex)
        {
            if (_coords.CheckValidCoords(m_sectionA.Width - 1, m_sectionA.Height - 1))
            {
                return m_sectionA.CheckCellFilled(_coords);
            }
        }
        else
        {
            if (_coords.CheckValidCoords(m_sectionB.Width - 1, m_sectionB.Height - 1))
            {
                return m_sectionB.CheckCellFilled(_coords);
            }
        }
        return true;
    }

    public void AddLava(int _sectionIndex, GridCoordinates _coords)
    {
        if (m_sectionA.Index == _sectionIndex)
        {
            if (_coords.CheckValidCoords(m_sectionA.Width - 1, m_sectionA.Height - 1))
            {
                m_sectionA.AddLava(_coords);
            }
        }
        else
        {
            if (_coords.CheckValidCoords(m_sectionB.Width - 1, m_sectionB.Height - 1))
            {
                m_sectionB.AddLava(_coords);
            }
        }
    }

    void LateUpdate()
    {
        if (m_currentBuffer)
        {
            m_sectionA.SpawnLava(m_lavaTemplate);
        }
        else
        {
            m_sectionB.SpawnLava(m_lavaTemplate);
        }
    }
}
