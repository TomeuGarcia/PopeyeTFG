using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public static class DataEventsParsing
    {
        public static string ToStringParsed(this Vector3 vector, string format = "F", string separator = ",")
        {
            string content =
                vector.x.ToString(format) + separator +
                vector.y.ToString(format) + separator +
                vector.z.ToString(format);

            return content;
        }
    }
}