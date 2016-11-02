using NUnit.Framework;
using System;
using CS422;

namespace UnitTests
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void IsStringValidNum ()
		{
			Assert.False (CS422.BigNum.IsStringValidNum ("    "));
			Assert.False (CS422.BigNum.IsStringValidNum ("-1234-5"));
			Assert.False (CS422.BigNum.IsStringValidNum ("hello"));
			Assert.False (CS422.BigNum.IsStringValidNum ("-12345.789 "));
			Assert.False (CS422.BigNum.IsStringValidNum ("12234-"));
			Assert.False (CS422.BigNum.IsStringValidNum ("122-34"));
			Assert.False (CS422.BigNum.IsStringValidNum ("12.23.4"));
			Assert.False (CS422.BigNum.IsStringValidNum ("s12356"));
			Assert.False (CS422.BigNum.IsStringValidNum ("12a234"));
			Assert.False (CS422.BigNum.IsStringValidNum (""));
			Assert.False (CS422.BigNum.IsStringValidNum (null));

			Assert.True (CS422.BigNum.IsStringValidNum ("123455"));
			Assert.True (CS422.BigNum.IsStringValidNum ("-123455"));
			Assert.True (CS422.BigNum.IsStringValidNum ("-12345.5"));
			Assert.True (CS422.BigNum.IsStringValidNum ("-.123455"));
			Assert.True (CS422.BigNum.IsStringValidNum (".123455"));
			Assert.True (CS422.BigNum.IsStringValidNum ("123455."));
		}

		[Test ()]
		public void StandardCtor ()
		{
			BigNum myBigNum = new BigNum ("127.6448");
			Assert.False (myBigNum.isUndefined);
		}
			
		[Test ()]
		public void PruneLeadingZeros  ()
		{
			string pruned = BigNum.PruneLeadingZeros ("0.07");
			Assert.AreEqual (".07", pruned);

			pruned = BigNum.PruneLeadingZeros ("000000010.07");
			Assert.AreEqual ("10.07", pruned);
		}

		[Test ()]
		public void PruneTrailingZeros  ()
		{
			string pruned = BigNum.PruneTrailingZeros ("0.789000001");
			Assert.AreEqual ("0.789000001", pruned);

			pruned = BigNum.PruneTrailingZeros ("0.78900000");
			Assert.AreEqual ("0.789", pruned);
		}
			
		[Test ()]
		public void PruneEverything  ()
		{
			string pruned = BigNum.PruneTrailingZeros ("0.789000001");
			pruned = BigNum.PruneLeadingZeros (pruned);
			Assert.AreEqual (".789000001", pruned);

			pruned = BigNum.PruneTrailingZeros ("000000010.78900000");
			pruned = BigNum.PruneLeadingZeros (pruned);
			Assert.AreEqual ("10.789", pruned);
		}

		[Test ()]
		public void PruneZero  ()
		{
			string pruned = BigNum.PruneTrailingZeros ("00000000000000000.0000000000000000");
			pruned = BigNum.PruneLeadingZeros (pruned);
			Assert.AreEqual ("0", pruned);
		}

		[Test ()]
		public void ToStringSimple  ()
		{
			BigNum myBigNum = new BigNum ("000127.64480000");
			string num = myBigNum.ToString();
			Assert.AreEqual ("127.6448", num);

			BigNum myBigNum2 = new BigNum ("00012764480000");
			string num2 = myBigNum2.ToString();
			Assert.AreEqual ("12764480000", num2);

			BigNum myBigNum3 = new BigNum ("-00012764480000");
			string num3 = myBigNum3.ToString();
			Assert.AreEqual ("-12764480000", num3);

			BigNum myBigNum4 = new BigNum ("0.40");
			string num4 = myBigNum4.ToString();
			Assert.AreEqual (".4", num4);
		}

		/*
		[Test ()]
		public void  ()
		{
		}
		*/
	}
}

