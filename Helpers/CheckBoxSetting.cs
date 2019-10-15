using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericSettings.Helpers
{
	class CheckBoxSetting : ISettingHandler<bool>
	{
		public CheckBoxSetting(CheckBox checkBox)
		{
			this.checkBox = checkBox ?? throw new ArgumentNullException(nameof(checkBox));
			checkBox.CheckedChanged += (sender, e) => this.SomethingChanged?.Invoke(this, e);
		}

		private readonly CheckBox checkBox;

		public event EventHandler SomethingChanged;

		public void Load(bool value)
		{
			this.checkBox.Checked = value;
		}

		public bool TryGet(out bool value)
		{
			value = this.checkBox.Checked;
			return true;
		}
	}
}
