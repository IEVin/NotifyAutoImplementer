﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using IEVin.NotifyAutoImplementer.Core;
using IEVin.NotifyAutoImplementer.TestCore.TestModels;
using NUnit.Framework;

namespace IEVin.NotifyAutoImplementer.TestCore
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void SimpleNotifyPropertyTest()
        {
            var model = Notifier.Of<TestModelBase>();

            var counter = 0;
            SetChangedAction(model, (TestModelBase x) => x.NotifyProperty, () => counter++);

            // modify
            model.NotifyProperty = 1;
            Assert.AreEqual(counter, 1);

            // modify
            model.NotifyProperty += 2;
            Assert.AreEqual(counter, 2);

            // not modify
            model.NotifyProperty = 3;
            Assert.AreEqual(counter, 2);

            // modify other
            model.OtherNotifyProperty = 100;
            Assert.AreEqual(counter, 2);

            Assert.AreEqual(model.NotifyProperty, 3);
            Assert.AreEqual(model.OtherNotifyProperty, 100);
        }

        [Test]
        public void NotNotifyPropertyTest()
        {
            var model = Notifier.Of<TestModelBase>();

            var counter = 0;
            SetChangedAction(model, (TestModelBase x) => x.NotNotifyProperty, () => counter++);

            model.NotifyProperty = 1;

            Assert.AreEqual(counter, 0);
        }

        [Test]
        public void VirtualNotNotifyPropertyTest()
        {
            var model = Notifier.Of<TestModelBase>();

            var counter = 0;
            SetChangedAction(model, (TestModelBase x) => x.VirtualNotNotifyProperty, () => counter++);

            model.VirtualNotNotifyProperty = 1;

            Assert.AreEqual(counter, 0);
        }

        [Test]
        public void FailedModelTest()
        {
            Assert.Throws(typeof(InvalidOperationException), () => Notifier.Of<NotVirtualModel>());
            Assert.Throws(typeof(InvalidOperationException), () => Notifier.Of<InternalGetModel>());
            Assert.Throws(typeof(InvalidOperationException), () => Notifier.Of<InternalSetModel>());
            Assert.Throws(typeof(InvalidOperationException), () => Notifier.Of<PrivateGetModel>());
            Assert.Throws(typeof(InvalidOperationException), () => Notifier.Of<PrivateSetModel>());

            Assert.DoesNotThrow(() => Notifier.Of<NotPublicModel>());
        }

        [Test]
        public void FailedInvocatorTest()
        {
            Assert.Throws(typeof(InvalidOperationException), () => Notifier.Of<ModelWithoutInvocator>());
            Assert.Throws(typeof(InvalidOperationException), () => Notifier.Of<ModelWithInvalideInvocator>());
            Assert.Throws(typeof(InvalidOperationException), () => Notifier.Of<ModelWithInvalideInvocator2>());
            Assert.Throws(typeof(InvalidOperationException), () => Notifier.Of<ModelWithNotPublicInvocator>());
            Assert.Throws(typeof(InvalidOperationException), () => Notifier.Of<ModelWithMultyInvocator>());
            Assert.Throws(typeof(ArgumentException), () => Notifier.Of(typeof(ModelWithAbstractInvocator)));

            Assert.DoesNotThrow(() => Notifier.Of<ModelWithCorrectInvocator>());
        }

        [Test]
        public void FailedTypeTest()
        {
            Assert.Throws(typeof(ArgumentNullException), () => Notifier.Of(null));
            Assert.Throws(typeof(ArgumentException), () => Notifier.Of(typeof(object)));
        }

        [Test]
        public void MultyNoifyPropertyTest()
        {
            var model = Notifier.Of<TestModelBase>();

            var doubleCounter = 0;
            var otherCounter = 0;
            SetChangedAction(model, (TestModelBase x) => x.MultyNotifyProperty, () => doubleCounter++);
            SetChangedAction(model, (TestModelBase x) => x.OtherNotifyProperty, () => otherCounter++);

            model.MultyNotifyProperty = 1;

            Assert.AreEqual(doubleCounter, 1);
            Assert.AreEqual(otherCounter, 1);
        }

        [Test]
        public void PrecisionDoubleNoifyPropertyTest()
        {
            var model = Notifier.Of<TestModelBase>();

            var counter = 0;
            SetChangedAction(model, (TestModelBase x) => x.DoubleNotifyProperty, () => counter++);

            model.DoubleNotifyProperty = 1;
            Assert.AreEqual(counter, 1);

            // very small number
            model.DoubleNotifyProperty += 1e-17;
            Assert.AreEqual(counter, 1);

            model.DoubleNotifyProperty += 1e-14;
            Assert.AreEqual(counter, 2);
        }

        [Test]
        public void PrecisionFloatNoifyPropertyTest()
        {
            var model = Notifier.Of<TestModelBase>();

            var counter = 0;
            SetChangedAction(model, (TestModelBase x) => x.FloatNotifyProperty, () => counter++);

            model.FloatNotifyProperty = 1;
            Assert.AreEqual(counter, 1);

            // very small number
            model.FloatNotifyProperty += 1e-9f;
            Assert.AreEqual(counter, 1);

            model.FloatNotifyProperty += 1e-7f;
            Assert.AreEqual(counter, 2);
        }

        [Test]
        public void VirtualNotNotifyPropertyInDerivedClassTest()
        {
            var model = Notifier.Of<TestModel>();
            var counter = 0;
            SetChangedAction(model, (TestModel x) => x.VirtualNotNotifyProperty, () => counter++);

            model.VirtualNotNotifyProperty = 1;
            Assert.AreEqual(counter, 0);
        }

        [Test]
        public void StringNotifyProperty()
        {
            var model = Notifier.Of<TestModel>();

            var counter = 0;
            var comboCounter = 0;
            SetChangedAction(model, (TestModel x) => x.StringNotifyProperty, () => counter++);
            SetChangedAction(model, (TestModel x) => x.ComboProperty, () => comboCounter++);

            model.StringNotifyProperty = "Test";
            Assert.AreEqual(counter, 1);
            Assert.AreEqual(comboCounter, 1);
        }

        [Test]
        public void RefletionTest()
        {
            var model = Notifier.Of<TestModelBase>();

            var counter = 0;
            SetChangedAction(model, (TestModel x) => x.NotifyProperty, () => counter++);

            Assert.DoesNotThrow(() =>
                                    {
                                        var prop = model.GetType().GetProperty("NotifyProperty");
                                        prop.SetValue(model, 1, null);
                                    });

            Assert.AreEqual(counter, 1);
        }

        static void SetChangedAction<T, TValue>(INotifyPropertyChanged model, Expression<Func<T, TValue>> property, Action onChanged)
        {
            var name = ((MemberExpression)property.Body).Member.Name;

            model.PropertyChanged += (o, e) =>
                                         {
                                             if(e.PropertyName == name)
                                                 onChanged();
                                         };
        }
    }
}