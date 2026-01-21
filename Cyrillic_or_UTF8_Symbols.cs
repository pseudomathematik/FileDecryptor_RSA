using System.Text;
using System.Text.RegularExpressions;


namespace CyrillicOrUTF8
{
	public class Cyrillic_or_UTF8_Symbols
	{
		public static bool LooksLikeUtf8WithCyrillic(byte[] bytes, out string decoded)
		{
			decoded = string.Empty;
			try
			{
				var utf8 = new UTF8Encoding(false, true); // throwOnInvalidBytes = true
				decoded = utf8.GetString(bytes);

				// Проверяем наличие кириллических символов
				if (Regex.IsMatch(decoded, @"\p{IsCyrillic}"))
					return true;
				else
					return false;
			}
			catch
			{
				return false;
			}
		}
	}
}
