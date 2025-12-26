using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    MineFieldManagerScript MineManagerScript;
    AudioSource SFXPlayer;

    [Header("Tile Settings")]
    public bool TileIsBomb = false;
    public bool TileIsFlagged = false;
    public bool TileIsActivated = false;
    public bool TileIsComplete = false;

    public int HowManyMinesInArea = 0;

    [Header("Lists")]
    public List<TileScript> SurroundingTiles = new List<TileScript>();
    public Material[] TileMaterial;
    public Material MineMat;

    [Header("Other")]
    public GameObject TileCover;
    public GameObject Flag;
    public GameObject Mine;
    public GameObject Coin;

    public Renderer TileCoverMaterialRendered;
    public Material TileCoverMatIdle;
    public Material TileCoverMatOnstep;

    [SerializeField] Renderer MaterialRendered;

    [SerializeField] AudioClip DigDirtSoundEffect;

    void OnEnable()
    {
        ActionsListener.OnAllMinesFlaggedCorrectly += OpenAllMinesAfterWin;
    }
    void OnDisable()
    {
        ActionsListener.OnAllMinesFlaggedCorrectly -= OpenAllMinesAfterWin;
    }




    private void Start()
    {
        MineManagerScript = GameObject.FindWithTag("MineFieldManager").GetComponent<MineFieldManagerScript>();
        SFXPlayer = GetComponent<AudioSource>();


        //Flag.SetActive(false);
        //Mine.SetActive(false);

    }


    public void InitTileMaterialCheck()
    {
        if (!TileIsBomb)
        {
            CheckTileState();
        }
        else
        {
            MaterialRendered.material = MineMat;
        }
    }


    void CheckTileState()
    {
        HowManyMinesInArea = 0;

        foreach (TileScript tile in SurroundingTiles)
        {
            if (tile.TileIsBomb)
            {
                HowManyMinesInArea++;
            }

            MaterialRendered.material = TileMaterial[HowManyMinesInArea];
        }
        
    }



    private void OnTriggerEnter(Collider other)
    {
        TileScript tile = other.GetComponent<TileScript>();
        if (tile && !SurroundingTiles.Contains(tile))
        {
            SurroundingTiles.Add(tile);
        }


        if (other.gameObject.layer == 3)
        {
            TileCoverMaterialRendered.material = TileCoverMatOnstep;
           
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            TileCoverMaterialRendered.material = TileCoverMatIdle;
        }
    }





    public void ActivateTheTile()
    {
        SFXPlayer.pitch = Random.Range(0.5f, 1.9f);
        SFXPlayer.PlayOneShot(DigDirtSoundEffect, 0.5f);
        

        if (!TileIsFlagged)
        {
            if (!MineManagerScript.MinesHaveBeenRandomized)
            {
                MineManagerScript.RandomizeTheMineField(this);
            }

            TileIsActivated = true;
            TileCover.SetActive(false);

            if (TileIsActivated && TileIsBomb)
            {
                Mine.SetActive(true);
                ActionsListener.OnDiggingUpMine();
                return;
            }

            if (HowManyMinesInArea == 0)
            {
                int rng = Random.Range(0, 1001);

                if (rng >= 950)
                {
                    Coin.SetActive(true);
                }


                Invoke("Open_NSEW_Tiles", 0.2f);
                Invoke("Open_NEWSEW_Tiles", 0.45f);

            }
        }
    }

    void Open_NSEW_Tiles()
    {
        foreach (TileScript tile in SurroundingTiles)
        {
           
            if (tile.TileIsActivated || tile.TileIsFlagged)
            {
                continue;
            }

            if (Mathf.Approximately(tile.transform.position.x, transform.position.x) ||
                Mathf.Approximately(tile.transform.position.z, transform.position.z))
            {
                tile.ActivateTheTile();
            }
        }
    }
    void Open_NEWSEW_Tiles()
    {
        foreach (TileScript tile in SurroundingTiles)
        {
            if (tile.TileIsActivated || tile.TileIsFlagged)
            {
                continue;
            }

            if (!Mathf.Approximately(tile.transform.position.x, transform.position.x) &&
                !Mathf.Approximately(tile.transform.position.z, transform.position.z))
            {
                tile.ActivateTheTile();
            }
        }
    }

    public void PlaceTheFlag()
    {
        SFXPlayer.pitch = Random.Range(0.5f, 1.9f);
        SFXPlayer.PlayOneShot(DigDirtSoundEffect, 0.5f);

        if (MineManagerScript.Flags > 0)
        {
            TileIsFlagged = true;
            Flag.SetActive(true);
            ActionsListener.OnFlagPlace();

            if (TileIsBomb && TileIsFlagged)
            {
                TileIsComplete = true;
                ActionsListener.OnFlagPlaceOnMine();
            }
        }
    }
    public void RemoveTheFlag()
    {
        SFXPlayer.pitch = Random.Range(0.5f, 1.9f);
        SFXPlayer.PlayOneShot(DigDirtSoundEffect, 0.5f);

        TileIsFlagged = false;
        Flag.SetActive(false);
        ActionsListener.OnFlagRemove();

        if (TileIsComplete)
        {
            TileIsComplete = false;
            ActionsListener.OnFlagRemoveOffMine();
        }

    }


    public void OpenAllMinesAfterWin()
    {
        if (!TileIsBomb)
        {
            TileIsActivated = true;
            TileCover.SetActive(false);
            
        }
    }



}
