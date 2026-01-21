using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Windows;
using CyrillicOrUTF8;
namespace RSA_Decryptor
{
	public class FileDecryptor
	{
		public readonly BigInteger d;
		public readonly BigInteger n;
		private readonly Action<string> log; //  делегат вывода
		public FileDecryptor(BigInteger d, BigInteger n, Action<string> log)
		{
			this.d = d;
			this.n = n;
			this.log = log;
		}
		
		public void DecryptFile(String encFile)
		{	
			String outFile = encFile.Replace(".enc", String.Empty);

			
			List<byte> output = new List<byte>();
			int blockIndex = 1;

			using (BinaryReader br = new BinaryReader(File.Open(encFile, FileMode.Open)))
			{
				while (br.BaseStream.Position < br.BaseStream.Length)
				{
					int len = br.ReadInt32();
					byte[] encBytes = br.ReadBytes(len);
					BigInteger c = new BigInteger(encBytes);
					BigInteger m = BigInteger.ModPow(c, d, n);
					byte b = (byte)m;

					output.Add(b);
					blockIndex++;
				}
			}

			byte[] resultBytes = output.ToArray();

			
			try
			{
				File.WriteAllBytes(outFile, resultBytes);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при записи файла: {ex.Message}","Ошибка",MessageBoxButton.OK);
				return;
			}
			if (Cyrillic_or_UTF8_Symbols.LooksLikeUtf8WithCyrillic(resultBytes, out string decodedUtf8))
			{
				try
				{
					// Перезаписываем тот же файл с корректной кодировкой Windows-1251
					Encoding cp1251 = Encoding.GetEncoding(1251);
					File.WriteAllText(outFile, decodedUtf8, cp1251);
					
				}
				catch (Exception ex)
				{
					log($"Не удалось обновить файл в Windows-1251: {ex.Message}");
				}
			}
			else
			{
				
				log($"Файл {encFile}\n расшифрован  в \n {outFile}\n");
			}
		}

	}
}


