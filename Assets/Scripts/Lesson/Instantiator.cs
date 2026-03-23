using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            InstantiatePrefab();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Time.timeScale = 2;
        }
        
        float dt = Time.deltaTime;
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        Time.timeScale = 1.0f;
    }

    private void InstantiatePrefab()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = -10;
        GameObject a = Instantiate(_prefab);
        a.transform.parent = null;
        a.transform.position = pos;
    }
}
