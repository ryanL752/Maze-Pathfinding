using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    GameObject westWall;

    [SerializeField]
    GameObject eastWall;

    [SerializeField]
    GameObject northWall;

    [SerializeField]
    GameObject southWall;

    [SerializeField]
    GameObject unvisitedBlock;

    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
        unvisitedBlock.SetActive(false);
    }

    public void ClearWestWall()
    {
        westWall.SetActive(false);
    }

    public void ClearEastWall()
    {
        eastWall.SetActive(false);
    }

    public void ClearNorthWall()
    {
        northWall.SetActive(false);
    }

    public void ClearSouthWall()
    {
        southWall.SetActive(false);
    }
}