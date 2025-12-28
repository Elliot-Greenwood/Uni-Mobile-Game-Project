using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class MineFieldManagerScript : MonoBehaviour
{
    SetLevelCompletion LevelCompletionScript;
    AudioSource SFXPlayer;
    [SerializeField] AudioSource MusicPlayer;

    public TileScript[] MineFieldTiles;
    public int SetAmmountOfMines = 0;
    public int MinesFlaggedCorrectly = 0;
    public int Flags = 0;
    public int CoinsCollected = 0;

    public bool MinesHaveBeenRandomized = false;
    public bool IsGameComplete = false;

    [SerializeField] GameObject PlayerFlag;


    [SerializeField] GameObject InputUIHUD;

    [SerializeField] GameObject LevelCompleteUI;
    [SerializeField] GameObject LevelFailedUI;

    [SerializeField] Text FlagsTextUI = null;
    [SerializeField] Text CoinsTextUI = null;


    [SerializeField] AudioClip WinSound, LoseSound;


    private void OnEnable()
    {
        ActionsListener.OnFlagPlaceOnMine += Mine_Was_Flagged_Correctly;
        ActionsListener.OnFlagRemoveOffMine += Mine_Was_Unflagged;

        ActionsListener.OnFlagPlace += Tile_Flagged;
        ActionsListener.OnFlagRemove += Tile_Unflagged;

        ActionsListener.OnDiggingUpMine += Mine_Was_Dug_Up;

        ActionsListener.OnCoinCollected += Collected_A_Coin;
    }
    private void OnDisable()
    {
        ActionsListener.OnFlagPlaceOnMine -= Mine_Was_Flagged_Correctly;
        ActionsListener.OnFlagRemoveOffMine -= Mine_Was_Unflagged;

        ActionsListener.OnFlagPlace -= Tile_Flagged;
        ActionsListener.OnFlagRemove -= Tile_Unflagged;

        ActionsListener.OnDiggingUpMine -= Mine_Was_Dug_Up;

        ActionsListener.OnCoinCollected -= Collected_A_Coin;
    }








    void Start()
    {
        SFXPlayer = GetComponent<AudioSource>();
        LevelCompletionScript = GetComponent<SetLevelCompletion>();
        Flags = SetAmmountOfMines;
        FlagsTextUI.text = Flags.ToString();
        CoinsCollected = 0;
        CoinsTextUI.text = CoinsCollected.ToString();

        if (PlayerPrefs.GetInt("AudioINT") == 0)
        {
            MusicPlayer.Play();
        }
        StartCoroutine(DiplayBannerAdds());
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
        if (PlayerPrefs.GetInt("VibrationINT") == 0)
        {
            PhoneVibration.FlagPlantVibration();
        }

        if (!IsGameComplete && SetAmmountOfMines == MinesFlaggedCorrectly) //Game Win
        {
            IsGameComplete = true;
            Invoke("InvokeLevelCompleteUI", 1f);
            InputUIHUD.SetActive(false);
            ActionsListener.OnAllMinesFlaggedCorrectly();
        }
    }
    void InvokeLevelCompleteUI()
    {
        SFXPlayer.PlayOneShot(WinSound, 0.5f);

        LevelCompleteUI.SetActive(true);
        LevelCompletionScript.CompleteLevel();

        PlayerPrefs.SetInt("GameCurrency", PlayerPrefs.GetInt("GameCurrency") + CoinsCollected);
        PlayerPrefs.Save();
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
        Invoke("InvokeDeathUI", 1f);

        if (PlayerPrefs.GetInt("VibrationINT") == 0) 
        { 
            PhoneVibration.ExplosionVibration(); 
        }
        
        InputUIHUD.SetActive(false);

        
        AddManager.Instance.BannerAd.HideBannerAd();
    }

    void InvokeDeathUI()
    {
        SFXPlayer.PlayOneShot(LoseSound, 0.75f);

        LevelFailedUI.SetActive(true);

        //add code to reduce this ad frequincy or completely remove.
        //replace it with reward ad
        AddManager.Instance.InterstitialAd.ShowAd();
    }


    void Collected_A_Coin()
    {
        CoinsCollected++;
        CoinsTextUI.text = CoinsCollected.ToString();

    }



    IEnumerator DiplayBannerAdds()
    {
        yield return new WaitForSeconds(1f);
        AddManager.Instance.BannerAd.ShowBannerAd();
    }

}
