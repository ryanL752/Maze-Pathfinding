using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    [SerializeField]
    PathFinder pathFinder;
    float time;

    [SerializeField]
    TMP_Text aStarTimer;
    [SerializeField]
    TMP_Text dijkstraTimer;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            time = 0f;
        }
        else
            Destroy(this);
    }

    void Update()
    {
        if (!TestManager.instance.stop)
        {
            time += Time.deltaTime;
            if (pathFinder.algorithms)
            {
                aStarTimer.text = time.ToString("F2");
                dijkstraTimer.text = PlayerPrefs.GetString("dijkstraTime");
            }
            else
            {
                aStarTimer.text = PlayerPrefs.GetString("aStarTime");
                dijkstraTimer.text = time.ToString("F2");
            }
        }
    }

    public void SaveValues()
    {
        TestManager.instance.stop = true;

        if (pathFinder.algorithms)
            PlayerPrefs.SetString("aStarTime", time.ToString("F2"));
        else
            PlayerPrefs.SetString("dijkstraTime", time.ToString("F2"));

        TestManager.instance.ShowButton();
    }

    public void Restart()
    {
        time = 0f;
    }
}
