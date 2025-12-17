using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class MineFieldManagerScript : MonoBehaviour
{
    public TileScript[] MineFieldTiles;
    public int SetAmmountOfMines = 0;
    public int MinesFlaggedCorrectly = 0;
    public int Flags = 0;

    public bool MinesHaveBeenRandomized = false;
    public bool IsGameComplete = false;

    [SerializeField] GameObject PlayerFlag;


    [SerializeField] GameObject InputUIHUD;

    [SerializeField] GameObject LevelCompleteUI;
    [SerializeField] GameObject LevelFailedUI;

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
        Flags = SetAmmountOfMines;
        FlagsTextUI.text = Flags.ToString();
    }


    private void Update()
    {
       
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
        PhoneVibration.FlagPlantVibration();

        if (!IsGameComplete && SetAmmountOfMines == MinesFlaggedCorrectly) //Game Win
        {
            IsGameComplete = true;
            Invoke("InvokeLevelCompleteUI", 3f);
            InputUIHUD.SetActive(false);
            ActionsListener.OnAllMinesFlaggedCorrectly();
        }
    }
    void InvokeLevelCompleteUI()
    {
        LevelCompleteUI.SetActive(true);
    }


    void Mine_Was_Unflagged()
    {
        MinesFlaggedCorrectly--;
    }
    void Tile_Flagged()
    {
        
        Flags--;
        FlagsTextUI.text = Flags.ToString();
        if (Flags == 0)
        {
            PlayerFlag.SetActive(false);
        }

    }
    void Tile_Unflagged()
    {
        Flags++;
        FlagsTextUI.text = Flags.ToString();
        if (Flags > 0)
        {
            PlayerFlag.SetActive(true);
        }
    }

    void Mine_Was_Dug_Up()
    {
        //HANDLE DEATH HERE


        IsGameComplete = true;
        Invoke("InvokeDeathUI", 3f);
        PhoneVibration.ExplosionVibration();
        InputUIHUD.SetActive(false);
    }

    void InvokeDeathUI()
    {
        LevelFailedUI.SetActive(true);
    }


}
