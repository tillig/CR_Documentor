using System;

namespace CR_Documentor.Diagnostics
{
	/// <summary>
	/// Indicates a logical activity under which log messages might be made.
	/// </summary>
	public class ActivityContext : IDisposable
	{
		/// <summary>
		/// The logger that gets entered/exited for this activity context.
		/// </summary>
		private ILog _logger = null;

		/// <summary>
		/// Starts a new logging activity context.
		/// </summary>
		/// <param name="activityName">
		/// The name of the logical activity taking place.
		/// </param>
		/// <param name="logger">
		/// The logger to write the activity information to.
		/// </param>
		/// <remarks>
		/// <para>
		/// The activity name will appear in a log message and then the log
		/// will be indented so new messages will appear "within" the logical
		/// activity context. When the object is disposed, the log will be
		/// outdented again.
		/// </para>
		/// </remarks>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="logger" /> or <paramref name="activityName" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// Thrown if <paramref name="activityName" /> is <see cref="System.String.Empty" />.
		/// </exception>
		public ActivityContext(ILog logger, string activityName)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			if (activityName == null)
			{
				throw new ArgumentNullException("activityName");
			}
			if (activityName.Length == 0)
			{
				throw new ArgumentException("Activity name may not be empty.", "activityName");
			}

			this._logger = logger;
			this._logger.Enter(activityName);
		}

		/// <summary>
		/// Flag indicating whether managed resources have already been disposed.
		/// </summary>
		private bool disposed = false;

		/// <summary>
		/// Performs application-defined tasks associated with freeing,
		/// releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources;
		/// <see langword="false"/> to release only unmanaged resources.
		/// </param>
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this._logger.Exit();
				}

				this.disposed = true;
			}
		}
	}
}
