using GenericSettings.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericSettings
{
	class MySettings
	{
		public bool Bool1 { get; set; } = true;
		public bool Bool2 { get; set; }

		public string File { get; set; }

		public Font Font { get; set; }

		public int TheInt { get; set; } = 5;

		public int OtherInt { get; set; } = 123;

		public Lerp Lerp { get; set; }

		public MyEnum Enum { get; set; }
		public MyEnum Enum2 { get; set; } = MyEnum.EnumValue2;

		public Color Color { get; set; } = Color.AliceBlue;
	}

	enum MyEnum
	{
		EnumValue0,
		EnumValue1,
		EnumValue2,
	}
}
