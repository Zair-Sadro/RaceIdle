using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionIngore : MonoBehaviour
{
    [SerializeField] private List<Collider> _cars;
    void Start()
    {
        //Test
        for (int i = 0; i < _cars.Count-1; i++)
        {
            for (int j = i+1; j < _cars.Count; j++)
            {
                Physics.IgnoreCollision(_cars[i], _cars[j], true);
            }
            
        }
    }
    public void AddIgnore(Collider c1,Collider c2)
    {
        Physics.IgnoreCollision(c1, c2, true);

    }

}
