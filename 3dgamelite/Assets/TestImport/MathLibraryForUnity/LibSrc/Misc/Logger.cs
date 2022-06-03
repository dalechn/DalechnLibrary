using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public interface ILogger
		{
			void LogInfo(object value);
			void LogWarning(object value);
			void LogError(object value);
		}

		public class Logger
		{
			private static ILogger _instance;

			static Logger()
			{
				_instance = new DefaultLogger();
			}

			public static void LogInfo(object value)
			{
				_instance.LogInfo(value);
			}

			public static void LogWarning(object value)
			{
				_instance.LogWarning(value);
			}

			public static void LogError(object value)
			{
				_instance.LogError(value);
			}

			public static void SetLogger(ILogger logger)
			{
				_instance = logger != null ? logger : new EmptyLogger();
			}
		}

		public class DefaultLogger : ILogger
		{
			public void LogInfo(object value)
			{
				Debug.Log(value);
			}

			public void LogWarning(object value)
			{
				Debug.LogWarning(value);
			}

			public void LogError(object value)
			{
				Debug.LogError(value);
			}
		}

		public class EmptyLogger : ILogger
		{
			public void LogInfo(object value)
			{
			}

			public void LogWarning(object value)
			{
			}

			public void LogError(object value)
			{
			}
		}
	}
}
