﻿using UnityEngine;
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


public class plane : MonoBehaviour
{

    public static bool istrue = false;
    public int i;
    public bool b;
    private Color32[] cameraVideoData = null;
    public MeshRenderer meshRenderer;
    private Texture2D cameraVideoTexture = null;//2d纹理
    private Material cameraVideoMaterial = null;

    private int screenWidth = 0;
    private int screenHeight = 0;

    public string  patternname = "Pattern";
    public static string rtname = "RT";

    private Mesh newVideoMesh(bool flipX, bool flipY)
    {
        Mesh m = new Mesh();
        m.Clear();

        float r = 0.5f;

        m.vertices = new Vector3[] {
                new Vector3(-r, -r, 0.5f),
                new Vector3( r, -r, 0.5f),
                new Vector3( r,  r, 0.5f),
                new Vector3(-r,  r, 0.5f),
            };

        m.normals = new Vector3[] {
                new Vector3(0.0f, 0.0f, 1.0f),
                new Vector3(0.0f, 0.0f, 1.0f),
                new Vector3(0.0f, 0.0f, 1.0f),
                new Vector3(0.0f, 0.0f, 1.0f),
            };

        float u1 = flipX ? 1.0f : 0.0f;
        float u2 = flipX ? 0.0f : 1.0f;

        float v1 = flipY ? 1.0f : 0.0f;
        float v2 = flipY ? 0.0f : 1.0f;

        m.uv = new Vector2[] {
                new Vector2(u1, v1),
                new Vector2(u2, v1),
                new Vector2(u2, v2),
                new Vector2(u1, v2)
            };

        m.triangles = new int[] {
                2, 1, 0,
                3, 2, 0
            };

        m.Optimize();
        return m;
    }
    // Use this for initialization
    void Start()
    {
       

        ARDLL.open_camera();
        //i = ARDLL.Imagerows();
        meshRenderer = this.GetComponent<MeshRenderer>();
        //cameraVideoData = new Color32(Color.blue.r, Color.blue.g, Color.blue.b, Color.blue.a);
        //b = pulgins.open_camera();
    }

    // Update is called once per frame
    void Update()
    {

        if (cameraVideoData == null)
        {
            int width, height;
            ARDLL.get_camera_size(out width, out height);
            // Debug.Log("width:" + width.ToString());
            //Debug.Log("height：" + height.ToString());
            cameraVideoData = new Color32[width * height];
            cameraVideoTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);

            cameraVideoTexture.hideFlags = HideFlags.HideAndDontSave;
            cameraVideoTexture.filterMode = FilterMode.Bilinear;
            cameraVideoTexture.wrapMode = TextureWrapMode.Clamp;
            cameraVideoTexture.anisoLevel = 0;

            Shader shaderSource = Shader.Find("VideoPlaneNoLight");
            cameraVideoMaterial = new Material(shaderSource);
            cameraVideoMaterial.hideFlags = HideFlags.HideAndDontSave;
            cameraVideoMaterial.mainTexture = cameraVideoTexture;

            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
            meshFilter.mesh = newVideoMesh(false, true);

            meshRenderer.receiveShadows = false;
            meshRenderer.material = cameraVideoMaterial;
        }

        GCHandle handle = GCHandle.Alloc(cameraVideoData, GCHandleType.Pinned);
        IntPtr address = handle.AddrOfPinnedObject();
        ARDLL.update_camera_frame(address);
        string patternpath = GetImagePath();
        string rtpath = GetRTPath();
        istrue = ARDLL.getPatternLocation(patternpath,rtpath);
        Debug.Log(istrue.ToString());
        handle.Free();
        meshRenderer.material.mainTexture = cameraVideoTexture;
        //Debug.Log(address.ToString());
        cameraVideoTexture.SetPixels32(cameraVideoData);
        cameraVideoTexture.Apply(false);
    }


    void OnDestory()
    {
        ARDLL.close_camera();
    }
    public string GetImagePath()
    {
        return Application.streamingAssetsPath  + "/"+patternname + ".jpg";
    }
    public static string GetRTPath()
    {
        return Application.streamingAssetsPath +"/"+ rtname + ".txt";
    }




}

