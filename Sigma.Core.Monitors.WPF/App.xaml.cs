﻿/* 
MIT License

Copyright (c) 2016 Florian Cäsar, Michael Plainer

For full license see LICENSE in the root directory of this project. 
*/

using System;
using System.Diagnostics;
using System.Windows;
using MahApps.Metro;

namespace Sigma.Core.Monitors.WPF
{

	/// <summary>
	/// This file contains the general interaction logic for App.xaml.
	/// See App.xaml for why these files are required. 
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		/// The corresponding WPF monitor.
		/// </summary>
		protected WPFMonitor monitor;

		public App(WPFMonitor monitor)
		{
			this.monitor = monitor;

			InitializeComponent();
		}
	}
}