using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericSettings.Helpers
{
	/// <summary>
	/// Optional helper class to simplify allocation.
	/// </summary>
	class SettingsUIHelper<TSettings>
	{
		public List<ISetting<TSettings>> Settings { get; } = new List<ISetting<TSettings>>();

		public THandler Add<THandler, TValue>(THandler handler, Func<TSettings, TValue> getter, Action<TSettings, TValue> setter)
			where THandler : ISettingHandler<TValue>
		{
			this.Settings.Add(new GenericSetting<TSettings, TValue>(handler, getter, setter));
			return handler;
		}

		public AddHelper<TSettings, TValue> Add<TValue>(Func<TSettings, TValue> getter, Action<TSettings, TValue> setter)
		{
			return new AddHelper<TSettings, TValue>(this.Settings, getter, setter);
		}
	}


	struct AddHelper<TSettings, TValue>
	{
		public AddHelper(List<ISetting<TSettings>> settings, Func<TSettings, TValue> getter, Action<TSettings, TValue> setter)
		{
			this.settings = settings;
			this.getter = getter;
			this.setter = setter;
		}

		private readonly List<ISetting<TSettings>> settings;
		private readonly Func<TSettings, TValue> getter;
		private readonly Action<TSettings, TValue> setter;

		public THandler Handler<THandler>(THandler handler)
			where THandler : ISettingHandler<TValue>
		{
			var setting = new GenericSetting<TSettings, TValue>(handler, this.getter, this.setter);
			this.settings.Add(setting);
			return handler;
		}
	}


	static class AddHelper
	{
		public static ParsingTextBoxSetting<int> ParsingTextBox<TSettings>(this AddHelper<TSettings, int> helper, Control textBox, string displayFormat = null, Func<int, bool> check = null)
		{
			return helper.Handler(ParsingTextBoxSetting.Int(textBox, displayFormat: displayFormat, check: check));
		}

		public static ParsingTextBoxSetting<double> ParsingTextBox<TSettings>(this AddHelper<TSettings, double> helper, Control textBox, string displayFormat = null, Func<double, bool> check = null)
		{
			return helper.Handler(ParsingTextBoxSetting.Double(textBox, displayFormat: displayFormat, check: check));
		}

		public static CheckBoxSetting CheckBox<TSettings>(this AddHelper<TSettings, bool> helper, CheckBox checkBox)
		{
			return helper.Handler(new CheckBoxSetting(checkBox));
		}
	}
}
