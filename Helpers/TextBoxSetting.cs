using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericSettings.Helpers
{
	class TextBoxSetting : ISettingHandler<string>
	{
		public TextBoxSetting(Control textBox)
		{
			this.textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
		}

		private readonly Control textBox;

		public event EventHandler SomethingChanged
		{
			add { this.textBox.TextChanged += value; }
			remove { this.textBox.TextChanged -= value; }
		}

		public void Load(string value)
		{
			this.textBox.Text = value;
		}

		public bool TryGet(out string value)
		{
			value = this.textBox.Text;
			return true;
		}
	}
}
