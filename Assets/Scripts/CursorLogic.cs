using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLogic : MonoBehaviour
{


    public Sprite[] cursorGraphics;

    private SpriteRenderer rend;
    private GameMaster gm;


    // Start is called before the first frame update
    void Start()
    {
        //sets the default white cursor to be invisible
        Cursor.visible = false;

        rend = GetComponent<SpriteRenderer>();
        gm = FindObjectOfType<GameMaster>();

    }

    // Update is called once per frame
    void Update()
    {
        //sets my custom cursor to follow the invisible cursor 
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPosition;

        rend.sprite = cursorGraphics[gm.playerTurn];
        
    }
}
