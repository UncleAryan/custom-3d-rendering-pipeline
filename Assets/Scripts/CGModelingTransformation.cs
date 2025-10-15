// Demo settings & input handling (keys 1/2 switch projection, 'A' toggles auto-spin)
// Attach this along with CG_Draw to the Camera.
using System.Collections.Generic;
using MyCustomMath;
using UnityEngine;

namespace MyCustomMath {
    public class CGModelingTransformation : MonoBehaviour {
        [Header("Projection")]
        public bool usePerspective = true;
        [Range(10f, 120f)] public float fovY = 60f;
        public float orthoHeight = 4f;        // total height of ortho box
        public float nearPlane = 0.1f;
        public float farPlane = 100f;

        [Header("Viewport (normalized)")]
        [Range(0, 1)] public float vpX = 0f;
        [Range(0, 1)] public float vpY = 0f;
        [Range(0, 1)] public float vpW = 1f;
        [Range(0, 1)] public float vpH = 1f;

        [Header("Modeling Transform (T * Rz * Ry * Rx * S)")]
        public Vector3 translate = new Vector3(0, 2, 6);
        public Vector3 rotateDeg = new Vector3(0, 0, 0);
        public Vector3 scale = new Vector3(1, 1, 1);
        public bool autoSpin = true;
        public Vector3 spinSpeedDegPerSec = new Vector3(0f, 45f, 0f);

        [Header("Primitives")]
        public bool showCube = true;
        public float cubeSize = 2f;

        // TODO B1: add public fields
        [Header("Object-Space, World-Fixed Objects")]
        public bool showAxes = true;
        public float axisLength = 5f;
        public bool showGridXZ = true;
        public float gridExtent = 8f;
        public float gridStep = 1f;


        /*[Header("Matrix GUI")]
        [SerializeField] bool showMatrixGui = true;
        [SerializeField] Rect matrixWin = new Rect(12, 12, 620, 300);  // top-left by default*/

        void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) usePerspective = false;
            if (Input.GetKeyDown(KeyCode.Alpha2)) usePerspective = true;
            if (Input.GetKeyDown(KeyCode.A)) autoSpin = !autoSpin;

            if (autoSpin) {
                rotateDeg.x += spinSpeedDegPerSec.x * Time.deltaTime;
                rotateDeg.y += spinSpeedDegPerSec.y * Time.deltaTime;
                rotateDeg.z += spinSpeedDegPerSec.z * Time.deltaTime;
            }
        }

        public Mat4 BuildModelMatrix() {
            var t = Mat4.Translate(translate.x, translate.y, translate.z);
            var rx = Mat4.RotationX(rotateDeg.x);
            var ry = Mat4.RotationY(rotateDeg.y);
            var rz = Mat4.RotationZ(rotateDeg.z);
            var s = Mat4.Scale(scale.x, scale.y, scale.z);
            // Order: M = T * Rz * Ry * Rx * S
            var m = (t * (rz * (ry * (rx * s))));
            return m;
        }

        public Mat4 BuildProjectionMatrix(int pixelW, int pixelH) {
            float aspect = (pixelH != 0) ? (pixelW / (float)pixelH) : 1f;
            if (usePerspective) {
                return Mat4.PerspectiveProjection(fovY, aspect, nearPlane, farPlane);
            } else {
                float halfH = orthoHeight * 0.5f;
                float halfW = halfH * aspect;
                return Mat4.OrthographicProjection(-halfW, +halfW, -halfH, +halfH, nearPlane, farPlane);
            }
        }

        public List<Line3> CollectPrims() {
            var lines = new List<Line3>();

            if (showCube) lines.AddRange(CGWirePrims.Cube(cubeSize));
            return lines;
        }

        // TODO B2: add two methods
        public List<Line3> CollectGridAndAxes() {
            // Build a new List<Line3>();
            var lines = new List<Line3>();  

            // if (showAxes) add CG_WirePrims.Axis(axisLength)
            if(showAxes) {
                lines.AddRange(CGWirePrims.Axis(axisLength));
            }

            // if (showGridXZ) add CG_WirePrims.GridXZ(gridExtent, gridStep)
            if (showGridXZ) {
                lines.AddRange(CGWirePrims.GridXZ(gridExtent, gridStep));
            }

            // return the list
            return lines;
        }

        public List<Line3> CollectCubeOnly() {
            // Preserve the existing cube behavior:
            // if (showCube) add CG_WirePrims.Cube(cubeSize)
            var lines = new List<Line3>();
            if (showCube) {
                lines.AddRange(CGWirePrims.Cube(cubeSize));
            }

            // return the list
            return lines;
        }
    }
}