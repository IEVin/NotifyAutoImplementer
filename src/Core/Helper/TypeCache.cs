﻿using System;

namespace IEVin.PropertyChangedNotificator.Helper
{
    public class TypeCache<T>
    {
        // ReSharper disable once StaticMemberInGenericType
        static Type s_type;

        public static Type GetOrCreate(Func<Type, Type> creator)
        {
            if(s_type != null)
                return s_type;

            lock(typeof(TypeCache<T>).FullName)
                return s_type ?? (s_type = creator(typeof(T)));
        }
    }
}