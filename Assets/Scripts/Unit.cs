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

    //COMBAT 

    //whether or not the unit has attacked already
    public bool hasAttacked;
    //range of attack
    public int attackRange;
    //icon to display that which enemies are able to be attacked (purely visual) 
    public GameObject weaponIcon;
    //list of units that this unit can attack 
    private List<Unit> enemiesInRange = new List<Unit>();
    
    //how much health the unit has 
    public int health;
    //how much damage the unit can deal
    public int attackDamage;
    //how much damage the unit deals when it is attacked
    public int defenseDamage;
    //how much damage the unit mitigates
    public int armor;


    public int testing;
    public int testpart2;
    public int testpart3;


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

        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f);
        Unit unit = col.GetComponent<Unit>();
        if(gm.selectedUnit != null)
        {
            if (gm.selectedUnit.enemiesInRange.Contains(unit) && gm.selectedUnit.hasAttacked == false)
            {
                gm.selectedUnit.Attack(unit);
            }
        }

        
    }


    private void Attack(Unit enemy)
    {
        hasAttacked = true;

        //how much damage will be inflicted to the enemy
        int enemyDamage = attackDamage - enemy.armor;
        //how much damge will be inflicted to the attacking unit 
        int myDamage = enemy.defenseDamage - this.armor;

        if(enemyDamage >= 1)
        {
            enemy.health -= enemyDamage;
        }
        if (myDamage >= 1)
        {
            this.health -= myDamage;
        }

        if(enemy.health <= 0)
        {
            Destroy(enemy.gameObject);
            GetWalkableTiles();
        }

        if(this.health <= 0)
        {
            gm.ResetTiles();
            Destroy(this);
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
