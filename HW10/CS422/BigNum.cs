using System;
using System.Numerics;

namespace CS422
{
	public class BigNum
	{
		readonly BigInteger m_Num;
		readonly int m_Power;

		public BigNum (string number)
		{
			// If string does not represent a valid
			// number throw an exception
			if (!IsStringValidNum (number))
				throw new ArgumentException ();


			// Remove all insignificant zeros
			// that are left of decimal point
			number = PruneLeadingZeros(number);


			if(number.Contains("."))
			{
				// Remove all insignificant zeros
				// that are right of decimal point
				number = PruneTrailingZeros (number);


				int decimalIndex = number.IndexOf ('.');

				// Remove decimal index
				number= number.Remove (decimalIndex, 1);

				m_Power = (number.Length - decimalIndex);
			}
			else
			{
				m_Power = 0;
			}

			m_Num = BigInteger.Parse (number);

			isUndefined = false;
			
		}

		public BigNum( double value, bool useDoubleToString)
		{
			if(IsDoubleUndefined(value))
			{
				isUndefined = true;
			}
			else if(useDoubleToString)
			{
				string number = value.ToString ();

				// Redundant code
				if (!IsStringValidNum (number))
					throw new ArgumentException ();

				number = PruneLeadingZeros(number);


				if(number.Contains("."))
				{
					number = PruneTrailingZeros (number);

					int decimalIndex = number.IndexOf ('.');
					number= number.Remove (decimalIndex, 1);
					m_Power = (number.Length - decimalIndex);
				}
				else
				{
					m_Power = 0;
				}

				m_Num = BigInteger.Parse (number);

				isUndefined = false;

			}
			else
			{
				//Parse through the bits in value
			}
		}
			
		public override string ToString ()
		{
			if (this.isUndefined) 
			{
				return "undefined";
			}

			string num = m_Num.ToString ();

			if (m_Power != 0) 
			{
				int decimalIndex = num.Length - m_Power;
				num = num.Insert (decimalIndex, ".");
			}
			return num;
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

		public static bool IsStringValidNum(string number)
		{
			if (number == null)
				return false;
			
			if(number.Contains(" ")|| number == "" )
				return false;

			int countDecimalPoint = 0;
			int countMinusSign = 0;

			for(int i = 0; i <number.Length; i++)
			{
				if (number [i] != '.' && number [i] != '-' && !Char.IsDigit (number [i]))
					return false;
				
				if (number [i] == '-' && i != 0)
					return false;

				if (number [i] == '-')
					countMinusSign++;
				
				if (number [i] == '.')
					countDecimalPoint++;

				if (countDecimalPoint > 1 || countMinusSign > 1)
					return false;
			}

			return true;
		}

		public static bool IsDoubleUndefined(double value)
		{
			if (Double.IsNaN (value) || Double.IsInfinity (value))
				return true;

			return false;
		}

		public static string PruneLeadingZeros (string number)
		{
			string leanNum;
			int i = 0;

			while (number [i] == '0' && i<number.Length) 
			{
				i++;
			}

			if (i == number.Length)
				return "0";
		
			leanNum = number.Substring (i);

			if (leanNum == ".")
				leanNum = "0";


			return leanNum;
		}

		public static string PruneTrailingZeros (string number)
		{
			string leanNum;
			int i = number.Length - 1;

			while (number [i] == '0') 
			{
				i--;
			}

			leanNum = number.Substring (0, i + 1);

			if (leanNum == ".")
				leanNum = "0";
			
			return leanNum;
		}

	}
}