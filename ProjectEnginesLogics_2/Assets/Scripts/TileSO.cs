using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Tiles can either be Safe or a Bomb, Bomb tiles will end the game if clicked
[SerializeField]
public enum Type
{
    Safe,
    Bomb
}
[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Create TileSO", order = 1)]
public class TileSO : ScriptableObject
{
    public Type type;
    public GameObject tilePrefab;
    // Will display the number of bombs touching the safe tile
    public TextMeshProUGUI adjacentTMP;
    public Image spriteHolder;
    // Will need 3 sprites: Bomb, Unclicked & Marked
    public List<Sprite> tileSprites;
    public List<TileSO> neighbours;
    public int numOfAdjacent;
    //Tiles X and Z coordanates
    public int coordX;
    public int coordZ;
    // Will change the sprite and stop it from being reclicked
    public bool isClicked = false;
    // Will change the sprite and stop it from being accidentally clicked
    public bool isMarked = false;
}
