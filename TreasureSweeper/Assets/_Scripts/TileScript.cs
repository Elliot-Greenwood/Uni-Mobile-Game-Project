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

    private void Start()
    {
        MineManagerScript = GameObject.FindWithTag("MineFieldManager").GetComponent<MineFieldManagerScript>();

        MaterialRendered = GetComponent<Renderer>();

        Invoke("InitTileCheck", 0.1f);
    }


    void InitTileCheck()
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
                //game over.
                Debug.Log("GAME OVER!!!");
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

        }
    }

    public void PlaceTheFlag()
    {
        TileIsFlagged = true;
        Flag.SetActive(true);

        if (TileIsBomb && TileIsFlagged)
        {
            TileIsComplete = true;
            MineManagerScript.MinesFlaggedCorrectly++;
        }

        if (MineManagerScript.TotalMinesInMap == MineManagerScript.MinesFlaggedCorrectly)
        {
            //Win Game
            Debug.Log("GAME COMPLETED!!!");
        }
    }
    public void RemoveTheFlag()
    {
        TileIsFlagged = false;
        Flag.SetActive(false);

        if (TileIsComplete)
        {
            TileIsComplete = false;
            MineManagerScript.MinesFlaggedCorrectly--;
        }

    }

}
