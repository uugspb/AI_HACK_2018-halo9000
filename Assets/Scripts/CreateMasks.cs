using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateMasks : MonoBehaviour {

	void Start () {

        Tilemap map = GetComponent<Tilemap>();
        BoundsInt bounds = map.cellBounds;

        Vector3Int min = bounds.min;
        Vector3Int max = bounds.max;

        for (int x = min.x; x < max.x; x++)
        {
            for (int y = min.y; y < max.y; y++)
            {
                for (int z = min.z; z < max.z; z++)
                {
                    Vector3Int cur = new Vector3Int(x, y, z);
                    Tile tile = map.GetTile<Tile>(cur);
                    if (tile != null)
                    {
                        GameObject go = new GameObject("TileMask");
                        go.transform.SetParent(transform);
                        go.transform.position = map.GetCellCenterWorld(cur);
                        SpriteMask spriteMask = go.AddComponent<SpriteMask>();
                        spriteMask.sprite = tile.sprite;
                    }
                }
            }
        }
    }
}
