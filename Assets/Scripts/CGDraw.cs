// Draws wire primitives using our own pipeline: Model -> Projection -> NDC -> Viewport -> Pixel
// Renders via GL.LINES in screen space (GL.LoadPixelMatrix). Attach to the Camera.
using System;
using System.Collections.Generic;
using MyCustomMath;
using UnityEngine;

namespace MyCustomMath {
    [RequireComponent(typeof(Camera))]
    public class CGDraw : MonoBehaviour {
        private Material lineMat;
        private CGModelingTransformation demo;
        private Camera cam;

        void Awake() {
            cam = GetComponent<Camera>();
            demo = GetComponent<CGModelingTransformation>();
            EnsureMaterial();
        }

        void EnsureMaterial() {
            if (lineMat != null) return;
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMat = new Material(shader);
            lineMat.hideFlags = HideFlags.HideAndDontSave;
            // basic state
            lineMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMat.SetInt("_ZWrite", 0);

            // Add this line:
            lineMat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        }


        void OnRenderObject() {
            if (demo == null) return;
            EnsureMaterial();

            int W = Screen.width;
            int H = Screen.height;

            // var m = demo.BuildModelMatrix();
            // var p = demo.BuildProjectionMatrix(w, h);

            // TODO C1: inside OnRenderObject()
            // var P = demo.BuildProjectionMatrix(W, H);
            // Compute pixel viewport (vx,vy,vw,vh) as before.
            var P = demo.BuildProjectionMatrix(W, H);
            var M_grid = Mat4.Identity(); // world-fixed
            var M_cube = demo.BuildModelMatrix(); // adjustable

            // viewport in pixels
            float vx = demo.vpX * W;
            float vy = demo.vpY * H;
            float vw = demo.vpW * W; if (vw < 1f) vw = 1f;
            float vh = demo.vpH * H; if (vh < 1f) vh = 1f;

            // var prims = demo.CollectPrims();
            var gridAndAxes = demo.CollectGridAndAxes();
            var cubeOnly = demo.CollectCubeOnly();
            GL.Color(Color.white); // or any bright color, alpha=1

            lineMat.SetPass(0);
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, W, H, 0); // 2D pixel space, (0,0)=top-left

            // DrawLinesTransformed(prims, m, p, vx, vy, vw, vh);

            // PASS 1: grid + axes with M_grid
            DrawLinesTransformed(gridAndAxes, M_grid, P, vx, vy, vw, vh);
            // PASS 2: cube with M_cube
            DrawLinesTransformed(cubeOnly, M_cube, P, vx, vy, vw, vh);

            GL.PopMatrix();
        }

        void DrawLinesTransformed(List<Line3> lines, Mat4 m, Mat4 p, float vx, float vy, float vw, float vh) {
            // --- CUBE (non-axis, non-grid) ---
            GL.Begin(GL.LINES);
            GL.Color(new Color(1, 1, 1, 1)); // white
            for (int i = 0; i < lines.Count; i++) {
                var ln = lines[i];
                DrawLineObject(ln.a, ln.b, m, p, vx, vy, vw, vh);
            }
            GL.End();
        }

        bool IsAxis(Line3 ln, int axis) // 0=x,1=y,2=z
        {
            if (axis == 0) return IsSame(ln.a, new Vec3(0, 0, 0)) && (ln.b.y == 0 && ln.b.z == 0);
            if (axis == 1) return IsSame(ln.a, new Vec3(0, 0, 0)) && (ln.b.x == 0 && ln.b.z == 0);
            return IsSame(ln.a, new Vec3(0, 0, 0)) && (ln.b.x == 0 && ln.b.y == 0);
        }

        bool IsSame(Vec3 a, Vec3 b) {
            return (Math.Abs(a.x - b.x) < 1e-6f) && (Math.Abs(a.y - b.y) < 1e-6f) && (Math.Abs(a.z - b.z) < 1e-6f);
        }

        void DrawLineObject(Vec3 aObj, Vec3 bObj, Mat4 m, Mat4 p, float vx, float vy, float vw, float vh) {
            //var aClip = Mat4.Mul(p, Mat4.Mul(m, Vec4.FromPoint(aObj)));
            //var bClip = Mat4.Mul(p, Mat4.Mul(m, Vec4.FromPoint(bObj)));

            var pm = p * m;         // composite once
            var aClip = pm * Vec4.From3DPoint(aObj);    // then apply to each vertex
            var bClip = pm * Vec4.From3DPoint(bObj);


            // perspective divide -> NDC
            Vec3 aNdc = aClip.Homogenized();
            Vec3 bNdc = bClip.Homogenized();

            if (BothOutside(aNdc, bNdc)) return;

            Vector2 aPix = NdcToPixel(aNdc, vx, vy, vw, vh);
            Vector2 bPix = NdcToPixel(bNdc, vx, vy, vw, vh);

            GL.Vertex3(aPix.x, aPix.y, 0);
            GL.Vertex3(bPix.x, bPix.y, 0);
        }

        bool BothOutside(Vec3 a, Vec3 b) {
            if (a.x < -1 && b.x < -1) return true;
            if (a.x > 1 && b.x > 1) return true;
            if (a.y < -1 && b.y < -1) return true;
            if (a.y > 1 && b.y > 1) return true;
            if (a.z < -1 && b.z < -1) return true;
            if (a.z > 1 && b.z > 1) return true;
            return false;
        }

        Vector2 NdcToPixel(Vec3 ndc, float vx, float vy, float vw, float vh) {
            float sx = (ndc.x * 0.5f + 0.5f) * vw + vx;
            float syUp = (ndc.y * 0.5f + 0.5f) * vh + vy; // origin bottom-left
            float sy = (vy + vh) - (syUp - vy);           // flip to top-left
            return new Vector2(sx, sy);
        }
    }
}