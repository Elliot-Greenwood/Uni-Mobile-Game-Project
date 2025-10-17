using UnityEngine;

public class MineFieldManagerScript : MonoBehaviour
{
    public TileScript[] MineFieldTiles;
    public int TotalMinesInMap = 0;
    public int MinesFlaggedCorrectly = 0;

    void Start()
    {
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
    }
}
