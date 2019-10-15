using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericSettings.Helpers
{
	class ColorSetting : ISettingHandler<Color>
	{
		public ColorSetting(Control colorPickerButton)
		{
			this.colorPickerButton = colorPickerButton ?? throw new ArgumentNullException(nameof(colorPickerButton));
			this.colorPickerButton.Click += this.HandleButtonClick;
		}

		private readonly Control colorPickerButton;

		public event EventHandler SomethingChanged;

		private Color currentColor;

		public void Load(Color value)
		{
			this.currentColor = value;
			this.UpdateCurrentColor();
		}

		public bool TryGet(out Color value)
		{
			value = this.currentColor;
			return true;
		}

		private void UpdateCurrentColor()
		{
			var c = this.currentColor;
			this.colorPickerButton.BackColor = c;
			bool isColorPrettyBright = c.R * 0.4 + c.G + c.B * 0.2 > 190;
			this.colorPickerButton.ForeColor = isColorPrettyBright ? Color.Black : Color.White;
		}

		private void HandleButtonClick(object sender, EventArgs e)
		{
			using (var dialog = new ColorDialog())
			{
				dialog.Color = this.currentColor;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					this.currentColor = dialog.Color;
					this.UpdateCurrentColor();
					this.SomethingChanged?.Invoke(this, e);
				}
			}
		}
	}
}
