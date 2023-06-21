using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    public SnakeSegment _next, _prev;
    public GridPosition _pos;
    public int _index;

    [SerializeField] private Sprite head, body, turn, tail;

    private Transform _trans;
    private SpriteRenderer _spriteRenderer;

    public void Init(SnakeSegment next, SnakeSegment prev, GridPosition pos, int index)
    {
        _next = next;
        _prev = prev;
        _pos = pos;
        _index = index;

        _trans.position = new Vector3(pos.x, pos.y, 0.0f);
    }

    void Start()
    {
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (_next == null && _prev == null) Debug.Log("attempted to update sprite on orphan segment");
        else if (_next == null) _spriteRenderer.sprite = head;
        else if (_prev == null) _spriteRenderer.sprite = tail;
        else if (GridPosition.InALine(_next._pos, _prev._pos)) _spriteRenderer.sprite = body;
        else _spriteRenderer.sprite = turn;

        // TODO: rotation logic
    }

    void Awake()
    {
        _trans = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}