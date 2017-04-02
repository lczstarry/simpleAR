using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
//using OpenCvSharp.Blob;
using OpenCvSharp.UserInterface;
//using OpenCvSharp.Utilities;
using System.Runtime.InteropServices;

public class camera : MonoBehaviour
{
    
    GameObject obj1 = null;
    Matrix4x4 Perspective = Matrix4x4.identity;
    string adr = "";
    float[,] rt = new float[4, 4];
    float[] Angle = new float[3];
    bool istrue = plane.istrue;
    Vector3 v1 = Vector3.zero;
    Vector3 v2 = Vector3.zero;
    public int number = 0;
    Vector3 vector3 = new Vector3(0.3f, 0.3f, 0.3f);
    // Use this for initialization
   
    void Start()
    {
            adr = plane.GetRTPath();
            Perspective = Matrix4x4.Perspective(65.0f, 1.5f, 0.01f, 100.0f);
            obj1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj1.transform.localScale = vector3;
        
    }

    // Update is called once per frame
    void Update()
    {
       

            rt = ReadRT(adr);
            MatrixAngle(rt, Angle);
            //Matrix4x4 NewWorldToCameraMatrix = transform.worldToLocalMatrix;

            Matrix4x4 m = Matrix4x4.TRS(new Vector3(-rt[3, 0], -rt[3, 1], rt[3, 2]), Quaternion.Euler(Angle[0], 180 + Angle[1], Angle[2]), Vector3.one);

            //Camera.main.worldToCameraMatrix = m * NewWorldToCameraMatrix;
            Perspective.SetTRS(new Vector3(-rt[3, 0], -rt[3, 1], rt[3, 2]), Quaternion.Euler(Angle[0], 180 + Angle[1], Angle[2]), Vector3.one);
            v2 = Perspective.MultiplyPoint3x4(v1);
            Debug.Log("v1的值" + v1);
            Debug.Log("v2的值" + v2);
            Debug.Log("rotation" + Quaternion.Euler(Angle[0], 180 + Angle[1], Angle[2]));

            obj1.transform.position = v2;
            obj1.transform.rotation = Quaternion.Euler(Angle[0], 180 + Angle[1], Angle[2]);

            obj1.name = "cube";
       


    }

    void MatrixAngle(float[,] rt, float[] Angle)
    {
        float c;
        float tx, ty;

        Angle[1] = (float)Math.Asin(rt[0, 2]);
        c = (float)Math.Cos(Angle[1]);
        if (Math.Abs(c) > 0.005)
        {
            tx = rt[2, 2] / c;
            ty = -rt[1, 2] / c;
            Angle[0] = (float)Math.Atan2(ty, tx);
            tx = rt[0, 0] / c;
            ty = -rt[0, 1] / c;
            Angle[2] = (float)Math.Atan2(ty, tx);
        }
        else
        {
            Angle[0] = 0;
            tx = rt[1, 1];
            ty = rt[1, 0];
            Angle[2] = (float)Math.Atan2(ty, tx);
        }
    }

    float[,] ReadRT(string adr)
    {
        float[,] rt = new float[4, 4];

        FileStream fs = new FileStream(adr, FileMode.Open, FileAccess.Read);

        StreamReader sr = new StreamReader(fs);

        string line = "";
        int j = 0;
        while ((line = sr.ReadLine()) != null)
        {
            string[] strArr = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //double[] dblArr = new double[strArr.Length];
            for (int i = 0; i < 3; i++)
            {

                rt[j, i] = float.Parse(strArr[i]);

            }
            j++;

        }


        return rt;
    }

}
