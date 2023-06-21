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
    public SnakeSegment next, prev;
    public GridPosition pos;
    public int index;
    public SegmentType segmentType;

    [SerializeField] private Sprite headSprite, bodySprite, turnSprite, tailSprite;
    
    private GameObject _spriteContainer;
    private SpriteRenderer _spriteRenderer;
    private Transform _trans;

    public void Init(SnakeSegment next, SnakeSegment prev, GridPosition pos, int index)
    {
        this.next = next;
        this.prev = prev;
        this.pos = pos;
        this.index = index;

        _trans.position = new Vector3(pos.x, pos.y, 0.0f);
    }

    void Start()
    {
        UpdateSprite();
    }

    // move the snake segment one unit in a given direction
    void Move(Direction dir)
    {
        
    }

    void UpdateSprite()
    {
        // no orphaned segments allowed
        Assert.IsTrue(next != null || prev != null);
        
        // determine the correct segment type based on neighboring segments
        if (next == null) segmentType = SegmentType.head;
        else if (prev == null) segmentType = SegmentType.tail;
        else if (GridPosition.InALine(next.pos, prev.pos)) segmentType = SegmentType.body;
        else segmentType = SegmentType.turn;

        // set the sprite accordingly
        switch(segmentType)
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