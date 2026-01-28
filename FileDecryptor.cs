using CyrillicOrUTF8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
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


            
            List<byte> result = new List<byte>();

            using (BinaryReader br = new BinaryReader(File.Open(encFile, FileMode.Open)))
            {
                int blockCount = br.ReadInt32();
                int originalSize = br.ReadInt32();

                byte[] nBytes = n.ToByteArray();
                int keySizeInBytes =
                    nBytes[nBytes.Length - 1] == 0 ? nBytes.Length - 1 : nBytes.Length;

                int blockSize = keySizeInBytes <= 2 ? 1 : keySizeInBytes - 1;

                for (int i = 0; i < blockCount; i++)
                {
                    int len = br.ReadInt32();
                    byte[] encBytes = br.ReadBytes(len);

                    BigInteger c = new BigInteger(encBytes);
                    BigInteger m = BigInteger.ModPow(c, d, n);

                    byte[] mBytes = m.ToByteArray(); 
                   
                    if (mBytes.Length > 1 && mBytes[mBytes.Length - 1] == 0)
                    {
                        Array.Resize(ref mBytes, mBytes.Length - 1);
                    }

                    
                    Array.Reverse(mBytes);

                    
                    byte[] block = new byte[blockSize];
                    int copyLen = Math.Min(mBytes.Length, blockSize);

                    // копируем В КОНЕЦ блока
                    Array.Copy(
                        mBytes,
                        mBytes.Length - copyLen,
                        block,
                        blockSize - copyLen,
                        copyLen
                    );

                    int bytesLeft = originalSize - result.Count;
                    if (bytesLeft <= 0) break;

                    int toCopy = Math.Min(blockSize, bytesLeft);
                    for (int j = 0; j < toCopy; j++)
                    {
                        result.Add(block[j]);
                    }
                }
            }

            byte[] resultBytes = result.ToArray();

            
            File.WriteAllBytes(outFile, resultBytes);

            
            string ext = Path.GetExtension(outFile).ToLower();
            if (ext == ".txt")
            {
                if (Cyrillic_or_UTF8_Symbols.LooksLikeUtf8WithCyrillic(resultBytes, out string decodedUtf8))
                {
                    Encoding cp1251 = Encoding.GetEncoding(1251);
                    File.WriteAllText(outFile, decodedUtf8, cp1251);
                }
            }

            log($"Файл {encFile}  расшифрован в {outFile}");
        }
    }
  
}



