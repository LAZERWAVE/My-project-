using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInTheDark : MonoBehaviour
{
    public LineRenderer lr;
    private int laserDistance = 100;
    private int reflection = 5;
    private int countLaser = 1;
    private Vector3 pos = new Vector3();
    private Vector3 directLaser = new Vector3();
    private bool active = true;
    public LayerMask layer;
    public int angle = 0;

    private float duration = 10f;
    private Vector3[] linePoints;
    
    // Start is called before the first frame update
    void Start()
    {
        active = true;
        pos = transform.position;
        countLaser = 1;

        directLaser = transform.forward;
        directLaser = new Vector3(directLaser.x +  Mathf.Sin(angle), directLaser.y +  Mathf.Cos(angle), directLaser.z);
        lr.positionCount = countLaser;
        lr.SetPosition(0,pos);

        while(active){
            RaycastHit2D hit = Physics2D.Raycast(pos, directLaser, laserDistance, layer);
            if(hit){
                countLaser++;
                lr.positionCount = countLaser;
                directLaser = Vector3.Reflect(directLaser, hit.normal);
                
                pos = (Vector2) directLaser.normalized +  hit.point;

                lr.SetPosition(countLaser - 1, hit.point);
            }else{
                countLaser++;
                lr.positionCount = countLaser;
                lr.SetPosition(countLaser - 1, pos + (directLaser.normalized * laserDistance));

                active = false;
            }

            if(countLaser > reflection) active = false;
        }

        linePoints = new Vector3[countLaser];
        for(int i=0; i<countLaser; i++){
            linePoints[i] = lr.GetPosition(i);
        }

        StartCoroutine(animate());

        // Destroy(this);
    }

    // Update is called once per frame
    private IEnumerator animate(){
        
        float segmentDuration = duration / countLaser;

        for(int i=0; i<countLaser-1; i++){
            float startTime = Time.time;

            Vector3 startPos = linePoints[i];
            Vector3 endPos = linePoints[i+1];

            Vector3 pos = startPos;
            while(pos != endPos){
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPos, endPos, t);
                
                for(int j=i+1; j<countLaser; j++){
                    lr.SetPosition(j,pos);
                }

                yield return null;
            }
        }   

        StartCoroutine(Done());
    }

    private IEnumerator Done()
    {
        yield return new WaitForSeconds(1f);
        Destroy(transform.gameObject);
    }  
}
