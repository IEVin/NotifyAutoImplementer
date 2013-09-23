﻿using System;

namespace NotifyAutoImplementer.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NotifyPropertyAttribute : Attribute
    {
        readonly string _propertyName;

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public NotifyPropertyAttribute()
        {
        }

        public NotifyPropertyAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class NotifyAllProperty : Attribute
    {
        public bool ThrowOnNoVirtual { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SuppressNotifyAttribute : Attribute
    {
    }
}