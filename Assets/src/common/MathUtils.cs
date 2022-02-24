using UnityEngine;

public class MathUtils : MonoBehaviour
{

    public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
        {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }
        else return a * Quaternion.Inverse(b);
    }

    public static Vector3 xzdir(Vector3 a, Vector3 b){
        Vector3 a1 = a;
        a1.y=0;
        Vector3 b1 = b;
        b1.y=0;
        return b1-a1;
    }

    public static float xzdist(Vector3 a, Vector3 b){
        Vector3 a1 = a;
        a1.y = 0;
        Vector3 b1 = b;
        b1.y=0;
        return Vector3.Distance(a1,b1);
    }

    public static Quaternion Multiply(Quaternion input, float scalar) 
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }
}