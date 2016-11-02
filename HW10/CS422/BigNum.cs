using System;
using System.Numerics;

namespace CS422
{
	public class BigNum
	{
		BigInteger m_Num;
		BigInteger m_Power;

		public BigNum (string number)
		{
		}

		public BigNum( double value, bool useDoubleToString)
		{
		}
			
		public override string ToString ()
		{
			return string.Format ("[BigNum]");
		}

		public bool isUndefined { get;}

		public static BigNum operator+(BigNum lhs, BigNum rhs)
		{
			throw new NotImplementedException ();
		}

		public static BigNum operator-(BigNum lhs, BigNum rhs)
		{
			throw new NotImplementedException ();
		}

		public static BigNum operator*(BigNum lhs, BigNum rhs)
		{
			throw new NotImplementedException ();
		}

		public static BigNum operator/(BigNum lhs, BigNum rhs)
		{
			throw new NotImplementedException ();
		}

		public static bool operator>(BigNum lhs, BigNum rhs)
		{
			throw new NotImplementedException ();
		}

		public static bool operator>=(BigNum lhs, BigNum rhs)
		{
			throw new NotImplementedException ();
		}

		public static bool operator<(BigNum lhs, BigNum rhs)
		{
			throw new NotImplementedException ();
		}

		public static bool operator<=(BigNum lhs, BigNum rhs)
		{
			throw new NotImplementedException ();
		}

		public static bool IsToStringCorrect(double value)
		{
			throw new NotImplementedException ();
		}

		/***************************************************/
		/*				Utility Functions 				   */
		/***************************************************/

		internal bool IsStringValidNum(string number)
		{
			throw new NotImplementedException ();
		}

		internal bool IsDoubleUndefined(double value)
		{
			throw new NotImplementedException ();
		}

	}


}