using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorDuplicator : MonoBehaviour
{

    [SerializeField] private Animator _animator_1;
    [SerializeField] private Animator _animator_2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move()
    {
        if (_animator_1 != null)
        {
            _animator_1.SetTrigger("Move"); 
        }
        
        if (_animator_2 != null)
        {
            _animator_2.SetTrigger("Move"); 
        }
    }
    
    public void Attack()
    {
        if (_animator_1 != null)
        {
            _animator_1.SetTrigger("Attack"); 
        }
        
        if (_animator_2 != null)
        {
            _animator_2.SetTrigger("Attack"); 
        }
    }
    

}
