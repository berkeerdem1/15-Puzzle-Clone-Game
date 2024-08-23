using UnityEngine;


public class Tiles : MonoBehaviour
{
    public bool inRightPlace = false;
    public int number;
    public Vector2 targetPos;

    private Vector2 correctPos; // True Pos
    private SpriteRenderer mySprite;

    private void Awake()
    {
        mySprite = GetComponent<SpriteRenderer>();
        targetPos = transform.position;
        correctPos = transform.position;
    }
    void FixedUpdate()
    {
        // Reaches the target smoothly
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f); 

        if(correctPos == targetPos) // Change color if in correct position
        {
            mySprite.color = Color.blue;
            inRightPlace = true;
        }
        else
        {
            mySprite.color = Color.white;
            inRightPlace = false;
        }
    }
}
