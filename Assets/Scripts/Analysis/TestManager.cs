using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    public static TestManager instance;

    public bool stop;
    public Vector3 startPos;
    [SerializeField]
    PathFinder pathFinder;

    [SerializeField]
    Button astarButton, dijkstraButton,mapButton;

    [SerializeField]
    MazeGenerator mazeGen;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            stop = false;
            astarButton.onClick.AddListener(Swap);
            dijkstraButton.onClick.AddListener(Swap);
            mapButton.onClick.AddListener(NewMap);
        }
        else
            Destroy(this);
    }

    public void Restart()
    {
        pathFinder.algorithms = !pathFinder.algorithms;
        pathFinder.objStartCube.transform.position = startPos;
        pathFinder.pathTo = 1;
        pathFinder.elapsedTime = 0;
        Timer.instance.Restart();
        GridManager.instance.ComputeGrid();
        pathFinder.FindPath();
        stop = false;
    }

    public void ShowButton()
    {
        if (pathFinder.algorithms)
        {
            dijkstraButton.gameObject.SetActive(true);
        }
        else
        {
            astarButton.gameObject.SetActive(true);
        }

        mapButton.gameObject.SetActive(true);
    }

    void Swap()
    {
        HideButtons();

        Restart();
    }

    void HideButtons()
    {
        astarButton.gameObject.SetActive(false);
        dijkstraButton.gameObject.SetActive(false);
        mapButton.gameObject.SetActive(false);
    }

    void NewMap()
    {
        SceneManager.LoadScene(0);
    }
}
