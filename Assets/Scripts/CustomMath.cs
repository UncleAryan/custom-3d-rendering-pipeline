using UnityEngine;

namespace MyCustomMath {
    public struct Vec3 {
        public float x, y, z;

        public Vec3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vec3 Zero => new Vec3(0f, 0f, 0f);
        public static Vec3 One => new Vec3(1f, 1f, 1f);
        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vec3 operator -(Vec3 a) => new Vec3(-a.x, -a.y, -a.z);
        public static Vec3 operator *(Vec3 a, float s) => new Vec3(s * a.x, s * a.y, s * a.z);
        public static Vec3 operator *(float s, Vec3 a) => a * s;
        public static Vec3 operator /(Vec3 a, float s) {
            if (s > -1e-8f && s < 1e-8f) return a;
            float inv = 1 / s;
            return new Vec3(a.x * inv, a.y * inv, a.z * inv);
        }
        public static Vec3 operator /(float s, Vec3 a) => a / s;
        public static float Dot(Vec3 a, Vec3 b) => (a.x * b.x + a.y * b.y + a.z * b.z);
        public static Vec3 Cross(Vec3 a, Vec3 b) => new Vec3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        public float SqrMagnitude() => x * x + y * y + z * z;
        public float Magnitude() => NewtonRaphsonSqrt(SqrMagnitude());
        public Vec3 Normalize() {
            if (Magnitude() > 1E-08f) {
                return this / Magnitude();
            }
            return this;
        }

        public override string ToString() {
            return "(" + x + ", " + y + ", " + z + ")";
        }

        public float NewtonRaphsonSqrt(float n) {
            float guess = n;
            float previousGuess = guess;
            float precision = 1E-08f;

            guess = (guess + (n / guess)) / 2;
            while (Mathf.Abs(guess - previousGuess) > precision) {
                previousGuess = guess;
                guess = (guess + (n / guess)) / 2;
            }

            return guess;
        }
    }
    
    public struct Vec4 {
        public float x, y, z, w;

        public Vec4(float x, float y, float z, float w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Vec4 From3DPoint(Vec3 p) => new Vec4(p.x, p.y, p.z, 1f);
        public static Vec4 From3DVector(Vec3 v) => new Vec4(v.x, v.y, v.z, 0f);
        public Vec3 XYZ() => new Vec3(x, y, z);
        public Vec3 Homogenized() {
            if(w > -1e-8f && w < 1e-8f) {
                return new Vec3(x, y, z);
            }
            float invW = 1.0f / w;
            return new Vec3(x * invW, y * invW, z * invW);
        }

        public override string ToString() {
            return "[" + x + "]\n" + "[" + y + "]\n" + "[" + z + "]\n" + "[" + w + "]";
        }
    }

    public struct Mat4 {
        public float m00, m01, m02, m03, 
                     m10, m11, m12, m13,
                     m20, m21, m22, m23,
                     m30, m31, m32, m33;

        public Mat4(float m00, float m01, float m02, float m03,
                    float m10, float m11, float m12, float m13,
                    float m20, float m21, float m22, float m23,
                    float m30, float m31, float m32, float m33) {
            this.m00 = m00; this.m01 = m01; this.m02 = m02; this.m03 = m03;
            this.m10 = m10; this.m11 = m11; this.m12 = m12; this.m13 = m13;
            this.m20 = m20; this.m21 = m21; this.m22 = m22; this.m23 = m23;
            this.m30 = m30; this.m31 = m31; this.m32 = m32; this.m33 = m33;
        }
        public static Mat4 Identity() {
            Mat4 m = new Mat4();

            m.m00 = 1;
            m.m11 = 1;
            m.m22 = 1;
            m.m33 = 1;
            
            return m;
        }

        public static Mat4 operator *(Mat4 a, Mat4 b) {
            return new Mat4(
                a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30,
                a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31,
                a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32,
                a.m00 * b.m03 + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33,

                a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20 + a.m13 * b.m30,
                a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31,
                a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32,
                a.m10 * b.m03 + a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33,

                a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20 + a.m23 * b.m30,
                a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31,
                a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32,
                a.m20 * b.m03 + a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33,

                a.m30 * b.m00 + a.m31 * b.m10 + a.m32 * b.m20 + a.m33 * b.m30,
                a.m30 * b.m01 + a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31,
                a.m30 * b.m02 + a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32,
                a.m30 * b.m03 + a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33
            );
        }

