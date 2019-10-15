using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenericSettings.Helpers
{
	interface ISetting<TSettings>
	{
		void LoadFrom(TSettings source);
		bool TrySaveTo(TSettings target);
		event EventHandler SomethingChanged;

		bool HasChanged { get; }
		/// <summary>
		/// Called on cancel -- resets this to its last loaded value.
		/// </summary>
		void Revert();
	}


	class GenericSetting<TSettings, TValue> : ISetting<TSettings>
	{
		public GenericSetting(ISettingHandler<TValue> handler, Func<TSettings, TValue> getter, Action<TSettings, TValue> setter)
		{
			this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
			this.getter = getter ?? throw new ArgumentNullException(nameof(getter));
			this.setter = setter ?? throw new ArgumentNullException(nameof(setter));
			this.handler.SomethingChanged += this.HandleSomethingChanged;
		}
		private readonly ISettingHandler<TValue> handler;
		private readonly Func<TSettings, TValue> getter;
		private readonly Action<TSettings, TValue> setter;

		public bool HasChanged { get; private set; }

		public event EventHandler SomethingChanged
		{
			add { this.handler.SomethingChanged += value; }
			remove { this.handler.SomethingChanged -= value; }
		}

		private TValue lastLoadedValue;

		public void LoadFrom(TSettings source)
		{
			this.lastLoadedValue = this.getter(source);
			this.Revert();
		}

		public bool TrySaveTo(TSettings destination)
		{
			if (!this.handler.TryGet(out TValue value))
			{
				return false;
			}
			this.setter(destination, value);
			return true;
		}

		public void Revert()
		{
			this.handler.Load(this.lastLoadedValue);
			this.HasChanged = false;
		}

		private void HandleSomethingChanged(object sender, EventArgs e)
		{
			this.HasChanged = true;
		}
	}


	static class GenericSetting
	{
		public static Func<CultureInfo> CultureSource { get; set; }
		public static CultureInfo GetEffectiveCulture() => CultureSource?.Invoke() ?? CultureInfo.InvariantCulture;

		public static Color ErrorBackColor { get; set; } = Color.Firebrick;
		public static Color ErrorForeColor { get; set; } = Color.White;
	}


	interface ISettingHandler<TValue>
	{
		void Load(TValue value);
		bool TryGet(out TValue value);
		event EventHandler SomethingChanged;
	}
}
