using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    //MOVEMENT

    //is the unit being selected (clicked)
    public bool selected;
    //the number of tiles can the unit move
    public int tileSpeed;
    //whether or not the unit has moved already
    public bool hasMoved;
    //how fast the unit moves between tiles (no gameplay effects)
    public float moveSpeed;

    //ATTACKING 

    //whether or not the unit has attacked already
    public bool hasAttacked;
    //range of attack
    public int attackRange;
    //icon to display that which enemies are able to be attacked (purely visual) 
    public GameObject weaponIcon;
    //list of units that this unit can attack 
    private List<Unit> enemiesInRange = new List<Unit>();



    //MISC

    //indicates which side the unit belongs to. 
    public int playerNumber;
    //game master object to have info about the game 
    private GameMaster gm;
    //whether or not the unit is moving
    public bool isMoving; 

    // Start is called before the first frame update
    private void Start() {
        gm = FindObjectOfType<GameMaster>(); 
    }

    //Logic for clicking on units 
    private void OnMouseDown() {

        ResetWeaponIcon();

        if(selected == true) {
            selected = false;
            gm.selectedUnit = null;
            gm.ResetTiles();
        } else {
            if(playerNumber == gm.playerTurn) {
                if (gm.selectedUnit != null) {
                    gm.selectedUnit.selected = false;
                }

                selected = true;
                gm.selectedUnit = this;
                gm.ResetTiles();

                GetEnemies();
                GetWalkableTiles();
            }

        }
        
    }



    //Calculates and displays which that the unit can move to
    private void GetWalkableTiles() {
        if(hasMoved == true) {
            return;
        }
        foreach (Tile tile in FindObjectsOfType<Tile>()) {

            if (Mathf.Abs(transform.position.x - tile.transform.position.x) + 
            Mathf.Abs(transform.position.y - tile.transform.position.y) <= tileSpeed) {
                if(tile.IsClear() == true)
                {
                    tile.Highlight();
                } else {
                    tile.Reset();
                }
            }
        }

    }

    //Moves the unit 
    public void Move(Vector2 tilePos) {
        gm.ResetTiles();
        StartCoroutine(StartMovement(tilePos));
    }
    
    //Animates the movement of the unit 
    private IEnumerator StartMovement(Vector2 tilePos) {



        //sets the unit as moving
        isMoving = true;
        //removes all the attack icons 
        ResetWeaponIcon();

        while (transform.position.x != tilePos.x) {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(tilePos.x, transform.position.y),
                                 moveSpeed * Time.deltaTime);
            yield return null;
        }

        while (transform.position.y != tilePos.y) {
            transform.position = Vector2.MoveTowards(transform.position, 
                                 new Vector2(transform.position.x, tilePos.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        hasMoved = true;

        //after the unit moves recalculate which enemies can be attacked. 
        GetEnemies();
        //set the unit as not moving
        isMoving = false;
    }

    //Calculates which enemies are in range of being attacked 
    private void GetEnemies()
    {
        enemiesInRange.Clear();

        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            //delta x + delta y <= AttackRange
            if (Mathf.Abs(transform.position.x - unit.transform.position.x) +
                    Mathf.Abs(transform.position.y - unit.transform.position.y) <= attackRange)
            {
                if(unit.playerNumber != gm.playerTurn && hasAttacked == false)
                {
                    enemiesInRange.Add(unit);
                    unit.weaponIcon.SetActive(true);
                }

            }

        }
    }

    public void ResetWeaponIcon()
    {
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            unit.weaponIcon.SetActive(false);
        }

    }


}
