using Fantasy;
using Hotfix;

public static class Extension {
    public static Vector3 ToVector3(this CSVector3 v) {
        return new Vector3(v.x, v.y, v.z);
    }
}