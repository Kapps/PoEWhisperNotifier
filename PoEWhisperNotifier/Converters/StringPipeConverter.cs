using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PoEWhisperNotifier.Converters {
	/// <summary>
	/// A TypeConverter that converts an array of strings into one separated by pipe characters.
	/// </summary>
	public class StringPipeConverter : TypeConverter {
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
			if (value is string[] Val) {
				if (Val.Any(c => c.Contains("|")))
					throw new FormatException("Individual values may not contain the | character.");
				return String.Join("|", Val);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
			var Val = value as string;
			return Val?.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
		}
	}
}
