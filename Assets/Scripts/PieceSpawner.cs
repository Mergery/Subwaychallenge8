using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public PieceType type;
    private Piece currentPiece;

    public void Spawn()
    {
        int amtObject = 0;
        switch (type)
        {
            case PieceType.jump:
                amtObject = LevelManager.instance.jumps.Count;
                break;

            case PieceType.longblock:
                amtObject = LevelManager.instance.longblocks.Count;
                break;

            case PieceType.slide:
                amtObject = LevelManager.instance.slides.Count;
                break;

            case PieceType.ramp:
                amtObject = LevelManager.instance.ramps.Count;
                break;
        }

        currentPiece = LevelManager.instance.GetPiece(type, Random.Range(0, amtObject));
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }

    public void DeSpawn()
    {
        currentPiece.gameObject.SetActive(false);
    }
}