        public static Vec4 operator *(Mat4 m, Vec4 v) {
            return new Vec4(
                m.m00 * v.x + m.m01 * v.y + m.m02 * v.z + m.m03 * v.w,
                m.m10 * v.x + m.m11 * v.y + m.m12 * v.z + m.m13 * v.w,
                m.m20 * v.x + m.m21 * v.y + m.m22 * v.z + m.m23 * v.w,
                m.m30 * v.x + m.m31 * v.y + m.m32 * v.z + m.m33 * v.w
            );
        }

        public static Mat4 Translate(float tx, float ty, float tz) {
            Mat4 m = Identity();
            m.m03 = tx;
            m.m13 = ty;
            m.m23 = tz;
            return m;
        }

        public static Mat4 Scale(float sx, float sy, float sz) {
            Mat4 m = Identity();
            m.m00 = sx;
            m.m11 = sy;
            m.m22 = sz;
            return m;
        }

        public static Mat4 RotationX(float degrees) {
            float cos = (float)Mathf.Cos(degrees * Mathf.Deg2Rad);
            float sin = (float)Mathf.Sin(degrees * Mathf.Deg2Rad);

            Mat4 m = Identity();

            m.m11 = cos;
            m.m12 = -sin;
            m.m21 = sin;
            m.m22 = cos;

            return m;
        }

        public static Mat4 RotationY(float degrees) {
            float cos = (float)Mathf.Cos(degrees * Mathf.Deg2Rad);
            float sin = (float)Mathf.Sin(degrees * Mathf.Deg2Rad);

            Mat4 m = Identity();

            m.m00 = cos;
            m.m02 = sin;
            m.m20 = -sin;
            m.m22 = cos;

            return m;
        }

        public static Mat4 RotationZ(float degrees) {
            float cos = (float)Mathf.Cos(degrees * Mathf.Deg2Rad);
            float sin = (float)Mathf.Sin(degrees * Mathf.Deg2Rad);

            Mat4 m = Identity();

            m.m00 = cos;
            m.m01 = -sin;
            m.m10 = sin;
            m.m11 = cos;

            return m;
        }


        public override string ToString() {
            return "[" + m00 + ", " + m01 + ", " + m02 + ", " + m03 + "]\n" +
                   "[" + m10 + ", " + m11 + ", " + m12 + ", " + m13 + "]\n" +
                   "[" + m20 + ", " + m21 + ", " + m22 + ", " + m23 + "]\n" +
                   "[" + m30 + ", " + m31 + ", " + m32 + ", " + m33 + "]\n";
        }

        public static Mat4 OrthographicProjection(float left, float right, float bottom, float top, float near, float far) {
            Mat4 m = Identity();

            m.m00 = 2 / (right - left);
            m.m11 = 2 / (top - bottom);
            m.m22 = 2 / (near - far);
            m.m03 = -(right +  left) / (right - left);
            m.m13 = -(top + bottom) / (top - bottom);
            m.m23 = (far + near) / (far - near);

            return m;
        }

        public static Mat4 PerspectiveProjection(float fovDegrees, float aspect, float near, float far) {
            float fovRad = fovDegrees * Mathf.Deg2Rad;
            float tanHalfFov = Mathf.Tan(fovRad * 0.5f);
            float range = far - near;  // CORRECTED: far - near

            Mat4 m = new Mat4();

            m.m00 = 1.0f / (aspect * tanHalfFov);
            m.m11 = 1.0f / tanHalfFov;
            m.m22 = -(far + near) / range;  // CORRECTED: negative
            m.m23 = -(2.0f * far * near) / range;  // CORRECTED: negative
            m.m32 = -1.0f;
            m.m33 = 0f;

            // Zero out other elements
            m.m01 = 0; m.m02 = 0; m.m03 = 0;
            m.m10 = 0; m.m12 = 0; m.m13 = 0;
            m.m20 = 0; m.m21 = 0; m.m30 = 0;
            m.m31 = 0;

            return m;
        }
    }
}