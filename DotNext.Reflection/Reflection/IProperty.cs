﻿using System.Reflection;

namespace DotNext.Reflection
{
	/// <summary>
	/// Represents reflected property.
	/// </summary>
	public interface IProperty: IMember<PropertyInfo>
	{
		bool CanRead { get; }
		bool CanWrite { get; }
	}

	/// <summary>
	/// Represents static property.
	/// </summary>
	/// <typeparam name="P">Type of property value.</typeparam>
	public interface IProperty<P>: IProperty
	{
		/// <summary>
		/// Gets or sets property value.
		/// </summary>
		P Value{ get; set; }
	}

	/// <summary>
	/// Represents instance property.
	/// </summary>
	/// <typeparam name="T">Property declaring type.</typeparam>
	/// <typeparam name="P">Type of property value.</typeparam>
	public interface IProperty<T, P>: IProperty
	{
		/// <summary>
		/// Gets or sets property value.
		/// </summary>
		/// <param name="instance">The object on which to invoke the property getter or setter.</param>
		/// <returns>Property value.</returns>
		P this[in T instance]{ get; set; }
	}
}
