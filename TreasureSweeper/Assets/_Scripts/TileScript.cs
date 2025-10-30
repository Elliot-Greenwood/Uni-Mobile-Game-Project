using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    MineFieldManagerScript MineManagerScript;

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

    public Renderer TileCoverMaterialRendered;
    public Material TileCoverMatIdle;
    public Material TileCoverMatOnstep;

    Renderer MaterialRendered;

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

        MaterialRendered = GetComponent<Renderer>();

        Invoke("InitTileMaterialCheck", 0.1f);
    }


    void InitTileMaterialCheck()
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
        if (!TileIsFlagged)
        {
            TileIsActivated = true;
            TileCover.SetActive(false);

            if (TileIsActivated && TileIsBomb)
            {
                ActionsListener.OnDiggingUpMine();
            }

            if (HowManyMinesInArea == 0)
            {
                foreach (TileScript tile in SurroundingTiles)
                {
                    if (!tile.TileIsBomb && !tile.TileIsActivated)
                    {
                        tile.TileIsActivated = true;
                        tile.ActivateTheTile();
                    }
                }
            }

           //if (HowManyMinesInArea != 0)
           //{
           //    foreach (TileScript tile in SurroundingTiles)
           //    {
           //        if (!tile.TileIsBomb && !tile.TileIsActivated && tile.HowManyMinesInArea == 0)
           //        {
           //            tile.TileIsActivated = true;
           //            tile.ActivateTheTile();
           //        }
           //    }
           //}

        }
    }

    public void PlaceTheFlag()
    {
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
        else
        {
            //display No more Flags
        }
       
    }
    public void RemoveTheFlag()
    {
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
