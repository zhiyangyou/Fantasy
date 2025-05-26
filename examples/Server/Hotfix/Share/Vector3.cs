using Fantasy;

namespace Hotfix;

public class Vector3 {
    public static Vector3 zero => new Vector3(0, 0, 0);

    public float x;
    public float y;
    public float z;

    public Vector3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3() { }

    public override string ToString() {
        return $"x:{x} y:{y} z:{z}";
    }

    public CSVector3 ToCSVector3() {
        return new CSVector3() {
            x = this.x,
            y = this.y,
            z = this.z
        };
    }
    
}