
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class SerializableVector3{
    public float x;
    public float y;
    public float z;
 
    [JsonIgnore]
    public Vector3 UnityVector{
        get{
            return new Vector3(x, y, z);
        }
    }
 
    public SerializableVector3(Vector3 v){
        x = v.x;
        y = v.y;
        z = v.z;
    }
    public SerializableVector3(){
        x = 0;
        y = 0;
        z = 0;
    }
}