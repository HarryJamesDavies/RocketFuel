using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepingLava : MonoBehaviour
{
    public enum CellState
    {
        Unchecked,
        Filled,
        Empty
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Null
    }

    [Serializable]
    public class DirectionData
    {
        public Direction Direction;
        public GridCoordinates Coords;

        public DirectionData(Direction _direction, GridCoordinates _coords)
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

        public void CheckUnchecked(int _sectionIndex, GridCoordinates _coords)
        {
            Up = BoolToState(LevelManager.Instance.CheckCellFilled(_sectionIndex, _coords.GetUp()));
            Right = BoolToState(LevelManager.Instance.CheckCellFilled(_sectionIndex, _coords.GetRight()));
            Down = BoolToState(LevelManager.Instance.CheckCellFilled(_sectionIndex, _coords.GetDown()));
            Left = BoolToState(LevelManager.Instance.CheckCellFilled(_sectionIndex, _coords.GetLeft()));

            GeneratePriorityList(_coords);
        }

        private void GeneratePriorityList(GridCoordinates _coords)
        {
            if(Down == CellState.Empty)
            {
                PriorityList.Add(new DirectionData(Direction.Down, _coords.GetDown()));
                Down = CellState.Filled;
            }

            if (Left == CellState.Empty)
            {
                PriorityList.Add(new DirectionData(Direction.Left, _coords.GetLeft()));
                Left = CellState.Filled;
            }

            if (Right == CellState.Empty)
            {
                PriorityList.Add(new DirectionData(Direction.Right, _coords.GetRight()));
                Right = CellState.Filled;
            }

            if (Up == CellState.Empty)
            {
                PriorityList.Add(new DirectionData(Direction.Up, _coords.GetUp()));
                Up = CellState.Filled;
            }
        }

        private CellState BoolToState(bool _state)
        {
            if(_state)
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

        public Direction PeekPriorityDirection()
        {
            if (PriorityList.Count != 0)
            {
                return PriorityList[0].Direction;
            }
            return Direction.Null;
        }
    }

    public TraversableDirections m_traversableDirections = new TraversableDirections();
    public float m_wait = 5.0f;

    private bool m_firstFrame = true;
    private bool m_processing = false;

    private CellData m_data = null;

    void Start()
    {
        m_data = GetComponent<CellData>();
    }

    void Update()
    {
        if (m_firstFrame)
        {
            m_firstFrame = false;
            m_traversableDirections.CheckUnchecked(m_data.m_sectionIndex, m_data.m_coords);
        }
        else if (!m_processing)
        {
            m_processing = true;
            StartCoroutine(DelaySpawn());
        }
    }

    private IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(m_wait);
        SpawnAdjacentCell();
        CheckSurrounded();
        m_processing = false;
    }

    private void SpawnAdjacentCell()
    {
        DirectionData data = m_traversableDirections.GetPriority();
        if (data != null)
        {
            LevelManager.Instance.AddLava(m_data.m_sectionIndex, data.Coords);

            if (data.Direction == Direction.Left && m_traversableDirections.PeekPriorityDirection() == Direction.Right)
            {
                data = m_traversableDirections.GetPriority();
                LevelManager.Instance.AddLava(m_data.m_sectionIndex, data.Coords);
            }
        }
    }

    private void CheckSurrounded()
    {
        if(m_traversableDirections.PriorityList.Count == 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().Sleep();
            enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.tag == "Player")
        {
            GlobalEventBoard.Instance.AddEvent(Events.Event.GLO_PlayerDied);
        }
    }
}
