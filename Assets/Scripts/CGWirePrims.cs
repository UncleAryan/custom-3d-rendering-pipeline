// Wireframe primitives as line segments (object space)
// Namespace: CG
using System.Collections.Generic;
using MyCustomMath;

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
    }
}
