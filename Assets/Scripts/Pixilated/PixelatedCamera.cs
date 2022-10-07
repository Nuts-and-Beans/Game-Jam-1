using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteAlways]
public class PixelatedCamera : MonoBehaviour {
    [Header("References")]
    [SerializeField] private RawImage display;
    
    [Header("Pixalize Settings")]
    [SerializeField] private PixelScreenMode mode;
    [SerializeField] private ScreenSize targetScreenSize = new ScreenSize { width = 256, height = 144 };
    [SerializeField] private uint screenScaleFactor = 1;

    private enum PixelScreenMode {
        Resize = 0,
        Scale,
    }

    [Serializable]
    private struct ScreenSize {
        public int width;
        public int height;
    }

    private Camera cam;
    private RenderTexture renderTexture;
    private int screenWidth;
    private int screenHeight;

    private void Start() {
        Init();
    }
    
    private void Init() {
        // ensure we have got the camera for rendering
        if (!cam) cam = GetComponent<Camera>();
        screenWidth  = Screen.width;
        screenHeight = Screen.height;
        
        
        
        // ensure we don't have any weird stuff
        if (screenScaleFactor < 1)       screenScaleFactor = 1;
        if (targetScreenSize.width < 1)  targetScreenSize.width = 1;
        if (targetScreenSize.height < 1) targetScreenSize.height = 1;

        // calculate the render texture size
        int width, height;
        if (mode == PixelScreenMode.Resize) {
            width  = (int)targetScreenSize.width;
            height = (int)targetScreenSize.height;
        } else {
            width  = screenWidth  / (int)screenScaleFactor;
            height = screenHeight / (int)screenScaleFactor;
        }


        // initialize the render texture
        renderTexture = new RenderTexture(width, height, 24) {
            filterMode = FilterMode.Point,
            antiAliasing = 1,
        };

        // set the render texture as the camera's output
        cam.targetTexture = renderTexture;

        // attaching texture to the display UI RawImage
        display.texture = renderTexture;
    }
    
#if UNITY_EDITOR
    private void OnValidate() {
        Init();
    }
#endif
}
