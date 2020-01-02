using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public Unit selectedUnit;

    public int playerTurn = 0;


    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) { 
            EndTurn();
        }
    }

    private void EndTurn() {

        //switches between the turn of player 1 and 2
        if (playerTurn == 0) {
            playerTurn = 1;
        } else if (playerTurn == 1) {
            playerTurn = 0;
        }

        //resets the selected unit 
        if (selectedUnit != null) {
            selectedUnit.selected = false;
            selectedUnit = null;
        }
        ResetTiles();

        //resets all the units  
        foreach(Unit unit in FindObjectsOfType<Unit>()) {
            unit.hasMoved = false;
            unit.weaponIcon.SetActive(false);
            unit.hasAttacked = false;
        }

    }

    public void ResetTiles() {
        foreach(Tile tile in FindObjectsOfType<Tile>()) {
            tile.Reset();
        }
    }
}
