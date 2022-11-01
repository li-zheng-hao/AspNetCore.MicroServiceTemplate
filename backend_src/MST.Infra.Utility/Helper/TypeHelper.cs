namespace MST.Infra.Utility.Helper;

public static class TypeUtil
{
    public static bool IsSubClassOrEqualEx(this Type exA,Type exB)
    {
        return exA.IsSubclassOf(exB) || exA == exB;
    }
}