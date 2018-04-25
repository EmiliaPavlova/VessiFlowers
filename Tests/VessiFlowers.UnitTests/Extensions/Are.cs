namespace VessiFlowers.UnitTests.Extensions
{
    using System;
    using System.Collections;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Custom assert methods
    /// </summary>
    public static class Are
    {
        /// <summary>
        /// Check expected and actual objects for equality
        /// </summary>
        /// <param name="expected">Expected object</param>
        /// <param name="actual">Actual object</param>
        /// <returns>True if the objects are equal</returns>
        public static bool ObjectsEqual(object expected, object actual)
        {
            if (expected?.GetType() != actual?.GetType())
            {
                Assert.Fail("Expected type is {0}, but actual type is {1}", expected?.GetType(), actual?.GetType());
            }

            PropertyInfo[] properties = expected.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object expectedValue = property.GetValue(expected, null);
                object actualValue = property.GetValue(actual, null);

                if (actualValue is IList)
                {
                    ListsEqual((IList)expectedValue, (IList)actualValue);
                }
                else if (!IsEqual(expectedValue, actualValue))
                {
                    Assert.Fail("Property {0}.{1} does not match. Expected: {2} but was: {3}", property.DeclaringType.Name, property.Name, expectedValue, actualValue);
                }
            }

            return true;
        }

        /// <summary>
        /// Check expected and actual lists for equality
        /// </summary>
        /// <param name="expectedList">Expected list</param>
        /// <param name="actualList">Actual list</param>
        /// <returns>True if the Lists are equal</returns>
        public static bool ListsEqual(IList expectedList, IList actualList)
        {
            if (expectedList.Count != actualList.Count)
            {
                Assert.Fail("Expected IList containing {2} elements but was IList containing {3} elements", expectedList.Count, actualList.Count);
            }

            for (int i = 0; i < actualList.Count; i++)
            {
                if (!ObjectsEqual(expectedList[i], actualList[i]))
                {
                    Assert.Fail("Expected IList with element {1} equals to {2} but was IList with element {1} equals to {3}", expectedList[i], actualList[i]);
                }
            }

            return true;
        }

        private static bool IsEqual(object expected, object actual)
        {
            if (expected == null && actual == null)
            {
                return true;
            }

            if (IsSimpleType(expected?.GetType()) && IsSimpleType(actual?.GetType()))
            {
                return Equals(expected, actual);
            }
            else
            {
                return ObjectsEqual(expected, actual);
            }
        }

        private static bool IsSimpleType(Type type)
        {
            return
             type == typeof(object) ||
             type == typeof(string) ||
             type == typeof(char) ||
             type == typeof(bool) ||
             type == typeof(byte) ||
             type == typeof(short) ||
             type == typeof(int) ||
             type == typeof(long) ||
             type == typeof(ushort) ||
             type == typeof(uint) ||
             type == typeof(ulong) ||
             type == typeof(float) ||
             type == typeof(double) ||
             type == typeof(decimal) ||
             type == typeof(DateTime);
        }
    }
}
