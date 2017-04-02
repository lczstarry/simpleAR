using UnityEngine;
using System.Collections;
using OpenCvSharp;
//using OpenCvSharp.MachineLearning;

public class CaptureTest5 : MonoBehaviour
{
    private CvCapture capture;
   
    

    private int maxPointNum;
    //private CvMemStorage storage;
    //private CvSeq<CvPoint> maxSeq;
    // Use this for initialization
    void Start()
    {
        //storage = Cv.CreateMemStorage(0);
        //test = new PointTest();
        //storage = Cv.CreateMemStorage(0);
        capture = Cv.CreateCameraCapture(0);
        
      
    }

    // Update is called once per frame
    unsafe void Update()
    {

        IplImage frame = Cv.QueryFrame(capture);
        //Debug.Log(frame.NChannels);

        Cv.ShowImage("frame", frame);






    }
        
    

  
    void OnDestroy()
    {
        //Cv.ReleaseMemStorage(storage);
        //Cv.ReleaseMemStorage(storage);
        Cv.DestroyAllWindows();
        Cv.ReleaseCapture(capture);

    }
}
