// Wireframe primitives as line segments (object space)
// Namespace: CG
using System.Collections.Generic;
using MyCustomMath;
using UnityEngine;

namespace MyCustomMath {
    public struct Line3 {
        public Vec3 a, b;
        public Line3(Vec3 a, Vec3 b) { this.a = a; this.b = b; }
    }

    public static class CGWirePrims {
        public static List<Line3> Cube(float size = 1f) {
            float s = size * 0.5f;
            Vec3[] v = new Vec3[] {
                new Vec3(-s,-s,-s), new Vec3(+s,-s,-s),
                new Vec3(+s,+s,-s), new Vec3(-s,+s,-s),
                new Vec3(-s,-s,+s), new Vec3(+s,-s,+s),
                new Vec3(+s,+s,+s), new Vec3(-s,+s,+s)
            };
            int[,] e = new int[,] {
                {0,1},{1,2},{2,3},{3,0}, // bottom face
                {4,5},{5,6},{6,7},{7,4}, // top face
                {0,4},{1,5},{2,6},{3,7}  // verticals
            };
            var L = new List<Line3>(12);
            for (int i = 0; i < 12; i++) L.Add(new Line3(v[e[i, 0]], v[e[i, 1]]));
            return L;
        }

        // TODO A1: inside CG_WirePrims (same class as Cube)
        public static List<Line3> Axis(float L = 5f) {
            // Create a new List<Line3> with capacity 6.
            var lines = new List<Line3>(6);

            // Add +/-X from (0,0,0) to (+L,0,0) and (0,0,0) to (-L,0,0).
            lines.Add(new Line3(new Vec3(0, 0, 0), new Vec3(+L, 0, 0)));
            lines.Add(new Line3(new Vec3(0, 0, 0), new Vec3(-L, 0, 0)));

            // Add +/-Y similarly.
            lines.Add(new Line3(new Vec3(0, 0, 0), new Vec3(0, +L, 0)));
            lines.Add(new Line3(new Vec3(0, 0, 0), new Vec3(0, -L, 0)));

            // Add +/-Z similarly.
            lines.Add(new Line3(new Vec3(0, 0, 0), new Vec3(0, 0, +L)));
            lines.Add(new Line3(new Vec3(0, 0, 0), new Vec3(0, 0, -L)));

            // Return the list.
            return lines;
        }

        /*
           Camera Position: (0, 0, 0) looking down +Z axis
           Grid Position:   Entirely at Y = 0 (XZ plane)
           Result: The grid is exactly in the camera's "horizon line"
        */

        // TODO A2: inside CG_WirePrims
        public static List<Line3> GridXZ(float extent = 8f, float step = 1f) {
            // Guard: if (step <= 0) step = 1; if (extent < step) extent = step;
            if (step <= 0) step = 1f;
            if (extent < step) extent = step;

            

            // int N = ceil(extent / step); float E = N * step;
            int N = Mathf.CeilToInt(extent / step);
            float E = N * step;


            var lines = new List<Line3>();

            // Loop i=-N..N to add lines parallel to X: from (-E,0,z=i*step) to (+E,0,z=i*step).
            for (int i = -N; i <= N; i++) {
                float z = i * step;
                lines.Add(new Line3(new Vec3(-E, 0, z), new Vec3(+E, 0, z)));
            }

            // Loop i=-N..N to add lines parallel to Z: from (x=i*step,0,-E) to (x=i*step,0,+E).
            for (int i = -N; i <= N; i++) {
                float x = i * step;
                lines.Add(new Line3(new Vec3(x, 0, -E), new Vec3(x, 0, +E)));
            }

            // Return the list.
            return lines;
        }
    }
}
