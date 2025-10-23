using UnityEngine;
using UnityEngine.UI;

public class MineFieldManagerScript : MonoBehaviour
{
    public TileScript[] MineFieldTiles;
    public int TotalMinesInMap = 0;
    public int MinesFlaggedCorrectly = 0;
    public int Flags = 0;


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



        foreach (TileScript Tile in MineFieldTiles)
        {
            Tile.TileIsBomb = Random.value < 0.15f; // 0.0f to 1.0f => 0% to 100% chance.
        }

        foreach (TileScript Tile in MineFieldTiles)
        {
            if (Tile.TileIsBomb)
            {
                TotalMinesInMap++;
            }
        }

        Flags = TotalMinesInMap;
        FlagsTextUI.text = Flags.ToString();
        

    }


    private void Update()
    {
        if (!IsGameComplete &&TotalMinesInMap == MinesFlaggedCorrectly)
        {
            IsGameComplete = true;
            GameCompleteUI.SetActive(true);
            //Game Win
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
