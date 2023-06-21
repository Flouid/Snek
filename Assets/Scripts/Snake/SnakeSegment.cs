using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum SegmentType
{
    None,
    Head,
    Body,
    Turn,
    Tail
};

public class SnakeSegment : MonoBehaviour
{
    public SnakeSegment next, prev;
    public Vector2 pos, dir;
    public int index;
    public SegmentType segmentType;

    [SerializeField] private Sprite headSprite, bodySprite, turnSprite, tailSprite;
    
    private GameObject _spriteContainer;
    private SpriteRenderer _spriteRenderer;
    private Transform _trans;

    public void Init(SnakeSegment next, SnakeSegment prev, Vector2 pos, Vector2 dir, int index)
    {
        this.next = next;
        this.prev = prev;
        this.pos = pos;
        this.dir = dir;
        this.index = index;

        _trans.position = new Vector3(pos.x, pos.y, 0.0f);
    }

    // move the snake segment one unit in a given direction
    // returns the direction the segment was facing before moving
    public Vector2 Move(Vector2 direction)
    {
        Vector2 lastDir = dir;
        dir = direction;
        pos += direction;
        _trans.position = new Vector3(pos.x, pos.y, 0.0f);
        return lastDir;
    }

    public void UpdateSprite()
    {
        // no orphaned segments allowed
        Assert.IsTrue(next != null || prev != null);
        // copy the type of the current type to check if it changed
        SegmentType currType = segmentType;
        
        // determine the correct segment type based on neighboring segments
        if (next == null) segmentType = SegmentType.Head;
        else if (prev == null) segmentType = SegmentType.Tail;
        else if (next.pos.x == prev.pos.x || next.pos.y == prev.pos.y) segmentType = SegmentType.Body;
        else segmentType = SegmentType.Turn;

        // don't do anything if the type didn't change
        if (currType == segmentType) return;

        Debug.Log("changing segment " + index + " from " + currType + " to " + segmentType);

        // set the sprite accordingly
        switch(segmentType)
        {
            case (SegmentType.Head): _spriteRenderer.sprite = headSprite; return;
            case (SegmentType.Body): _spriteRenderer.sprite = bodySprite; return;
            case (SegmentType.Turn): _spriteRenderer.sprite = turnSprite; return;
            case (SegmentType.Tail): _spriteRenderer.sprite = tailSprite; return;
        }
    }

    // adjust the sprite relative to the segment to smoothly interpolate between grid positions
    public void Interpolate(float t)
    {
        Vector2 animAnchor = dir * t;
        _spriteContainer.transform.localPosition = new Vector3(animAnchor.x, animAnchor.y, 0.0f);
    }

    void Start()
    {
        UpdateSprite();
    }

    void Awake()
    {
        _trans = GetComponent<Transform>();
        _spriteContainer = transform.GetChild(0).gameObject;
        _spriteRenderer = _spriteContainer.GetComponent<SpriteRenderer>();
    }
}