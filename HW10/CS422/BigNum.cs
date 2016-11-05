using System;
using System.Numerics;
using System.Collections;
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
			m_Power = power;
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
				var byteArray = BitConverter.GetBytes(value);
				BitArray bits = new BitArray (byteArray);

				bool negative = false;

				if (bits [63])
					negative = true;

				//Exponent
				BigInteger exp = ExponenetFromBitmap(bits);
				string expS = exp.ToString ();

				// Mantissa
				BigNum mantissa = MantissaFromBitmap(bits);
				string manS = mantissa.ToString ();

				BigNum one = new BigNum ("1");
				BigNum test =  one + mantissa;
				test = test * Pow2 ((int)exp);

				BigNum resultant = new BigNum (test.ToString ());

				m_Num = resultant.m_Num;
				m_Power = resultant.m_Power;
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
			//Special Case
			if (lhs.m_Power == rhs.m_Power) 
			{
				BigInteger bigSpecialSum = lhs.m_Num + rhs.m_Num;
				return new BigNum (bigSpecialSum, lhs.m_Power);
			}

			// Standard Case
			BigInteger A = lhs.m_Num;
			BigInteger B = rhs.m_Num;

			int biggerExp;
			int smallerExp;

			if(-lhs.m_Power > -rhs.m_Power)
			{		
				A = lhs.m_Num;
				B = rhs.m_Num;
				biggerExp = -lhs.m_Power;
				smallerExp = -rhs.m_Power;
			}
			else
			{
				A = rhs.m_Num;
				B = lhs.m_Num;
				biggerExp = -rhs.m_Power;
				smallerExp = -lhs.m_Power;
			}

			int newExp = biggerExp - smallerExp;

			BigInteger base10 = new BigInteger (10);
			BigInteger exp = BigInteger.Pow (base10, newExp);
			BigInteger sum = A * (exp) + B;

			BigNum summation = new BigNum (sum, -smallerExp);

			return summation;
		}

		public static BigNum operator-(BigNum lhs, BigNum rhs)
		{
			//Special Case
			if (lhs.m_Power == rhs.m_Power) 
			{
				BigInteger bigSpecialDiff = lhs.m_Num - rhs.m_Num;
				return new BigNum (bigSpecialDiff, lhs.m_Power);
			}

			int biggerExp;
			int smallerExp;
			int newExp;

			BigInteger base10 = new BigInteger (10);
			BigInteger exp;
			BigInteger sum;

			if(-lhs.m_Power > -rhs.m_Power)
			{		
				biggerExp = -lhs.m_Power;
				smallerExp = -rhs.m_Power;
				newExp = biggerExp - smallerExp;
				exp = BigInteger.Pow (base10, newExp);

				sum = (lhs.m_Num * (exp)) - rhs.m_Num;
			}
			else
			{
				biggerExp = -rhs.m_Power;
				smallerExp = -lhs.m_Power;
				newExp = biggerExp - smallerExp;
				exp = BigInteger.Pow (base10, newExp);

				sum = lhs.m_Num  - (rhs.m_Num * (exp));
			}

			BigNum summation = new BigNum (sum, -smallerExp);
			return summation;
		}

		public static BigNum operator*(BigNum lhs, BigNum rhs)
		{
			BigInteger product = BigInteger.Multiply (lhs.m_Num, rhs.m_Num);
			int newPow = lhs.m_Power + rhs.m_Power;
			return new BigNum (product, newPow);
		}

		public static BigNum operator/(BigNum lhs, BigNum rhs)
		{
			if (lhs.isUndefined || rhs.isUndefined || (rhs.m_Num == 0 && rhs.m_Power == 0))
				return new BigNum (double.NaN, false);
			
			BigInteger base10 = new BigInteger (10);
			BigInteger oneThirtyZeros = BigInteger.Pow (base10, 30);
			BigInteger Numerator = lhs.m_Num * oneThirtyZeros; 

			int power = lhs.m_Power - 30;

			BigInteger Denominator = rhs.m_Num;
			BigInteger bigQuotient = BigInteger.Divide (Numerator, Denominator);

			power = power - rhs.m_Power;

			if (power < 0)
				power = power * (-1);

			return new BigNum (bigQuotient,power);
		}

		public static bool operator>(BigNum lhs, BigNum rhs)
		{
			BigNum diff = lhs - rhs;
			char sign = diff.ToString () [0];

			if (sign != '-')
				return true;

			return false;
			
		}

		public static bool operator>=(BigNum lhs, BigNum rhs)
		{
			BigNum diff = lhs - rhs;
			char sign = diff.ToString () [0];

			if (sign != '-' || diff.m_Num == 0)
				return true;

			return false;
		}

		public static bool operator<(BigNum lhs, BigNum rhs)
		{
			BigNum diff = lhs - rhs;
			char sign = diff.ToString () [0];

			if (sign == '-')
				return true;

			return false;

		}

		public static bool operator<=(BigNum lhs, BigNum rhs)
		{
			BigNum diff = lhs - rhs;
			char sign = diff.ToString () [0];

			if (sign == '-' || diff.m_Num == 0)
				return true;

			return false;
		}

		public static bool IsToStringCorrect(double value)
		{
			BigNum toString = new BigNum (value, true);

			BigNum extractBits = new BigNum (value, false);

			if (toString.ToString () == extractBits.ToString ())
				return true;

			return false;
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


		public static BigInteger ExponenetFromBitmap(BitArray bits)
		{
			BigInteger base2 = new BigInteger (2);
			BigInteger exp = new BigInteger(0);

			for(int i = 62; i > 51; i--)
			{
				if (bits [i])
					exp = exp + BigInteger.Pow (base2,i - 52);
			}

			exp = exp - 1023;
			return exp;
		}

		public static BigNum MantissaFromBitmap(BitArray bits)
		{
			BitArray mantissa = new BitArray (52);
			BigInteger base2 = new BigInteger (2);
			BigInteger exp = new BigInteger(0);

			int j = 0;
			for (int i = 51; i >= 0; i--) 
			{
				if (bits [i])
					exp = exp + BigInteger.Pow (base2, (i - 52) * (-1));
			}

			BigNum fraction = new BigNum (exp.ToString ());
			fraction = fraction * Pow2 (-52);

			return fraction;
		}


		public static BigNum Pow2(int pow)
		{
			BigNum one = new BigNum ("1");

			if (pow < 0) 
			{
				BigNum Deno = Pow2 (-1 * pow);
				return one / Deno;
			}

			else
			{
				BigInteger baseTwo = new BigInteger (2);
				BigInteger exponential = BigInteger.Pow (baseTwo, pow);
				return new BigNum (exponential.ToString ()); 
			}
		}

	}
}