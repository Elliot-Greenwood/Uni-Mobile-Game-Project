using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MineFieldManagerScript : MonoBehaviour
{
    public TileScript[] MineFieldTiles;
    public int SetAmmountOfMines = 12;
    public int MinesFlaggedCorrectly = 0;
    public int Flags = 0;

    public bool MinesHaveBeenRandomized = false;
    bool IsGameComplete = false;
    bool IsGameOver = false;

    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject GameCompleteUI;

    [SerializeField] Text FlagsTextUI = null;


    private void OnEnable()
    {
        ActionsListener.OnFlagPlaceOnMine += Mine_Was_Flagged_Correctly;
        ActionsListener.OnFlagRemoveOffMine += Mine_Was_Unflagged;

        ActionsListener.OnFlagPlace += Tile_Flagged;
        ActionsListener.OnFlagRemove += Tile_Unflagged;

        ActionsListener.OnDiggingUpMine += Mine_Was_Dug_Up;
    }
    private void OnDisable()
    {
        ActionsListener.OnFlagPlaceOnMine -= Mine_Was_Flagged_Correctly;
        ActionsListener.OnFlagRemoveOffMine -= Mine_Was_Unflagged;

        ActionsListener.OnFlagPlace -= Tile_Flagged;
        ActionsListener.OnFlagRemove -= Tile_Unflagged;

        ActionsListener.OnDiggingUpMine -= Mine_Was_Dug_Up;
    }








    void Start()
    {
        GameOverUI.SetActive(false);
        GameCompleteUI.SetActive(false);

        Flags = SetAmmountOfMines;
        FlagsTextUI.text = Flags.ToString();
    }


    private void Update()
    {
        if (!IsGameComplete && SetAmmountOfMines == MinesFlaggedCorrectly)
        { 
            IsGameComplete = true;
            GameCompleteUI.SetActive(true);
            //Game Win

            ActionsListener.OnAllMinesFlaggedCorrectly();
        }
    }


    public void RandomizeTheMineField(TileScript FirstActivatedTile)
    {
        if (MinesHaveBeenRandomized) return;

        MinesHaveBeenRandomized = true;

        List<TileScript> RemainingTiles = new List<TileScript>(MineFieldTiles);
        RemainingTiles.Remove(FirstActivatedTile);

        for (int i = 0; i < RemainingTiles.Count; i++)
        {
            TileScript Temp = RemainingTiles[i];
            int RNG = Random.Range(i, RemainingTiles.Count);
            RemainingTiles[i] = RemainingTiles[RNG];
            RemainingTiles[RNG] = Temp;
        }


        int HowManyMinesToPlace = Mathf.Clamp(SetAmmountOfMines, 0, RemainingTiles.Count);

        for (int i = 0; i < HowManyMinesToPlace; i++)
        {
            RemainingTiles[i].TileIsBomb = true;
        }


        Flags = SetAmmountOfMines;
        FlagsTextUI.text = Flags.ToString();


        foreach (TileScript tile in MineFieldTiles)
        {
            tile.InitTileMaterialCheck();
        }

    }







    void Mine_Was_Flagged_Correctly()
    {
        MinesFlaggedCorrectly++;
    }

    void Mine_Was_Unflagged()
    {
        MinesFlaggedCorrectly--;
    }
    void Tile_Flagged()
    {
        
        Flags--;
        FlagsTextUI.text = Flags.ToString();
        
    }
    void Tile_Unflagged()
    {
        Flags++;
        FlagsTextUI.text = Flags.ToString();
    }

    void Mine_Was_Dug_Up()
    {
        GameOverUI.SetActive(true);
    }


}
