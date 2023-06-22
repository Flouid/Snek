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
    // rotates the segment to face to new direction
    // returns the direction the segment was facing before moving
    public Vector2 Move(Vector2 direction)
    {
        Vector2 lastDir = dir;
        // translate
        dir = direction;
        pos += direction;
        _trans.position = new Vector3(pos.x, pos.y, 0.0f);
        // rotate
        _trans.Rotate(new Vector3(0.0f, 0.0f, Vector2.SignedAngle(lastDir, direction)));
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

    // handle sprite animation for any frame partially between move steps
    public Vector2 Animate(Vector2 direction, float t)
    {
        // don't animate corner segments
        if (segmentType == SegmentType.Turn) return dir;

        if (direction == dir) TranslateSprite(t);
        else RotateSprite(direction, t);

        return dir;
    }
    
    public void ResetSprite() { spriteController.ResetSpriteTransform(); }

    void RotateSprite(Vector2 direction, float t)
    {
        float angle = Vector2.SignedAngle(dir, direction) * t;
        spriteController.RotateSprite(angle);
    }

    void TranslateSprite(float t) {
        // instead of using the local transform, use the absolute as the frame of reference does not rotate
        Vector2 lerp = new Vector2(_trans.position.x, _trans.position.y) + dir * t;
        spriteController.TranslateSprite(lerp);
    }

    void Start() { UpdateSegment(); }
    void Awake() { _trans = GetComponent<Transform>(); }
}