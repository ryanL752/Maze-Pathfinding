using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour {
    private static GridManager staticInstance = null;

    public static GridManager instance {
        get {
            if (staticInstance == null) {
                staticInstance = FindObjectOfType(typeof(GridManager)) as GridManager;
                if (staticInstance == null)
                    Debug.Log("Could not locate an GridManager object. \n You have to have exactly one GridManager in the scene.");
            }
            return staticInstance;
        }
    }

    void OnApplicationQuit() {
        staticInstance = null;
    }

    public int numberOfRows;
    public int numberOfColumns;
    public float gridCellSize;
    public float obstacleEpsilon = 0.2f;
    public bool showGrid = true;
    public bool showObstacleBlocks = true;

    [SerializeField]
    PathFinder pathFinder;

    public Node[,] nodes { get; set; }

    public Vector3 Origin {
        get { return transform.position; }
    }

    public float StepCost {
        get { return gridCellSize; }
    }

    void Awake() {
        ComputeGrid();
    }

    void ComputeGrid() {
        nodes = new Node[numberOfColumns, numberOfRows];

        for (int i = 0; i < numberOfColumns; i++) {
            for (int j = 0; j < numberOfRows; j++) {
                Vector3 cellPos = GetGridCellCenter(i,j);
                float cost = pathFinder.algorithms ? 0.0f : Mathf.Infinity; 
                Node node = new(cellPos, cost);


                var collisions = Physics.OverlapSphere(cellPos, gridCellSize / 2 - obstacleEpsilon, 1 << LayerMask.NameToLayer("Obstacles"));
                if (collisions.Length != 0) {
                    node.MarkAsObstacle();
                }
                nodes[i, j] = node;
            }
        }
    }

    public Vector3 GetGridCellCenter(int col, int row) {
        Vector3 cellPosition = GetGridCellPosition(col, row);
        cellPosition.x += gridCellSize / 2.0f;
        cellPosition.z += gridCellSize / 2.0f;

        return cellPosition;
    }

    public Vector3 GetGridCellPosition(int col, int row) {
        float xPosInGrid = col * gridCellSize;
        float zPosInGrid = row * gridCellSize;

        return Origin + new Vector3(xPosInGrid, 0.0f, zPosInGrid);
    }

    public (int,int) GetGridCoordinates(Vector3 pos) {
        if (!IsInBounds(pos)) {
            return (-1,-1);
        }

        int col = (int)Mathf.Floor((pos.x-Origin.x) / gridCellSize);
        int row = (int)Mathf.Floor((pos.z-Origin.z) / gridCellSize);

        return (col, row);
    }

    public bool IsInBounds(Vector3 pos) {
        float width = numberOfColumns * gridCellSize;
        float height = numberOfRows * gridCellSize;

        return (pos.x >= Origin.x && pos.x <= Origin.x + width && pos.x <= Origin.z + height && pos.z >= Origin.z);
    }

    public bool IsTraversable(int col, int row) {
        return col >= 0 && row >= 0 && col < numberOfColumns && row < numberOfRows && !nodes[col, row].isObstacle;
    }

    public List<Node> GetNeighbours(Node node) {
        List<Node> result = new();
        var (column, row) = GetGridCoordinates(node.position);

        if (IsTraversable(column - 1, row)) {
            result.Add(nodes[column - 1, row]);
        }
        if (IsTraversable(column + 1, row)) {
            result.Add(nodes[column + 1, row]);
        }
        if (IsTraversable(column, row - 1)) {
            result.Add(nodes[column, row - 1]);
        }
        if (IsTraversable(column, row + 1)) {
            result.Add(nodes[column, row + 1]);
        }
        return result;
    }

    void OnDrawGizmos() {
        if (showGrid) {
            DebugDrawGrid(Color.blue);
        }

        Gizmos.DrawSphere(Origin, 0.5f);

        if (nodes == null) return;

        if (showObstacleBlocks) {
            Vector3 cellSize = new Vector3(gridCellSize, 1.0f, gridCellSize);
            Gizmos.color = Color.red;
            for (int i = 0; i < numberOfColumns; i++) {
                for (int j = 0; j < numberOfRows; j++) {
                    if (nodes != null && nodes[i, j].isObstacle) {
                        Gizmos.DrawCube(GetGridCellCenter(i,j), cellSize);
                    }
                }
            }
        }
    }

    public void DebugDrawGrid(Color color) {
        float width = (numberOfColumns * gridCellSize);
        float height = (numberOfRows * gridCellSize);

        for (int i = 0; i < numberOfRows + 1; i++) {
            Vector3 startPos = Origin + i * gridCellSize * new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }

        for (int i = 0; i < numberOfColumns + 1; i++) {
            Vector3 startPos = Origin + i * gridCellSize * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f, 1.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
    }

}
