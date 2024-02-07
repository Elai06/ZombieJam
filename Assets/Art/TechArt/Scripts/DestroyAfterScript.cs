using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterScript : MonoBehaviour
{ 
    
    [SerializeField] private float lifeTime;

  

    private float _lifeTime;
    private void OnEnable()
    {
        _lifeTime = lifeTime;
    }

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
           
                Destroy(gameObject);
            
        }
    }
}

