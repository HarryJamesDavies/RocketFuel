using System;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public enum CellState
    {
        Unchecked,
        Filled,
        Empty
    }

    [Serializable]
    public class DirectionData
    {
        public Direction.Directions Direction;
        public GridCoordinates Coords;

        public DirectionData(Direction.Directions _direction, GridCoordinates _coords)
        {
            Direction = _direction;
            Coords = _coords;
        }
        public DirectionData(DirectionData _data)
        {
            Direction = _data.Direction;
            Coords = _data.Coords;
        }

    }

    [Serializable]
    public class TraversableDirections
    {
        public CellState Up = CellState.Unchecked;
        public CellState Right = CellState.Unchecked;
        public CellState Down = CellState.Unchecked;
        public CellState Left = CellState.Unchecked;

        public List<DirectionData> PriorityList = new List<DirectionData>();

        public CellState CheckDirection(Direction.Directions _direction, GridCoordinates _coords)
        {
            CellState state = CellState.Filled;
            switch(_direction)
            {
                case Direction.Directions.Up:
                    {
                        state = ContentToState(LevelManager.Instance.CheckCellContent(_coords.GetUp()));
                        break;
                    }
                case Direction.Directions.Right:
                    {

                        state = ContentToState(LevelManager.Instance.CheckCellContent(_coords.GetRight()));
                        break;
                    }
                case Direction.Directions.Down:
                    {
                        state = ContentToState(LevelManager.Instance.CheckCellContent(_coords.GetDown()));
                        break;
                    }
                case Direction.Directions.Left:
                    {

                        state = ContentToState(LevelManager.Instance.CheckCellContent(_coords.GetLeft()));
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return state;
        }

        public void CheckUnchecked(int _sectionIndex, GridCoordinates _coords)
        {
            Up = ContentToState(LevelManager.Instance.CheckCellContent(_sectionIndex, _coords.GetUp()));
            Right = ContentToState(LevelManager.Instance.CheckCellContent(_sectionIndex, _coords.GetRight()));
            Down = ContentToState(LevelManager.Instance.CheckCellContent(_sectionIndex, _coords.GetDown()));
            Left = ContentToState(LevelManager.Instance.CheckCellContent(_sectionIndex, _coords.GetLeft()));

            GeneratePriorityList(_coords);
        }

        private void GeneratePriorityList(GridCoordinates _coords)
        {
            if (Down == CellState.Empty)
            {
                PriorityList.Add(new DirectionData(Direction.Directions.Down, _coords.GetDown()));
                Down = CellState.Filled;
            }

            if (Left == CellState.Empty)
            {
                PriorityList.Add(new DirectionData(Direction.Directions.Left, _coords.GetLeft()));
                Left = CellState.Filled;
            }

            if (Right == CellState.Empty)
            {
                PriorityList.Add(new DirectionData(Direction.Directions.Right, _coords.GetRight()));
                Right = CellState.Filled;
            }

            if (Up == CellState.Empty)
            {
                PriorityList.Add(new DirectionData(Direction.Directions.Up, _coords.GetUp()));
                Up = CellState.Filled;
            }
        }

        private CellState ContentToState(CellData.CellContent _content)
        {
            if (_content == CellData.CellContent.Solid || _content == CellData.CellContent.Liquid)
            {
                return CellState.Filled;
            }
            return CellState.Empty;
        }

        public DirectionData GetPriority()
        {
            if (PriorityList.Count != 0)
            {
                DirectionData result = new DirectionData(PriorityList[0]);
                PriorityList.RemoveAt(0);
                return result;
            }
            return null;
        }

        public Direction.Directions PeekPriorityDirection()
        {
            if (PriorityList.Count != 0)
            {
                return PriorityList[0].Direction;
            }
            return Direction.Directions.Null;
        }

        public bool CheckSurrounded()
        {
            return (PriorityList.Count == 0);
        }
    }

    public TraversableDirections m_traversableDirections = new TraversableDirections();
    protected CellData m_data = null;

    void Awake()
    {
        m_data = GetComponent<CellData>();
    }
}
