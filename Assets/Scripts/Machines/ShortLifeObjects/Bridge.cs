using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : TileCollector,IProduce
{
    public int tilesNeeded { get; set; }
    public int halfBridgeCount;
    public int productMaxCount { get; set; }

    public override void Collect()
    {
       
    }
    public override void Remove()
    {
        base.Remove();
    }
    private void OnTriggerStay(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {

        }
    }
    bool IsPlayer(GameObject ob)
    {
        if (ob.CompareTag("Player"))
            return true;
        return false;
    }

    public void Produce(Action action)
    {
        action.Invoke();
    }
}
