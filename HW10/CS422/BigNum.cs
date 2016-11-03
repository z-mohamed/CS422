using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace CS422
{
	public class BigNum
	{
		readonly BigInteger m_Num;
		readonly int m_Power;

		private BigNum( BigInteger num, int power)
		{
			m_Num = num;
			m_Power = m_Power;
			isUndefined = false;
		}


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

				var union = new DoubleLongUnion();
				union.Double = value;


				var longBytes = union.Long;

				ulong sign =(longBytes>>63);

				ulong exp =(longBytes<<1)>>53;

				ulong fraction = (longBytes << 12);


				unsafe 
				{

					var bit = ((ulong*)&longBytes)[0];
					int x = 0;

				}

				




			}
		}

		[StructLayout(LayoutKind.Explicit)]
		struct DoubleLongUnion
		{
			[FieldOffset(0)] 
			public ulong Long;

			[FieldOffset(0)] 
			public double Double;
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
			
			BigInteger lhsActual = BigInteger.Parse (lhs.m_Num.ToString ());
			lhsActual = lhsActual * (10 ^ (-lhs.m_Power));

			BigInteger thing2 = rhs.m_Num;
			thing2 = thing2 * 10 ^ (-rhs.m_Power);

			BigInteger sumthing = lhsActual + thing2;

			string finalthing = sumthing.ToString ();

			BigNum summation = new BigNum (finalthing);

			/*
			if(lhs.m_Power == rhs.m_Power)
			{
				BigInteger sum = lhs.m_Num + rhs.m_Num;
				summation = new BigNum (sum, lhs.m_Power);
			}

			BigNum LargerExp;
			BigNum SmallerExp;

			if(lhs.m_Power > rhs.m_Power)
			{
				LargerExp = lhs;
				SmallerExp = rhs;
			}
			else
			{
				LargerExp = rhs;
				SmallerExp = lhs;
			}


			// Increase rhs.m_Power by
			// lhs.m_Power - rhs.m_Power
			//Change 
			//else( rhs.m_Power < rhs


			*/
			return summation;
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