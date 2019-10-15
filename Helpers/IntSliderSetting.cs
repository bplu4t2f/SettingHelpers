using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericSettings.Helpers
{
	class IntSliderSetting : ISettingHandler<int>
	{
		public IntSliderSetting(TrackBar trackBar, Control label)
		{
			this.trackBar = trackBar ?? throw new ArgumentNullException(nameof(trackBar));
			this.label = label;
			this.trackBar.ValueChanged += this.HandleTrackBarValueChanged;
		}

		private readonly TrackBar trackBar;
		private readonly Control label;
		/// <summary>
		/// This exists because the <see cref="TrackBar.Value"/> is confined to <see cref="TrackBar.Minimum"/> .. <see cref="TrackBar.Maximum"/>.
		/// </summary>
		private int realValue;
		private bool updating = false;

		public event EventHandler SomethingChanged
		{
			add { this.trackBar.ValueChanged += value; }
			remove { this.trackBar.ValueChanged -= value; }
		}

		public void Load(int value)
		{
			this.updating = true;
			this.realValue = value;
			int trackBarValue = Math.Min(Math.Max(value, this.trackBar.Minimum), this.trackBar.Maximum);
			this.trackBar.Value = trackBarValue;
			this.updating = false;
			this.UpdateText();
		}

		public bool TryGet(out int value)
		{
			value = this.realValue;
			return true;
		}

		private void UpdateText()
		{
			this.label.Text = this.realValue.ToString(CultureInfo.InvariantCulture);
		}

		private void HandleTrackBarValueChanged(object sender, EventArgs e)
		{
			if (!this.updating)
			{
				this.realValue = this.trackBar.Value;
				this.UpdateText();
			}
		}
	}
}
