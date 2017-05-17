using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class PerlinTest : MonoBehaviour
{
    public Color test;
    //public Material M;

    Vector3 vectorSpace;
    // Use this for initialization
    void Start()
    {
        Renderer reder = GetComponent<Renderer>();

        // M = reder.material;

        vectorSpace = new Vector3(Random.Range(0, 10f), Random.Range(0, 10f), Random.Range(0, 10f));
    }

//Updateiscalledonceperframe

    void Update()
    {
        //利用PerlinNoise实现线性的过度作用

        float sx = Mathf.PerlinNoise(Time.time, vectorSpace.x);

        float sY = Mathf.PerlinNoise(Time.time, vectorSpace.y);

        float sZ = Mathf.PerlinNoise(Time.time, vectorSpace.z);

        test = new Color(sx, sY, sZ);
    }
}