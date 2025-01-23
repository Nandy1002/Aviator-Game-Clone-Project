using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GraphAlgorithm : MonoBehaviour
{
    [SerializeField]private Transform startPoint;
    [SerializeField]private Transform tangentPoint;
    [SerializeField]private Transform endPoint;
    [SerializeField]private LineRenderer linerenderer;
    private float vertexCount;
    private float curveture;
    // Start is called before the first frame update
    void Start()
    {
        endPoint.transform.localPosition = new Vector2(0, 0);
        vertexCount = 1;
        curveture = -0.001f;
    }

    // Update is called once per frame
    void Update()
    {

        if(BetAlgorithm.Instance.IsBetting()){
            var localPosition = endPoint.transform.localPosition;

            if(endPoint.transform.localPosition.x < 10){
                localPosition.x += Time.deltaTime * 2;
            }
            if(endPoint.transform.localPosition.y < 5){
                localPosition.y += BetAlgorithm.Instance.GetBetValue() * Time.deltaTime; // Ensure the value is scaled by Time.deltaTime
            }
            if(curveture > -1.5){
                curveture -= BetAlgorithm.Instance.GetBetValue() * Time.deltaTime*0.5f; // Ensure the value is scaled by Time.deltaTime
            }
            if(vertexCount < 25 && endPoint.transform.localPosition.y > 0.2){
                vertexCount += 1f; // Ensure the value is scaled by Time.deltaTime
            }
            endPoint.transform.localPosition = localPosition;
        }
        



        // draw graph algorithm
        tangentPoint.transform.position = new Vector2((startPoint.transform.position.x + endPoint.transform.position.x) / 2, curveture);
        var pointList = new List<Vector2>();
        
        for (float ratio = 0; ratio <= 1; ratio += 1f / vertexCount)
        {
            var tangent1 = Vector2.Lerp(startPoint.position, tangentPoint.position, ratio);
            var tangent2 = Vector2.Lerp(tangentPoint.position, endPoint.position, ratio);
            var curve = Vector2.Lerp(tangent1, tangent2, ratio);
        
            pointList.Add(curve);
        }
        
        linerenderer.positionCount = pointList.Count;
        linerenderer.SetPositions(pointList.Select(p => new Vector3(p.x, p.y, 0)).ToArray());
    }

    public void ResetEndPoint(){
        if(!BetAlgorithm.Instance.IsBetting()){
            endPoint.transform.localPosition = new Vector2(0, 0);
        }
    }
}
