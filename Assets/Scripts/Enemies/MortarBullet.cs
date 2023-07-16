using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarBullet : Bullet
{
    private float heightPos;
    private float maxHeightPos;
    private float heightStep;
    private float timeToTarget;
    private Vector2 initialPos;
    private Vector2 targetPos;
    private Rigidbody2D rigid;
    private bool falling;
    [SerializeField] private float heightDiffTolerance;
    [SerializeField] private Transform projectileSpriteTransform;
    
    private void Start()
    {
        initialPos = gameObject.transform.position;
        falling = false;
        rigid = GetComponent<Rigidbody2D>();
        float distance = Vector2.Distance(initialPos, targetPos);
        float fullDistance = distance / (2*maxHeightPos - heightPos) * heightPos + distance;
        rigid.velocity = (targetPos - initialPos).normalized * distance / timeToTarget;
        heightStep = maxHeightPos / (fullDistance * 0.5f) * rigid.velocity.magnitude * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        if(!falling)
        {
            heightPos += heightStep;
            if (heightPos >= maxHeightPos)
            {
                falling = true;
            }
        }
        else
        {
            heightPos -= heightStep;
        }

        if(heightPos <= 0)
        {
            Destroy(gameObject);
        }
        SetSpriteYPos();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        GameObject col = collider.gameObject;
        //print("Height: "+(heightPos - col.GetComponentInParent<Statistics>().GetHeight()));
        if (col.CompareTag(targetTag))
        {
            float heightDiff = heightPos - col.GetComponentInParent<Statistics>().GetHeight();
            //print("Diff: "+heightDiff);
            if (heightDiff <= heightDiffTolerance)
            {
                OnHitDecision(col);
            }
        }
    }

    public void SetSpriteYPos()
    {
        projectileSpriteTransform.position = new Vector2(projectileSpriteTransform.position.x, gameObject.transform.position.y + heightPos);
    }

    public float GetMaxHeightPos()
    {
        return maxHeightPos;
    }

    public void SetMaxHeightPos(float height)
    {
        maxHeightPos = height;
    }

    public float GetHeightPos()
    {
        return heightPos;
    }

    public void SetHeightPos(float height)
    {
        heightPos = height;
    }

    public float GetTimeToTarget()
    {
        return timeToTarget;
    }

    public void SetTimeToTarget(float time)
    {
        timeToTarget = time;
    }

    public Vector2 GetTargetPos()
    {
        return targetPos;
    }

    public void SetTargetPos(Vector2 targetPos)
    {
        this.targetPos = targetPos;
    }
}
