using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    public SnakeSegment _next, _prev;
    public GridPosition _pos;
    public int _index;

    private Transform _trans;

    public void Init(SnakeSegment next, SnakeSegment prev, GridPosition pos, int index)
    {
        _next = next;
        _prev = prev;
        _pos = pos;
        _index = index;

        _trans.position = new Vector3(pos.x, pos.y, 0.0f);
    }
    
    void Awake()
    {
        _trans = GetComponent<Transform>();
    }
}