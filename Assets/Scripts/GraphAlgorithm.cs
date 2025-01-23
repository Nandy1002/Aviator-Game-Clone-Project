using System.Collections.Generic;
using UnityEngine;
public class GraphAlgorithm : MonoBehaviour
{
    [SerializeField]private Transform startPoint;
    [SerializeField]private Transform tangentPoint;
    [SerializeField]private Transform endPoint;
    [SerializeField]private LineRenderer linerenderer;
    [SerializeField]private float vertexCount = 12;
    [SerializeField]private float Point2Ypositio = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tangentPoint.transform.position = new Vector3((startPoint.transform.position.x + endPoint.transform.position.x), Point2Ypositio, (startPoint.transform.position.z + endPoint.transform.position.z) / 2);
        var pointList = new List<Vector3>();

        for(float ratio = 0;ratio<=1;ratio+= 1/vertexCount)
        {
            var tangent1 = Vector3.Lerp(startPoint.position, tangentPoint.position, ratio);
            var tangent2 = Vector3.Lerp(tangentPoint.position, endPoint.position, ratio);
            var curve = Vector3.Lerp(tangent1, tangent2, ratio);

            pointList.Add(curve);
        }

        linerenderer.positionCount = pointList.Count;
        linerenderer.SetPositions(pointList.ToArray());
    }
}
