using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericSettings.Helpers
{
	class FileDialogSetting : ISettingHandler<string>
	{
		public FileDialogSetting(Control textBox, Control browseButton, FileDialogSettingType type = FileDialogSettingType.Open)
		{
			this.textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
			this.type = type;

			if (browseButton != null)
			{
				browseButton.Click += this.HandleBrowseButtonClick;
			}
		}

		private readonly Control textBox;
		private readonly FileDialogSettingType type;

		public string Filter { get; set; }

		public event EventHandler SomethingChanged
		{
			add { this.textBox.TextChanged += value; }
			remove { this.textBox.TextChanged -= value; }
		}

		private void HandleBrowseButtonClick(object sender, EventArgs e)
		{
			switch (this.type)
			{
				case FileDialogSettingType.Open:
					using (var dialog = new OpenFileDialog())
					{
						dialog.Filter = this.Filter;
						dialog.FileName = this.textBox.Text;
						if (dialog.ShowDialog() == DialogResult.OK)
						{
							this.textBox.Text = dialog.FileName;
						}
					}
					break;

				case FileDialogSettingType.Save:
					using (var dialog = new SaveFileDialog())
					{
						dialog.Filter = this.Filter;
						dialog.FileName = this.textBox.Text;
						if (dialog.ShowDialog() == DialogResult.OK)
						{
							this.textBox.Text = dialog.FileName;
						}
					}
					break;

				case FileDialogSettingType.FolderBrowserDialog:
					using (var dialog = new FolderBrowserDialog())
					{
						dialog.SelectedPath = this.textBox.Text;
						if (dialog.ShowDialog() == DialogResult.OK)
						{
							this.textBox.Text = dialog.SelectedPath;
						}
					}
					break;
			}
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

	enum FileDialogSettingType
	{
		Open,
		Save,
		FolderBrowserDialog
	}
}
