﻿using System;
using System.Runtime.InteropServices;

namespace Vulkan {
    public unsafe static partial class Helper {

        //public static string[] Get(IntPtr target, UInt32 count) {
        //    string[] result = null;
        //    var pointer = (IntPtr*)target;
        //    if (pointer != null) {
        //        result = new string[count];
        //        for (int i = 0; i < count; i++) {
        //            result[i] = Marshal.PtrToStringAnsi(pointer[i]);
        //        }
        //    }
        //    return result;
        //}

        //public static void DisposeStrings(ref IntPtr target, ref UInt32 count) {
        //    var pointer = (IntPtr*)target;
        //    if (pointer != null) {
        //        for (int i = 0; i < count; i++) {
        //            Marshal.FreeHGlobal(pointer[i]);
        //        }
        //        target = IntPtr.Zero;
        //        count = 0;
        //    }
        //}

        ///// <summary>
        ///// Set a string to specified <paramref name="target"/>.
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of string.</param>
        //public static void Set(this string value, ref IntPtr target) {
        //    {   // free unmanaged memory.
        //        if (target != IntPtr.Zero) {
        //            Marshal.FreeHGlobal(target);
        //            target = IntPtr.Zero;
        //        }
        //    }
        //    {
        //        if (value != null && value.Length > 0) {
        //            target = Marshal.StringToHGlobalAnsi(value);
        //        }
        //        else {
        //            target = IntPtr.Zero;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Set an array of strings to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this string[] value, ref IntPtr* target, ref UInt32 count) {
        //    {   // free unmanaged memory.
        //        if (target != null) {
        //            for (int i = 0; i < count; i++) {
        //                Marshal.FreeHGlobal(target[i]);
        //            }
        //        }
        //    }
        //    {
        //        int length = value.Length;
        //        if (length > 0) {
        //            int elementSize = Marshal.SizeOf(typeof(IntPtr));
        //            int byteLength = (int)(length * elementSize);
        //            IntPtr array = Marshal.AllocHGlobal(byteLength);
        //            IntPtr* pointer = (IntPtr*)array.ToPointer();
        //            for (int i = 0; i < length; i++) {
        //                IntPtr str = Marshal.StringToHGlobalAnsi(value[i]);
        //                pointer[i] = str;
        //            }
        //            target = pointer;
        //        }
        //        count = (UInt32)length;
        //    }
        //}

        //public static T[] Get<T>(IntPtr target, UInt32 count) where T : struct {
        //    T[] result = null;
        //    if (target != IntPtr.Zero) {
        //        result = new T[count];
        //        if (count > 0) {
        //            GCHandle pin = GCHandle.Alloc(result, GCHandleType.Pinned);
        //            IntPtr address = Marshal.UnsafeAddrOfPinnedArrayElement(result, 0);
        //            var dst = (byte*)address;
        //            var src = (byte*)target;
        //            int elementSize = Marshal.SizeOf(typeof(T));
        //            int byteLength = (int)(count * elementSize);
        //            for (int i = 0; i < byteLength; i++) {
        //                dst[i] = src[i];
        //            }
        //            pin.Free();
        //        }
        //    }

        //    return result;
        //}

