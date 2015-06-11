using System;
using CR_Documentor.Diagnostics;
using DevExpress.CodeRush.Core;

namespace CR_Documentor.Options
{
	/// <summary>
	/// Extension methods for <see cref="DevExpress.CodeRush.Core.DecoupledStorage"/>.
	/// </summary>
	public static class DecoupledStorageExtensions
	{
		/// <summary>
		/// Safely reads a <see cref="System.Boolean"/> from decoupled storage, logging any errors that arise.
		/// </summary>
		/// <param name="storage">The <see cref="DevExpress.CodeRush.Core.DecoupledStorage"/> containing the value.</param>
		/// <param name="section">The configuration section in which the value is stored.</param>
		/// <param name="key">The key in the configuration section in which the value is stored.</param>
		/// <param name="defaultValue">The value to return in the event no stored value is found or an error occurs.</param>
		/// <param name="log">The logger to which error messages should be written if encountered.</param>
		/// <returns>
		/// The value read from configuration; or <paramref name="defaultValue" />
		/// if there is no value in configuration or if any error occurs.
		/// </returns>
		public static bool SafeReadBoolean(this DecoupledStorage storage, string section, string key, bool defaultValue, ILog log)
		{
			try
			{
				return storage.ReadBoolean(section, key, defaultValue);
			}
			catch (Exception ex)
			{
				log.Write(LogLevel.Error, String.Format("Unable to read {0}/{1} from options. Defaulting to {2}.", section, key, defaultValue), ex);
				return defaultValue;
			}
		}

		/// <summary>
		/// Safely reads an enumeration value from decoupled storage, logging any errors that arise.
		/// </summary>
		/// <typeparam name="T">The enumeration type to read.</typeparam>
		/// <param name="storage">The <see cref="DevExpress.CodeRush.Core.DecoupledStorage"/> containing the value.</param>
		/// <param name="section">The configuration section in which the value is stored.</param>
		/// <param name="key">The key in the configuration section in which the value is stored.</param>
		/// <param name="defaultValue">The value to return in the event no stored value is found or an error occurs.</param>
		/// <param name="log">The logger to which error messages should be written if encountered.</param>
		/// <returns>
		/// The value read from configuration; or <paramref name="defaultValue" />
		/// if there is no value in configuration or if any error occurs.
		/// </returns>
		public static T SafeReadEnum<T>(this DecoupledStorage storage, string section, string key, T defaultValue, ILog log)
		{
			try
			{
				return storage.ReadEnum<T>(section, key, defaultValue);
			}
			catch (Exception ex)
			{
				log.Write(LogLevel.Error, String.Format("Unable to read {0}/{1} from options. Defaulting to {2}.", section, key, defaultValue), ex);
				return defaultValue;
			}
		}

		/// <summary>
		/// Safely reads an <see cref="System.Int32"/> from decoupled storage, logging any errors that arise.
		/// </summary>
		/// <param name="storage">The <see cref="DevExpress.CodeRush.Core.DecoupledStorage"/> containing the value.</param>
		/// <param name="section">The configuration section in which the value is stored.</param>
		/// <param name="key">The key in the configuration section in which the value is stored.</param>
		/// <param name="defaultValue">The value to return in the event no stored value is found or an error occurs.</param>
		/// <param name="log">The logger to which error messages should be written if encountered.</param>
		/// <returns>
		/// The value read from configuration; or <paramref name="defaultValue" />
		/// if there is no value in configuration or if any error occurs.
		/// </returns>
		public static Int32 SafeReadInt32(this DecoupledStorage storage, string section, string key, Int32 defaultValue, ILog log)
		{
			try
			{
				return storage.ReadInt32(section, key, defaultValue);
			}
			catch (Exception ex)
			{
				log.Write(LogLevel.Error, String.Format("Unable to read {0}/{1} from options. Defaulting to {2}.", section, key, defaultValue), ex);
				return defaultValue;
			}
		}

		/// <summary>
		/// Safely reads a <see cref="System.String"/> from decoupled storage, logging any errors that arise.
		/// </summary>
		/// <param name="storage">The <see cref="DevExpress.CodeRush.Core.DecoupledStorage"/> containing the value.</param>
		/// <param name="section">The configuration section in which the value is stored.</param>
		/// <param name="key">The key in the configuration section in which the value is stored.</param>
		/// <param name="defaultValue">The value to return in the event no stored value is found or an error occurs.</param>
		/// <param name="log">The logger to which error messages should be written if encountered.</param>
		/// <returns>
		/// The value read from configuration; or <paramref name="defaultValue" />
		/// if there is no value in configuration or if any error occurs.
		/// </returns>
		public static string SafeReadString(this DecoupledStorage storage, string section, string key, string defaultValue, ILog log)
		{
			try
			{
				return storage.ReadString(section, key, defaultValue);
			}
			catch (Exception ex)
			{
				log.Write(LogLevel.Error, String.Format("Unable to read {0}/{1} from options. Defaulting to {2}.", section, key, defaultValue), ex);
				return defaultValue;
			}
		}
	}
}
