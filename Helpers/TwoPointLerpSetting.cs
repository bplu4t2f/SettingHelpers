using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericSettings.Helpers
{
	class TwoPointLerpSetting : ISettingHandler<Lerp>
	{
		public TwoPointLerpSetting(Control slope, Control intercept, Control x1, Control y1, Control x2, Control y2, double initial_x1 = 0.0, double initial_x2 = 1.0, string displayFormat = null)
		{
			this.textBox_slope = slope;
			this.textBox_intercept = intercept;
			this.textBox_x1 = x1 ?? throw new ArgumentNullException(nameof(x1));
			this.textBox_y1 = y1 ?? throw new ArgumentNullException(nameof(y1));
			this.textBox_x2 = x2 ?? throw new ArgumentNullException(nameof(x2));
			this.textBox_y2 = y2 ?? throw new ArgumentNullException(nameof(y2));
			this.initial_x1 = initial_x1;
			this.initial_x2 = initial_x2;
			if (slope != null)
			{
				if (slope is TextBox slopeTextBox)
				{
					slopeTextBox.ReadOnly = true;
				}
				this.slope = ParsingTextBoxSetting.Double(slope, displayFormat);
			}
			if (intercept != null)
			{
				if (intercept is TextBox interceptTextBox)
				{
					interceptTextBox.ReadOnly = true;
				}
				this.intercept = ParsingTextBoxSetting.Double(intercept, displayFormat);
			}
			this.x1 = ParsingTextBoxSetting.Double(x1, displayFormat);
			this.y1 = ParsingTextBoxSetting.Double(y1, displayFormat);
			this.x2 = ParsingTextBoxSetting.Double(x2, displayFormat);
			this.y2 = ParsingTextBoxSetting.Double(y2, displayFormat);
			this.SomethingChanged += this.HandleSomethingChanged;
		}

		private readonly Control textBox_slope;
		private readonly Control textBox_intercept;
		private readonly Control textBox_x1;
		private readonly Control textBox_y1;
		private readonly Control textBox_x2;
		private readonly Control textBox_y2;
		private readonly double initial_x1;
		private readonly double initial_x2;

		private readonly ParsingTextBoxSetting<double> slope;
		private readonly ParsingTextBoxSetting<double> intercept;
		private readonly ParsingTextBoxSetting<double> x1;
		private readonly ParsingTextBoxSetting<double> y1;
		private readonly ParsingTextBoxSetting<double> x2;
		private readonly ParsingTextBoxSetting<double> y2;

		private IEnumerable<Control> Each()
		{
			if (this.slope != null) yield return this.textBox_slope;
			if (this.intercept != null) yield return this.textBox_intercept;
			yield return this.textBox_x1;
			yield return this.textBox_y1;
			yield return this.textBox_x2;
			yield return this.textBox_y2;
		}

		public event EventHandler SomethingChanged
		{
			add
			{
				foreach (var textBox in this.Each())
				{
					textBox.TextChanged += value;
				}
			}
			remove
			{
				foreach (var textBox in this.Each())
				{
					textBox.TextChanged -= value;
				}
			}
		}

		private bool updating;

		private void HandleSomethingChanged(object sender, EventArgs e)
		{
			if (!this.updating)
			{
				this.Recalc(out _);
			}
		}

		private bool Recalc(out Lerp lerp)
		{
			bool good = true;
			good &= this.x1.TryGet(out double x1);
			good &= this.y1.TryGet(out double y1);
			good &= this.x2.TryGet(out double x2);
			good &= this.y2.TryGet(out double y2);
			if (!good)
			{
				lerp = default(Lerp);
				return false;
			}
			lerp = Lerp.FromTwoPoints(x1: x1, y1: y1, x2: x2, y2: y2);
			this.slope?.Load(lerp.Slope);
			this.intercept?.Load(lerp.Intercept);
			return true;
		}

		public void Load(Lerp value)
		{
			this.slope?.Load(value.Slope);
			this.intercept?.Load(value.Intercept);
			this.updating = true;
			try
			{
				this.x1.Load(this.initial_x1);
				this.y1.Load(value.Y(this.initial_x1));
				this.x2.Load(this.initial_x2);
				this.y2.Load(value.Y(this.initial_x2));
			}
			finally
			{
				this.updating = false;
			}
		}

		public bool TryGet(out Lerp value)
		{
			if (this.Recalc(out value))
			{
				return true;
			}
			else
			{
				value = default(Lerp);
				return false;
			}
		}
	}

	struct Lerp
	{
		public Lerp(double slope, double intercept)
		{
			this.Slope = slope;
			this.Intercept = intercept;
		}

		public static Lerp FromTwoPoints(double x1, double y1, double x2, double y2)
		{
			double dy = y2 - y1;
			double dx = x2 - x1;
			double slope = dy / dx;
			double intercept = y1 - slope * x1;
			return new Lerp(slope: slope, intercept: intercept);
		}

		public double Slope { get; }
		public double Intercept { get; }

		public double Y(double x)
		{
			return this.Slope * x + this.Intercept;
		}

		public override string ToString()
		{
			return $"({this.Slope}) * x + ({this.Intercept})";
		}
	}
}