        //public static void DisposeStructs(ref IntPtr target, ref UInt32 count) {
        //    if (target != IntPtr.Zero) {
        //        Marshal.FreeHGlobal(target);
        //        target = IntPtr.Zero;
        //        count = 0;
        //    }
        //}
        /// <summary>
        /// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        /// <para>Enumeration types are not allowed to use this method.
        /// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target">address of first element/array.</param>
        /// <param name="count">How many elements?</param>
        public static void Set<T>(this T[] value, ref IntPtr target, ref UInt32 count) where T : struct {
            {   // free unmanaged memory.
                if (target != IntPtr.Zero) {
                    Marshal.FreeHGlobal(target);
                    target = IntPtr.Zero;
                    count = 0;
                }
            }

            if (value != null) {
                int length = value.Length;

                if (typeof(T).IsEnum) { // if T is an enum type.(eg. enum VkResult : int { .. } )
                    Type underlying = typeof(T).GetEnumUnderlyingType(); // underlying : int
                    int elementSize = Marshal.SizeOf(underlying); // elementSize : sizeof(int) = 4

                    int byteLength = length * elementSize;
                    IntPtr array = Marshal.AllocHGlobal(byteLength);
                    if (elementSize == 1) {
                        var dst = (byte*)array;
                        for (int i = 0; i < length; i++) { dst[i] = Convert.ToByte(value[i]); }
                    }
                    else if (elementSize == 2) {
                        var dst = (Int16*)array;
                        for (int i = 0; i < length; i++) { dst[i] = Convert.ToInt16(value[i]); }
                    }
                    else if (elementSize == 4) {
                        var dst = (Int32*)array;
                        for (int i = 0; i < length; i++) { dst[i] = Convert.ToInt32(value[i]); }
                    }
                    else if (elementSize == 8) {
                        var dst = (Int64*)array;
                        for (int i = 0; i < length; i++) { dst[i] = Convert.ToInt64(value[i]); }
                    }
                    else {
                        throw new ArgumentException(string.Format("Unknown type({0}) length", typeof(T)));
                    }

                    target = array;
                }
                else { // when T is a regular struct.
                    int elementSize = Marshal.SizeOf(typeof(T));

                    int byteLength = length * elementSize;
                    IntPtr array = Marshal.AllocHGlobal(byteLength);
                    var dst = (byte*)array;
                    GCHandle pin = GCHandle.Alloc(value, GCHandleType.Pinned);
                    IntPtr address = Marshal.UnsafeAddrOfPinnedArrayElement(value, 0);
                    var src = (byte*)address;
                    for (int i = 0; i < byteLength; i++) {
                        dst[i] = src[i];
                    }
                    //System.Buffer.MemoryCopy(src, dst, byteLength, byteLength);
                    pin.Free();

                    target = array;
                }

                count = (UInt32)length;
            }
        }

        ///// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this Boolean[] value, ref Boolean* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (Boolean*)ptr;
        //}
        //// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this Byte[] value, ref Byte* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (Byte*)ptr;
        //}
        //// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this Int16[] value, ref Int16* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (Int16*)ptr;
        //}
        //// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this Int32[] value, ref Int32* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (Int32*)ptr;
        //}
        //// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this Int64[] value, ref Int64* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (Int64*)ptr;
        //}
        //// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this Char[] value, ref Char* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (Char*)ptr;
        //}
        //// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this SByte[] value, ref SByte* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (SByte*)ptr;
        //}
        //// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this UInt16[] value, ref UInt16* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (UInt16*)ptr;
        //}
        //// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this UInt32[] value, ref UInt32* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (UInt32*)ptr;
        //}
        //// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this UInt64[] value, ref UInt64* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (UInt64*)ptr;
        //}
        //// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this Single[] value, ref Single* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (Single*)ptr;
        //}
        //// <summary>
        ///// Set an array of structs to specified <paramref name="target"/> and <paramref name="count"/>.
        ///// <para>Enumeration types are not allowed to use this method.
        ///// If you have to, convert them to byte/short/ushort/int/uint according to their underlying types first.</para>
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="target">address of first element/array.</param>
        ///// <param name="count">How many elements?</param>
        //public static void Set(this Double[] value, ref Double* target, ref UInt32 count) {
        //    IntPtr ptr = (IntPtr)target;
        //    Set(value, ref ptr, ref count);
        //    target = (Double*)ptr;
        //}

        ///// <summary>
        /////
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="structObj"></param>
        ///// <returns></returns>
        //static byte[] ToBytes<T>(this T structObj) where T : struct {
        //    Int32 size = Marshal.SizeOf(structObj);
        //    Byte[] bytes = new Byte[size];
        //    IntPtr buffer = IntPtr.Zero;
        //    try {
        //        buffer = Marshal.AllocHGlobal(size);
        //        Marshal.StructureToPtr(structObj, buffer, false);
        //        Marshal.Copy(buffer, bytes, 0, size);
        //    } finally {
        //        Marshal.FreeHGlobal(buffer);
        //    }

        //    return bytes;
        //}
    }
}
