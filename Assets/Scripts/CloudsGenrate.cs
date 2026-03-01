using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsGenrate : MonoBehaviour
{
    public GameObject[] Clouds;
    public GameObject Cloudss;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            Instantiate(Clouds[Random.Range(0, Clouds.Length)], new Vector3(Random.Range(-10,20), Random.Range(2,12), 5), Quaternion.identity, Cloudss.transform);
        }
    }
}
