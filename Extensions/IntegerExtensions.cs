namespace TodoApiNet.Extensions
{
    public static class IntegerExtensions
    {
        public static bool IsNotEqual(this int integer, int value) => integer != value ? true : false;
    }
}