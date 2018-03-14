﻿using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KP.GmailClient.Common
{
    /// <summary>
    /// Common extensions.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        public static string GetValidFilename(this string name, char replaceWith = '_')
        {
            if (Path.GetInvalidFileNameChars().Contains(replaceWith))
            {
                throw new Exception(string.Concat("Replacement char '", replaceWith, "' is not valid!"));
            }
            if (name.Length > 256)// total file including path max is 256 chars
            {
                name = new string(name.Take(260).ToArray());
            }

            return new string(name.Select(s => Path.GetInvalidFileNameChars().Contains(s) ? replaceWith : s).ToArray());
        }

        /// <summary>
        /// Get the Attribute of an Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetAttribute<T, T2>(this T2 value)
            where T : Attribute
            where T2 : struct, IConvertible// enum
        {
            Type type = typeof(T2);
            string name = Enum.GetName(type, value);

            return type.GetField(name).GetCustomAttribute<T>();
        }

        /// <summary>
        /// Get the integer values of an Enum which uses [Flags]
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static T[] GetFlagEnumValues<T>(this T e)
            where T : struct, IConvertible // = enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<int>()
                .Where(s => s > 0)// Flag enum should start with 1 (2,4,etc)
                .Where(f => (f & Convert.ToInt32(e)) == f)
                .Cast<T>()
                .ToArray();
        }

        public static T ParseEnumValue<T>(this Enum action)
            where T : struct, IConvertible // = enum
        {
            int value = Convert.ToInt32(action);
            string enumValueName = action.ToString();
            if (!Enum.IsDefined(typeof(T), enumValueName))
            {
                throw new InvalidEnumArgumentException(nameof(action), value, action.GetType());
            }

            return (T)Enum.Parse(typeof(T), enumValueName);
        }

        /// <summary>
        /// Encodes to an URL Base64 encoded string
        /// See http://tools.ietf.org/html/rfc4648#section-5: 'Base 64 Encoding with URL and Filename Safe Alphabet'
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string ToBase64UrlString(this string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return plainText;
            }

            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
            return base64.Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// Decodes an URL Base64 encoded string
        /// See http://tools.ietf.org/html/rfc4648#section-5: 'Base 64 Encoding with URL and Filename Safe Alphabet'
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string FromBase64UrlString(this string base64)
        {
            if (string.IsNullOrEmpty(base64))
            {
                return base64;
            }

            string safeBase64 = base64.Replace('-', '+').Replace('_', '/');
            byte[] bytes = Convert.FromBase64String(safeBase64);
            return Encoding.UTF8.GetString(bytes);
        }

        public static long ToUnixTime(this DateTime dateTime)
        {
            return (int)dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// Append line if condition is true.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        public static StringBuilder Append(this StringBuilder builder, string value, bool condition)
        {
            if (!condition)
            {
                return builder;
            }

            builder.Append(value);
            return builder;
        }
    }
}
