/*

using MyCustomMath;    // your Mat4 lives here
using UnityEngine;   // MonoBehaviour + GUI only

public class Mat4GUI : MonoBehaviour {
    
    [SerializeField] float m00, m01, m02, m03;
    [SerializeField] float m10, m11, m12, m13;
    [SerializeField] float m20, m21, m22, m23;
    [SerializeField] float m30, m31, m32, m33;
    [SerializeField] float ax = 1f, ay = 2f, az = 3f, aw = 1f;
    [SerializeField] float tx = 1f, ty = 2f, tz = 3f;
    [SerializeField] float sx = 1f, sy = 2f, sz = 3f;
    [SerializeField] float rotateDegreesX = 0f;
    [SerializeField] float rotateDegreesY = 0f;
    [SerializeField] float rotateDegreesZ = 0f;

    Rect win = new Rect(12, 12, 1200, 520);

    void OnGUI() {
        win = GUI.Window(42, win, DrawWindow, "Vector Display (CustomMath.Mat4)");
    }

    void DrawWindow(int id) {
        GUILayout.BeginVertical();

        
        GUILayout.Label("Matrix");
        Slider(ref m00, "m00", -5f, 5f);
        Slider(ref m01, "m01", -5f, 5f);
        Slider(ref m02, "m02", -5f, 5f);
        Slider(ref m03, "m03", -5f, 5f);

        Slider(ref m10, "m10", -5f, 5f);
        Slider(ref m11, "m11", -5f, 5f);
        Slider(ref m12, "m12", -5f, 5f);
        Slider(ref m13, "m13", -5f, 5f);

        Slider(ref m20, "m20", -5f, 5f);
        Slider(ref m21, "m21", -5f, 5f);
        Slider(ref m22, "m22", -5f, 5f);
        Slider(ref m23, "m23", -5f, 5f);

        Slider(ref m30, "m30", -5f, 5f);
        Slider(ref m31, "m31", -5f, 5f);
        Slider(ref m32, "m32", -5f, 5f);
        Slider(ref m33, "m33", -5f, 5f);
        

        // Build vectors and compute with your custom math
        var vector = new Vec4(ax, ay, az, aw);
        var matrix = new Mat4(m00, m01, m02, m03,
                              m10, m11, m12, m13,
                              m20, m21, m22, m23,
                              m30, m31, m32, m33);

        var identityMatrix = Mat4.Identity();
        var multiplyMatrixByMatrix = matrix * matrix;
        var multiplyMatrixByVector = matrix * vector;
        var translatedMatrix = matrix.Translate(tx, ty, tz);
        var scaledMatrix = matrix.Scale(sx, sy, sz);
        var rotateMatrixX = matrix.RotationX(rotateDegreesX);
        var rotateMatrixY = matrix.RotationY(rotateDegreesY);
        var rotateMatrixZ = matrix.RotationZ(rotateDegreesZ);

        // Basic operations in left column
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical(GUILayout.Width(400));
        GUILayout.Label("Matrix:");
        GUILayout.Label(matrix.ToString());
        GUILayout.Space(5);

        GUILayout.Label("Identity Matrix:");
        GUILayout.Label(identityMatrix.ToString());
        GUILayout.Space(5);

        GUILayout.Label("Matrix * Matrix:");
        GUILayout.Label(multiplyMatrixByMatrix.ToString());
        GUILayout.Space(5);

        GUILayout.Label("Matrix * Vector:");
        GUILayout.Label(multiplyMatrixByVector.ToString());
        GUILayout.EndVertical();

        // Transform operations in right column  
        GUILayout.BeginVertical(GUILayout.Width(400));
        GUILayout.Label("Apply Translation to Matrix:");
        GUILayout.Label(translatedMatrix.ToString());
        GUILayout.Space(5);

        GUILayout.Label("Apply Scaling to Matrix:");
        GUILayout.Label(scaledMatrix.ToString());
        GUILayout.Space(5);
        GUILayout.EndVertical();

        GUILayout.BeginVertical(GUILayout.Width(400));
        GUILayout.Label("Apply Rotation in X axis to Matrix:");
        GUILayout.Label(rotateMatrixX.ToString());
        GUILayout.Space(5);

        GUILayout.Label("Apply Rotation in Y axis to Matrix:");
        GUILayout.Label(rotateMatrixY.ToString());
        GUILayout.Space(5);

        GUILayout.Label("Apply Rotation in Z axis to Matrix:");
        GUILayout.Label(rotateMatrixZ.ToString());
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        // Drag by title bar
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    // --- UI helpers (no Mathf used) ---
    static void Slider(ref float v, string label, float min, float max) {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label + "  " + F(v), GUILayout.Width(110));
        v = GUILayout.HorizontalSlider(v, min, max);
        GUILayout.EndHorizontal();
    }

    // Lightweight number -> string (round ~3 decimals) without format strings
    static string F(float value) {
        float sign = value < 0f ? -1f : 1f;
        float a = value * sign;
        int scaled = (int)(a * 1000f + 0.5f);
        float r = (scaled / 1000f) * sign;
        return r.ToString();
    }

    static float Rand(float min, float max) => min + (max - min) * Random.value; // UI-only

}
*/
