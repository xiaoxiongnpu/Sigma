﻿/* 
MIT License

Copyright (c) 2016 Florian Cäsar, Michael Plainer

For full license see LICENSE in the root directory of this project. 
*/

using log4net;
using Sigma.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma.Core.Data
{
	/// <summary>
	/// A data type that can be used for data buffers and mathematical operations. Data types are used to define buffer types to use at runtime.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IDataType
	{
		/// <summary>
		/// The underlying system type of this data type. 
		/// </summary>
		System.Type UnderlyingType { get; }

		/// <summary>
		/// The size of this type in bytes.
		/// </summary>
		int SizeBytes { get; }

		/// <summary>
		/// Creates a generic array with a given length and the underlying type of this data type as type. 
		/// </summary>
		/// <param name="length">The length of the array to create.</param>
		/// <returns>An array with the underlying type of this data type as type and the given length.</returns>
		Array CreateArray(int length);
	}

	/// <summary>
	/// A collection of common data type constants and a common type registry, used by the internal data handling implementations.
	/// </summary>
	public class DataTypes
	{
		private static Dictionary<System.Type, IDataType> registeredTypes = new Dictionary<Type, IDataType>();
		private static ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static bool AllowExternalTypeOverwrites { get; set; } = false;

		public static readonly IDataType FLOAT32 = Register(typeof(float), new DataType<float>(4));
		public static readonly IDataType FLOAT64 = Register(typeof(double), new DataType<double>(8));

		public static readonly IDataType INT8 = Register(typeof(byte), new DataType<byte>(1));
		public static readonly IDataType INT16 = Register(typeof(short), new DataType<short>(2));
		public static readonly IDataType INT32 = Register(typeof(int), new DataType<int>(4));
		public static readonly IDataType INT64 = Register(typeof(long), new DataType<long>(8));

		/// <summary>
		/// Register a system data type with a Sigma data type interface to be automatically inferred whenever the system type is used. 
		/// You can change already registered types by setting AllowExternalTypeOverwrites to true and registering your own, BUT this may render existing models and environments incompatible. Use with caution.
		/// </summary>
		/// <param name="underlyingType">The underlying system type to map.</param>
		/// <param name="type">The mapped data type interface.</param>
		/// <returns>The registered data type interface (for convenience).</returns>
		public static IDataType Register(System.Type underlyingType, IDataType type)
		{
			if (!registeredTypes.ContainsKey(underlyingType))
			{
				registeredTypes.Add(underlyingType, type);
			}
			else
			{
				if (AllowExternalTypeOverwrites)
				{
					logger.Info($"Overwrote internal system type {underlyingType} to now refer to {type} (this may not be what you wanted).");
				}
				else
				{
					throw new ArgumentException($"System type {underlyingType} is already registered as {registeredTypes[underlyingType]} and cannot be changed to {type} (AllowExternalTypeOverwrites flag is set to false).");
				}
			}

			return type;
		}

		/// <summary>
		/// Get the registered data type interface for a certain system type. Typically used when data type interface was not explicitly given.
		/// </summary>
		/// <param name="underlyingType">The system type the data type interface should be registered for.</param>
		/// <returns>The data type interface that matches the underlying system type.</returns>
		public static IDataType GetMatchingType(System.Type underlyingType)
		{
			if (!registeredTypes.ContainsKey(underlyingType))
			{
				throw new ArgumentException($"There is no data type interface mapping for {underlyingType} in this registry (are you missing a cast?).");
			}

			return registeredTypes[underlyingType];
		}
	}

	public class DataType<T> : IDataType
	{
		public int SizeBytes
		{
			get; private set;
		}

		public Type UnderlyingType
		{
			get;
		} = typeof(T);

		public DataType(int sizeBytes)
		{
			this.SizeBytes = sizeBytes;
		}

		public Array CreateArray(int length)
		{
			return new T[length];
		}
	}
}