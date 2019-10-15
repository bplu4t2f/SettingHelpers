using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericSettings.Helpers
{
	class RadioButtonSetting<T> : ISettingHandler<T>
	{
		private readonly Dictionary<RadioButton, T> values = new Dictionary<RadioButton, T>();

		public void AddValue(RadioButton radioButton, T value)
		{
			if (radioButton == null) throw new ArgumentNullException(nameof(radioButton));
			this.values.Add(radioButton, value);
			radioButton.CheckedChanged += this.HandleRadioButtonCheckedChanged;
		}

		private void HandleRadioButtonCheckedChanged(object sender, EventArgs e)
		{
			this.OnSomethingChanged();
		}

		public event EventHandler SomethingChanged;

		protected virtual void OnSomethingChanged()
		{
			this.SomethingChanged?.Invoke(this, EventArgs.Empty);
		}

		public void Load(T value)
		{
			foreach (var kv in this.values)
			{
				kv.Key.Checked = object.Equals(value, kv.Value);
			}
		}

		public bool TryGet(out T value)
		{
			foreach (var kv in this.values)
			{
				if (kv.Key.Checked)
				{
					value = kv.Value;
					return true;
				}
			}
			value = default(T);
			return false;
		}
	}
}
