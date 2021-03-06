﻿using System;
using System.Runtime.InteropServices;

namespace IEVin.PropertyChangedNotificator.Helper
{
    [StructLayout(LayoutKind.Explicit)]
    struct ObjectTypeStruct
    {
        [FieldOffset(0)]
        public IntPtr MethodTable;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct ObjectToStructConv
    {
        [FieldOffset(0)]
        public ObjWrap From;

        [FieldOffset(0)]
        public InfoWrap To;
    }

    class ObjWrap
    {
        public object Value;
    }

    unsafe class InfoWrap
    {
#pragma warning disable 0649
        public ObjectTypeStruct* Value;
#pragma warning restore 0649
    }
}