using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {
    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    public List<Node> pathArray;

    public GameObject objStartCube, objEndCube;

    public float elapsedTime = 0.0f;
    public float intervalTime = 1.0f; //Interval time between path finding

    public int pathTo = 1,speed = 5;

    public bool algorithms = false;

    void Start() {
        objStartCube = GameObject.FindGameObjectWithTag("Start");
        objEndCube = GameObject.FindGameObjectWithTag("End");

        pathArray = new List<Node>();
        FindPath();
    }

    void Update() {
        if (TestManager.instance.stop)
            return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= intervalTime) {
            elapsedTime = 0.0f;
            FindPath();
        }

        if(pathArray != null)
        {
            if (pathTo < pathArray.Count)
            {
                Node node = pathArray[pathTo];
                if (Vector3.Distance(objStartCube.transform.position, node.position) < 0.5f)
                {
                    pathTo++;
                }
                else
                {
                    objStartCube.transform.position = Vector3.MoveTowards(objStartCube.transform.position, new Vector3(node.position.x, 0.5f, node.position.z), speed * Time.deltaTime);
                }
            }
            else
            {
                Timer.instance.SaveValues();
            }
        }
    }

    public void FindPath()
    {
        objStartCube = GameObject.FindGameObjectWithTag("Start");
        objEndCube = GameObject.FindGameObjectWithTag("End");

        startPos = objStartCube.transform;
        endPos = objEndCube.transform;

        var (startColumn, startRow) = GridManager.instance.GetGridCoordinates(startPos.position);
        var (goalColumn, goalRow) = GridManager.instance.GetGridCoordinates(endPos.position);

        if (algorithms) {
            startNode = new Node(GridManager.instance.GetGridCellCenter(startColumn, startRow),0f);
            goalNode = new Node(GridManager.instance.GetGridCellCenter(goalColumn, goalRow),0f);
            pathArray = new AStar().FindPath(startNode, goalNode);
        }
        else
        {
            startNode = new Node(GridManager.instance.GetGridCellCenter(startColumn, startRow),Mathf.Infinity);
            goalNode = new Node(GridManager.instance.GetGridCellCenter(goalColumn, goalRow),Mathf.Infinity);
            pathArray = new Dijkstra().FindPath(startNode, goalNode);
        }
    }

    void OnDrawGizmos() {
        if (pathArray == null)
            return;

        if (pathArray.Count > 0) {
            int index = 1;
            foreach (Node node in pathArray) {
                if (index < pathArray.Count) {
                    Node nextNode = pathArray[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.green);
                    index++;
                }
            };
        }
    }
}