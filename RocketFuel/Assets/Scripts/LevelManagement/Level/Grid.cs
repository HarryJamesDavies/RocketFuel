using System;
using UnityEngine;

[Serializable]
public class Grid : ScriptableObject
{
    public GameObject[,] CurrentData;
    public GameObject[,] NextData;

    public int Index;
    public int Width;
    public int Height;

    private Transform Holder;
    private float CellWidth;
    private float CellHeight;
    private Vector3 Origin;

    public void Init(int _index, int _width, int _height, GameObject[,] _content, Transform _parent)
    {
        Index = _index;
        CurrentData = _content;
        Width = _width - 1;
        Height = _height - 1;
        Holder = new GameObject("Section " + _index + " Holder: ").transform;
        Holder.SetParent(_parent);

        NextData = new GameObject[_width, _height];
        for (int y = 0; y <= Height; y++)
        {
            for (int x = 0; x <= Width; x++)
            {
                NextData[x, y] = null;
            }
        }
    }

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
                CurrentData[x, y] = Instantiate(CurrentData[x, y], pos, Quaternion.identity);
                CurrentData[x, y].transform.SetParent(Holder);
                CurrentData[x, y].GetComponent<CellData>().Initialise(Index, new GridCoordinates(x, y));
            }
        }
    }

    public void AddCell(GameObject _cell, GridCoordinates _coords)
    {
        Vector3 pos = CurrentData[_coords.X, _coords.Y].transform.position;
        Destroy(CurrentData[_coords.X, _coords.Y]);

        CurrentData[_coords.X, _coords.Y] = Instantiate(_cell, pos, Quaternion.identity);
        CurrentData[_coords.X, _coords.Y].transform.SetParent(Holder);
        CurrentData[_coords.X, _coords.Y].GetComponent<CellData>().Initialise(Index, new GridCoordinates(_coords.X, _coords.Y));
    }

    public void AddDelayedCell(GameObject _cell, GridCoordinates _coords)
    {
        NextData[_coords.X, _coords.Y] = _cell;
    }

    public void OverrideCell(GameObject _cell, GridCoordinates _coords)
    {
        Vector3 pos = CurrentData[_coords.X, _coords.Y].transform.position;
        Destroy(CurrentData[_coords.X, _coords.Y]);

        CurrentData[_coords.X, _coords.Y] = _cell;
        CurrentData[_coords.X, _coords.Y].transform.position = pos;
        CurrentData[_coords.X, _coords.Y].transform.SetParent(Holder);
        CurrentData[_coords.X, _coords.Y].GetComponent<CellData>().Initialise(Index, new GridCoordinates(_coords.X, _coords.Y));
    }

    public GameObject GetCell(GridCoordinates _coords)
    {
        return CurrentData[_coords.X, _coords.Y];
    }

    public void LateUpdate()
    {
        for (int y = 0; y <= Height; y++)
        {
            for (int x = 0; x <= Width; x++)
            {
                if (NextData[x, y])
                {
                    Vector3 pos = CurrentData[x, y].transform.position;
                    Destroy(CurrentData[x, y]);

                    CurrentData[x, y] = Instantiate(NextData[x, y], pos, Quaternion.identity);
                    CurrentData[x, y].transform.SetParent(Holder);
                    CurrentData[x, y].GetComponent<CellData>().Initialise(Index, new GridCoordinates(x, y));

                    NextData[x, y] = null;
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
                Destroy(CurrentData[x, y]);
            }
        }

        Destroy(Holder.gameObject);
    }

    public CellData.CellContent CheckCellContent(GridCoordinates _coords)
    {
        return CurrentData[_coords.X, _coords.Y].GetComponent<CellData>().m_content;
    }
}
