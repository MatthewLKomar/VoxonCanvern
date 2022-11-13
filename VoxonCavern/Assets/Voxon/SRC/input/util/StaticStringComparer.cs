﻿// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public class StaticStringComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x == y;
        }

        public int GetHashCode(string str)
        {
            unchecked
            {
                var hash1 = 5381;
                int hash2 = hash1;

                for (var i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}