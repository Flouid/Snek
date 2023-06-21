using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum SegmentType
{
    None = 0,
    Head = 1,
    Body = 2,
    Turn = 3,
    Tail = 4
};

public class SnakeSegment : MonoBehaviour
{
    public SnakeSegment next, prev;
    public Vector2 pos, dir;
    public int index;
    public SegmentType segmentType;
    public SnakeSpriteController spriteController;
    
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

    // update bookkeeping for type segment and associated sprite based on movement of neighbors
    public void UpdateSegment()
    {
        // no orphaned segments allowed
        Assert.IsTrue(next != null || prev != null);
        // copy the type of the current segment to check if it changed later
        SegmentType currType = segmentType;
        
        // determine the correct segment type based on neighboring segments
        if (next == null) segmentType = SegmentType.Head;
        else if (prev == null) segmentType = SegmentType.Tail;
        else if (next.pos.x == prev.pos.x || next.pos.y == prev.pos.y) segmentType = SegmentType.Body;
        else segmentType = SegmentType.Turn;

        // stop here if the segment type didn't actually change
        if (currType == segmentType) return;
        Debug.Log("changing segment " + index + " from " + currType + " to " + segmentType);
        spriteController.SetSprite(segmentType);
    }

    // handle sprite translation for any frame partially between move steps
    public Vector2 Translate(Vector2 direction, float t)
    {
        // translation
        if (direction != dir) return dir;

        Vector2 lerp;
        // corner segments should be locked to the grid
        if (segmentType == SegmentType.Turn) lerp = Vector2.zero;
        // otherwise, scale the current direction by the lerp value to get a relative offset
        else lerp = dir * t;
        spriteController.TranslateSprite(lerp);

        return dir;
    }

    // snap sprite into rotation associated with new direction after move step
    public Vector2 Rotate(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(dir, direction);
        spriteController.SnapRotateSprite(angle);
        return dir;
    }
    
    public void ResetSpritePos() { spriteController.ResetSpritePos(); }

    void Start() { UpdateSegment(); }
    void Awake() { _trans = GetComponent<Transform>(); }
}