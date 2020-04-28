using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;

    public GameObject Cube;

    private void Awake()
    {
        Instance = this;
    }
}
