using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private LayerMask layer;
    private GameObject player;
    private LightBehaviour lb;
    void Start()
    {
        lb = GetComponent<LightBehaviour>();
        layer = lb.layer;
        player= lb.player;
        for(int i=0; i<360; i+=10){
            GameObject go = new GameObject();
            go.transform.position = player.transform.position;
            go.transform.parent = player.transform;
            LightInTheDark litd =  go.AddComponent(typeof(LightInTheDark)) as LightInTheDark;
            LineRenderer lr = go.AddComponent(typeof(LineRenderer)) as LineRenderer;
            lr.material = new Material (Shader.Find ("Sprites/Default"));
            lr.material.color = Color.white; 
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.numCapVertices = 10;
            lr.numCornerVertices = 10;
            litd.lr = lr;
            litd.angle = i;
            litd.layer = layer;
        }
    }
}
