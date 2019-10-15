using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericSettings.Helpers
{
	interface IParseFormat<T>
	{
		bool TryParse(string str, IFormatProvider formatProvider, out T value);
		string Format(T value, string format, IFormatProvider formatProvider);
	}

	static class ParseFormat
	{
		public static IParseFormat<int> Int { get; } = new ParseFormat_Int();
		public static IParseFormat<double> Double { get; } = new ParseFormat_Double();
	}

	class ParseFormat_Double : IParseFormat<double>
	{
		public string Format(double value, string format, IFormatProvider formatProvider)
		{
			return value.ToString(format, formatProvider);
		}

		public bool TryParse(string str, IFormatProvider formatProvider, out double value)
		{
			return double.TryParse(str, NumberStyles.Float, formatProvider, out value);
		}
	}

	class ParseFormat_Int : IParseFormat<int>
	{
		public string Format(int value, string format, IFormatProvider formatProvider)
		{
			return value.ToString(format, formatProvider);
		}

		public bool TryParse(string str, IFormatProvider formatProvider, out int value)
		{
			return int.TryParse(str, NumberStyles.Integer, formatProvider, out value);
		}
	}
}
