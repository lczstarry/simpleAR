using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;


    public class ARDLL
    {
        private const string LIBRARY_NAME = "ARDLLan";
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Imagerows();

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool update_camera_frame(IntPtr colors32);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool get_camera_size(out int width, out int height);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool open_camera();
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool open_camera1();

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool close_camera();

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool getPatternLocation([MarshalAs(UnmanagedType.LPStr)] string filepath, [MarshalAs(UnmanagedType.LPStr)] string rtpath);

    }

