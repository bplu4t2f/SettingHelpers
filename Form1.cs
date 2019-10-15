using GenericSettings.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericSettings
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			this.InitializeComponent();

			var helper = new SettingsUIHelper<MySettings>();
			helper.Add(new CheckBoxSetting(this.checkBox1), x => x.Bool1, (x, v) => x.Bool1 = v);
			helper.Add(new CheckBoxSetting(this.checkBox2), x => x.Bool2, (x, v) => x.Bool2 = v);
			helper.Add(new FontSetting(this.buttonFont, this.labelFont), x => x.Font, (x, v) => x.Font = v);
			helper.Add(new IntSliderSetting(this.trackBar1, this.labelTrackBarDisplay), x => x.OtherInt, (x, v) => x.OtherInt = v);

			helper.Add(x => x.Color, (x, v) => x.Color = v).Handler(new ColorSetting(this.buttonColor));

			helper.Add(x => x.File, (x, v) => x.File = v).Handler(new FileDialogSetting(this.textBox8, this.buttonBrowseFile));

			helper.Add(x => x.TheInt, (x, v) => x.TheInt = v).ParsingTextBox(this.textBox1);

			//helper.Add(x => x.Bool1, (x, v) => x.Bool1 = v).CheckBox(this.checkBox1);

			var theIntSetting = helper.Add(new ComboBoxSetting<int>(this.comboBox1), x => x.TheInt, (x, v) => x.TheInt = v);

			var enumSetting = helper.Add(x => x.Enum, (x, v) => x.Enum = v).Handler(new ComboBoxSetting<MyEnum>(this.comboBox2));
			enumSetting.AddAllEnumValues();

			var five = new BogusFormattable() { Value = "five" };
			theIntSetting.AddValue(5, five);
			theIntSetting.AddValue(5538867, $"best");

			helper.Add(x => x.Lerp, (x, v) => x.Lerp = v).Handler(new TwoPointLerpSetting(
				this.textBox2,
				this.textBox3,
				this.textBox_x1,
				this.textBox_y1,
				this.textBox_x2,
				this.textBox_y2,
				0.0,
				1.0
				));

			var radioButtonSetting = helper.Add(x => x.Enum2, (x, v) => x.Enum2 = v).Handler(new RadioButtonSetting<MyEnum>());
			radioButtonSetting.AddValue(this.radioButton1, MyEnum.EnumValue0);
			radioButtonSetting.AddValue(this.radioButton2, MyEnum.EnumValue1);
			radioButtonSetting.AddValue(this.radioButton3, MyEnum.EnumValue2);

			this.button2.Click += (sender, e) => { five.Value = "six"; theIntSetting.UpdateItems(); /*this.comboBox1.Items.RemoveAt(bleh); */} ;

			foreach (var setting in helper.Settings)
			{
				setting.LoadFrom(this.settings);
			}

			this.button3.Click += (sender, e) =>
			{
				foreach (var setting in helper.Settings)
				{
					if (!setting.TrySaveTo(this.settings))
					{
						MessageBox.Show("invalid setting");
						break;
					}
				}
				;
			};
		}

		private readonly MySettings settings = new MySettings();
	}

	public class BogusFormattable : IFormattable
	{
		public string Value { get; set; }

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.ToString();
		}

		public override string ToString()
		{
			return this.Value;
		}
	}
}
