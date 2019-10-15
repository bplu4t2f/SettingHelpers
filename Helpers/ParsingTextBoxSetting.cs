using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericSettings.Helpers
{
	class ParsingTextBoxSetting<T> : ISettingHandler<T>
	{
		public ParsingTextBoxSetting(Control textBox, IParseFormat<T> parseFormat, string displayFormat = null, Func<T, bool> check = null)
		{
			this.textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
			this.parseFormat = parseFormat ?? throw new ArgumentNullException(nameof(parseFormat));
			this.displayFormat = displayFormat;
			this.check = check;
			this.textBox.TextChanged += this.HandleTextChanged;
		}

		private readonly Control textBox;
		private readonly IParseFormat<T> parseFormat;
		private readonly string displayFormat;
		private readonly Func<T, bool> check;

		public event EventHandler SomethingChanged
		{
			add { this.textBox.TextChanged += value; }
			remove { this.textBox.TextChanged -= value; }
		}

		private void HandleTextChanged(object sender, EventArgs e)
		{
			this.TryGet(out _);
		}

		public void Load(T value)
		{
			this.textBox.Text = this.parseFormat.Format(value, this.displayFormat, GenericSetting.GetEffectiveCulture());
		}
		
		private bool TryGetInternal(out T value)
		{
			if (!this.parseFormat.TryParse(this.textBox.Text, GenericSetting.GetEffectiveCulture(), out value))
			{
				return false;
			}
			if (this.check != null)
			{
				if (!this.check(value))
				{
					return false;
				}
			}
			return true;
		}

		public bool TryGet(out T value)
		{
			if (!this.TryGetInternal(out value))
			{
				this.textBox.BackColor = GenericSetting.ErrorBackColor;
				this.textBox.ForeColor = GenericSetting.ErrorForeColor;
				return false;
			}
			else
			{
				this.textBox.BackColor = Color.Empty;
				this.textBox.ForeColor = Color.Empty;
				return true;
			}
		}
	}


	static class ParsingTextBoxSetting
	{
		public static ParsingTextBoxSetting<int> Int(Control textBox, string displayFormat = null, Func<int, bool> check = null)
		{
			return new ParsingTextBoxSetting<int>(textBox, ParseFormat.Int, displayFormat: displayFormat, check: check);
		}

		public static ParsingTextBoxSetting<double> Double(Control textBox, string displayFormat = null, Func<double, bool> check = null)
		{
			return new ParsingTextBoxSetting<double>(textBox, ParseFormat.Double, displayFormat: displayFormat, check: check);
		}
	}
}
