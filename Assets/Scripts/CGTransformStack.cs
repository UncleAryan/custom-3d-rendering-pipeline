using System.Collections.Generic;

namespace MyCustomMath {
    public class CGTransformStack {
        List<Mat4> stack = new List<Mat4>();
        private Mat4 current;
        public CGTransformStack() {
            current = Mat4.Identity();
        }

        public Mat4 Current => current;

        public void LoadIdentity() {
            current = Mat4.Identity();
        }

        public void Push() {
            stack.Add(current);
        }

        public void Pop() {
            if(stack.Count > 0) {
                current = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
            }
        }

        public void Mult(Mat4 m) {
            current *= m;
        }

        public void Translate(float x, float y, float z) {
            Mult(Mat4.Translate(x, y, z));
        }

        public void Scale(float x, float y, float z) {
            Mult(Mat4.Scale(x, y, z));
        }

        public void RotateX(float degrees) {
            Mult(Mat4.RotationX(degrees));    
        }

        public void RotateY(float degrees) {
            Mult(Mat4.RotationY(degrees));
        }

        public void RotateZ(float degrees) {
            Mult(Mat4.RotationZ(degrees));
        }
    }
}