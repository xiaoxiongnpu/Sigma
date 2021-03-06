﻿/* 
MIT License

Copyright (c) 2016-2017 Florian Cäsar, Michael Plainer

For full license see LICENSE in the root directory of this project. 
*/

using System.Threading;
using System.Windows.Controls;
using NUnit.Framework;
using Sigma.Core.Monitors.WPF.View;
// ReSharper disable InconsistentNaming

namespace Sigma.Core.Monitors.WPF.Tests.View
{
	public class TestUIWrapper
	{
		[TestCase]
		public void TestUIWrapperCreation()
		{
			Thread fred = new Thread(() =>
			{
				TestUIWrapperClass wrapper = new TestUIWrapperClass();

				Assert.IsTrue(wrapper.WrappedContent != null);

				wrapper.WrappedContent.Test = "hello";

				Assert.AreEqual(wrapper.WrappedContent.Test, "hello");

				wrapper.WrappedContent = new TestControl() { Test = "world" };

				Assert.AreNotEqual(wrapper.WrappedContent.Test, "hello");
				Assert.AreEqual(wrapper.WrappedContent.Test, "world");
			});

			fred.SetApartmentState(ApartmentState.STA);
			fred.Priority = ThreadPriority.Highest;
			fred.Start();

			fred.Join();
		}

		private class TestControl : ContentControl
		{
			public string Test { get; set; }
		}

		private class TestUIWrapperClass : UIWrapper<TestControl>
		{

		}
	}
}