using System.Collections.Generic;
using System.Collections;
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

        Flag.SetActive(false);
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
                ActionsListener.OnDiggingUpMine();
                return;
            }

            if (HowManyMinesInArea == 0)
            {
                StartCoroutine(RevealTilesGradually());
            }
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


    IEnumerator RevealTilesGradually()
    {
        HashSet<TileScript> visited = new HashSet<TileScript>();
        Queue<TileScript> currentWave = new Queue<TileScript>();
        Queue<TileScript> nextWave = new Queue<TileScript>();

        visited.Add(this);
        currentWave.Enqueue(this);

        while (currentWave.Count > 0)
        {
            // Reveal all tiles in this wave
            while (currentWave.Count > 0)
            {
                TileScript current = currentWave.Dequeue();

                // reveal the tile (even if already open, it’s fine)
                current.TileIsActivated = true;
                current.TileCover.SetActive(false);

                // Only spread from empty tiles
                if (current.HowManyMinesInArea == 0)
                {
                    foreach (TileScript neighbor in current.SurroundingTiles)
                    {
                        if (neighbor.TileIsBomb || neighbor.TileIsFlagged || visited.Contains(neighbor))
                            continue;

                        // Only NSEW spread
                        if (IsCardinalNeighbor(current, neighbor))
                        {
                            visited.Add(neighbor);
                            nextWave.Enqueue(neighbor);
                        }
                    }
                }
            }

            // Wait before revealing the next ring
            yield return new WaitForSeconds(0.15f);

            // Move next layer to current
            (currentWave, nextWave) = (nextWave, new Queue<TileScript>());
        }
    }



    bool IsCardinalNeighbor(TileScript center, TileScript neighbor)
    {
        Vector3 delta = neighbor.transform.position - center.transform.position;
        delta.y = 0f;

        float dx = Mathf.Abs(delta.x);
        float dz = Mathf.Abs(delta.z);
        float tolerance = 0.6f; // good for irregular grid spacing

        bool northSouth = dx < tolerance && dz > tolerance;
        bool eastWest = dz < tolerance && dx > tolerance;

        return northSouth || eastWest;
    }




}
