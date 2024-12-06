using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject startPrefab, endPrefab;

    [SerializeField]
    MazeCell mazeCellPrefab;

    [SerializeField]
    int mazeWidth,mazeHeight;

    MazeCell[,] mazeGrid;

    [SerializeField]
    int offset = 1;

    [SerializeField]
    GameObject mapHolder;

    void Awake()
    {
        NewMap();
    }

    Vector3 OpenCell(Vector3 foundNode)
    {
        bool open = false;
        do
        {
            int row = (2*Random.Range(0, mazeWidth));
            int column = (2*Random.Range(0, mazeHeight));
            Vector3 newNode = new Vector3(row, 0, column);
            if (foundNode != newNode)
            {
                if (mazeGrid[row,column])
                    return mazeGrid[row, column].unvisitedBlock.transform.position;
            }
        }
        while (!open);

        return new Vector3(-1,-1,-1);
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

    public void NewMap()
    {
        mazeGrid = new MazeCell[mazeWidth * offset, mazeHeight * offset];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeHeight; z++)
            {
                mazeGrid[x * offset, z * offset] = Instantiate(mazeCellPrefab, new Vector3(x * offset, 0, z * offset), Quaternion.identity);
                mazeGrid[x * offset, z * offset].gameObject.transform.SetParent(mapHolder.transform);
            }
        }

        GenerateMaze(null, mazeGrid[0, 0]);
        GridManager.instance.ComputeGrid();

        Vector3 startPos = OpenCell(new Vector3(-1, -1, -1));
        Vector3 endPos = OpenCell(startPos);

        if (startPos.y != -1 && endPos.y != -1)
        {
            GameObject startObj = Instantiate(startPrefab);
            GameObject endObj = Instantiate(endPrefab);

            startObj.transform.position = startPos;
            endObj.transform.position = endPos;

            TestManager.instance.startPos = startPos;
        }
    }

    public void DeleteMap()
    {
        for(int i = 0; i < mapHolder.transform.childCount; i++)
        {
            Destroy(mapHolder.transform.GetChild(i).gameObject);
        }
    }
}