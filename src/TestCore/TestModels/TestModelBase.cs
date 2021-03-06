﻿namespace IEVin.PropertyChangedNotificator.TestCore.TestModels
{
    public class TestModelBase : NotificationObject
    {
        [NotifyProperty]
        public virtual int NotifyProperty { get; set; }

        public int NotNotifyProperty { get; set; }

        public virtual int VirtualNotNotifyProperty { get; set; }

        [NotifyProperty]
        [NotifyProperty("OtherNotifyProperty")]
        public virtual int MultyNotifyProperty { get; set; }

        [NotifyProperty]
        public virtual int OtherNotifyProperty { get; set; }

        [NotifyProperty]
        public virtual double DoubleNotifyProperty { get; set; }

        [NotifyProperty]
        public virtual float FloatNotifyProperty { get; set; }

        public TestModelBase()
        {
            Notificator.Create(this);
        }
    }
}