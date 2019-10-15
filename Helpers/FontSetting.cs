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
	class FontSetting : ISettingHandler<Font>
	{
		public FontSetting(Control changeButton, Control label, bool applyFontToLabel = true)
		{
			this.changeButton = changeButton ?? throw new ArgumentNullException(nameof(changeButton));
			this.label = label ?? throw new ArgumentNullException(nameof(label));
			this.applyFontToLabel = applyFontToLabel;
			this.changeButton.Click += this.HandleButtonClick;
		}

		private readonly Control changeButton;
		private readonly Control label;
		private readonly bool applyFontToLabel;

		public event EventHandler SomethingChanged;

		private Font currentFont;

		public void Load(Font value)
		{
			this.currentFont = value;
			this.UpdateCurrentFont();
		}

		public bool TryGet(out Font value)
		{
			value = this.currentFont;
			return true;
		}

		private void UpdateCurrentFont()
		{
			var f = this.currentFont;
			string text = f == null ? String.Empty : String.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2} pt", f.FontFamily.Name, f.Style, f.Size);
			this.label.Text = text;
			if (this.applyFontToLabel)
			{
				this.label.Font = f;
			}
		}

		private void HandleButtonClick(object sender, EventArgs e)
		{
			using (var dialog = new FontDialog())
			{
				dialog.Font = this.currentFont;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					this.currentFont = dialog.Font;
					this.UpdateCurrentFont();
					this.SomethingChanged?.Invoke(this, e);
				}
			}
		}
	}
}
