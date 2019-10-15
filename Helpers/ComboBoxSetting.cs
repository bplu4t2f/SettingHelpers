using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericSettings.Helpers
{
	class ComboBoxSetting<T> : ISettingHandler<T>
	{
		public ComboBoxSetting(ComboBox comboBox)
		{
			this.comboBox = comboBox ?? throw new ArgumentNullException(nameof(comboBox));
		}

		private readonly ComboBox comboBox;
		private readonly List<ComboBoxItem> items = new List<ComboBoxItem>();

		public ComboBoxItem AddValue(T value, IFormattable displayText)
		{
			var item = new ComboBoxItem(value, displayText);
			this.items.Add(item);
			this.UpdateItems();
			return item;
		}

		public void AddValues(IEnumerable<ComboBoxItem> values)
		{
			foreach (var value in values)
			{
				this.items.Add(value);
			}
			this.UpdateItems();
		}

		/// <summary>
		/// Only works if <typeparamref name="T"/> is an enum type.
		/// </summary>
		public void AddAllEnumValues()
		{
			foreach (T value in Enum.GetValues(typeof(T)))
			{
				this.AddValue(value, $"{value}");
			}
			this.UpdateItems();
		}

		public void UpdateItems()
		{
			this.comboBox.DataSource = null;
			this.comboBox.DataSource = this.items;
		}

		public sealed class ComboBoxItem
		{
			public ComboBoxItem(T value, IFormattable displayText = null)
			{
				this.Value = value;
				this.DisplayText = displayText;
			}
			public T Value { get; }
			public IFormattable DisplayText { get; set; }
			public override string ToString()
			{
				// NOTE: Returning null here will cause a OutOfMemoryException saying "Too many items in the combo box".
				return this.DisplayText?.ToString() ?? this.Value?.ToString() ?? String.Empty;
			}
		}

		public event EventHandler SomethingChanged
		{
			add { this.comboBox.SelectedIndexChanged += value; }
			remove { this.comboBox.SelectedIndexChanged -= value; }
		}

		public void Load(T value)
		{
			for (int i = 0; i < this.comboBox.Items.Count; ++i)
			{
				if (this.comboBox.Items[i] is ComboBoxItem item)
				{
					if (Object.Equals(item.Value, value))
					{
						this.comboBox.SelectedIndex = i;
						return;
					}
				}
			}
			this.comboBox.SelectedIndex = -1;
		}

		public bool TryGet(out T value)
		{
			if (this.comboBox.SelectedItem is ComboBoxItem item)
			{
				value = item.Value;
				return true;
			}
			else
			{
				value = default(T);
				return false;
			}
		}
	}
}
