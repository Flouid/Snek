using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum SegmentType
{
    head,
    body,
    turn,
    tail
};

public class SnakeSegment : MonoBehaviour
{
    public SnakeSegment _next, _prev;
    public GridPosition _pos;
    public int _index;
    public SegmentType _segmentType;

    [SerializeField] private Sprite headSprite, bodySprite, turnSprite, tailSprite;
    
    private GameObject _spriteContainer;
    private SpriteRenderer _spriteRenderer;
    private Transform _trans;

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
        // no orphaned segments allowed
        Assert.IsTrue(_next != null || _prev != null);
        
        // determine the correct segment type based on neighboring segments
        if (_next == null) _segmentType = SegmentType.head;
        else if (_prev == null) _segmentType = SegmentType.tail;
        else if (GridPosition.InALine(_next._pos, _prev._pos)) _segmentType = SegmentType.body;
        else _segmentType = SegmentType.turn;

        // set the sprite accordingly
        switch(_segmentType)
        {
            case (SegmentType.head): _spriteRenderer.sprite = headSprite; break;
            case (SegmentType.body): _spriteRenderer.sprite = bodySprite; break;
            case (SegmentType.turn): _spriteRenderer.sprite = turnSprite; break;
            case (SegmentType.tail): _spriteRenderer.sprite = tailSprite; break;
        }
    }

    void Awake()
    {
        _trans = GetComponent<Transform>();
        _spriteContainer = transform.GetChild(0).gameObject;
        _spriteRenderer = _spriteContainer.GetComponent<SpriteRenderer>();
    }
}