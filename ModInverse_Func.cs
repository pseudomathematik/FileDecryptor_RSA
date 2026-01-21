using System;
using System.Numerics;

namespace WpfApp_Decrypt_ModInv
{
	public class ModInverse_Func
	{
		public static BigInteger ModInverse(BigInteger a, BigInteger m)
		{
			BigInteger t = 0, newT = 1;
			BigInteger r = m, newR = a;

			while (newR != 0)
			{
				BigInteger q = r / newR;
				(t, newT) = (newT, t - q * newT);
				(r, newR) = (newR, r - q * newR);
			}

			if (r > 1) // a не обратим
				throw new Exception("Не найден обратный: gcd != 1");

			if (t < 0) t += m;
			return t;
		}
	}
}
