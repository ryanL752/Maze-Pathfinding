using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    MazeCell mazeCellPrefab;

    int mazeWidth,mazeHeight;

    MazeCell[,] mazeGrid;

    [SerializeField]
    int offset = 1;

    void Start()
    {
        mazeWidth = SizeManager.instance.width;
        mazeHeight = SizeManager.instance.height;
        mazeGrid = new MazeCell[ mazeWidth*offset,  mazeHeight*offset];

        for (int x = 0; x <  mazeWidth; x++)
        {
            for (int z = 0; z <  mazeHeight; z++)
            {
                mazeGrid[x* offset, z* offset] = Instantiate( mazeCellPrefab, new Vector3(x*offset, 0, z*offset), Quaternion.identity);
            }
        }

        GenerateMaze(null, mazeGrid[0, 0]);
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy( _ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + offset <  mazeWidth*offset)
        {
            var cellToEast =  mazeGrid[x + offset, z];

            if (cellToEast.IsVisited == false)
            {
                yield return cellToEast;
            }
        }

        if (x - offset >= 0)
        {
            var cellToWest = mazeGrid[x - offset, z];

            if (cellToWest.IsVisited == false)
            {
                yield return cellToWest;
            }
        }

        if (z + offset < mazeHeight* offset)
        {
            var cellToNorth = mazeGrid[x, z + offset];

            if (cellToNorth.IsVisited == false)
            {
                yield return cellToNorth;
            }
        }

        if (z - offset >= 0)
        {
            var cellToSouth = mazeGrid[x, z - offset];

            if (cellToSouth.IsVisited == false)
            {
                yield return cellToSouth;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearEastWall();
            currentCell.ClearWestWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearWestWall();
            currentCell.ClearEastWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearNorthWall();
            currentCell.ClearSouthWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearSouthWall();
            currentCell.ClearNorthWall();
            return;
        }
    }

}