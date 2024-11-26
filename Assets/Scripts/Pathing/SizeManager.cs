using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeManager : MonoBehaviour
{
    public static SizeManager instance;
    public int height = 50, width = 50;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this);
    }

    void OnValidate()
    {
        transform.localScale = new Vector3(width, 1, height);
    }
}
