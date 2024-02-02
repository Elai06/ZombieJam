using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSpawner : MonoBehaviour
{

    [SerializeField] private GameObject deathFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FXSpawn()
    {
        Instantiate(deathFX, gameObject.transform.position, Quaternion.identity);
    }
}
