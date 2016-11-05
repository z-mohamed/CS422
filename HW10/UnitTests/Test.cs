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

		[Test ()]
		public void ComplexCtor ()
		{
			BigNum myBigNum = new BigNum (48.9, false);
			Assert.False (myBigNum.isUndefined);
		}


		[Test ()]
		public void SummationSon ()
		{
			BigNum myBigNum1 = new BigNum ("1831.5671");
			BigNum myBigNum2 = new BigNum ("432.1");

			BigNum bigSum = myBigNum2 + myBigNum1;

			// 1831.5671 + 432.1 = 2263.6671

			Assert.AreEqual ("1831.5671", myBigNum1.ToString());
			Assert.AreEqual ("432.1", myBigNum2.ToString());
			Assert.AreEqual ("2263.6671", bigSum.ToString());
		}


		[Test ()]
		public void SubtractSon ()
		{
			BigNum myBigNum1 = new BigNum ("1831.5671");
			BigNum myBigNum2 = new BigNum ("432.1");

			BigNum bigDiff = myBigNum1 - myBigNum2;

			// 1831.5671 + 432.1 = 2263.6671

			Assert.AreEqual ("1831.5671", myBigNum1.ToString());
			Assert.AreEqual ("432.1", myBigNum2.ToString());
			Assert.AreEqual ("1399.4671", bigDiff.ToString());
		}


		[Test ()]
		public void SpecialCaseSubtract()
		{

			BigNum myBigNum1 = new BigNum ("1831.19");
			BigNum myBigNum2 = new BigNum ("432.14");

			BigNum specialDiff = myBigNum1 - myBigNum2;

			Assert.AreEqual ("1831.19", myBigNum1.ToString ());
			Assert.AreEqual ("432.14", myBigNum2.ToString ());
			Assert.AreEqual ("1399.05", specialDiff.ToString ());

		}

		[Test ()]
		public void MultiplySon  ()
		{
			BigNum myBigNum1 = new BigNum ("1831.19");
			BigNum myBigNum2 = new BigNum ("432.14");

			BigNum product = myBigNum1 * myBigNum2;

			Assert.AreEqual ("1831.19", myBigNum1.ToString ());
			Assert.AreEqual ("432.14", myBigNum2.ToString ());
			Assert.AreEqual ("791330.4466", product.ToString ());
		}


		[Test ()]
		public void DivideSon  ()
		{
			BigNum myBigNum1 = new BigNum ("1");
			BigNum myBigNum2 = new BigNum ("3");

			BigNum quotient = myBigNum1 / myBigNum2;

			int x = 0;
		}


		[Test ()]
		public void lessThanThou  ()
		{
			BigNum myBigNum1 = new BigNum ("1");
			BigNum myBigNum2 = new BigNum ("3");

			Assert.IsTrue (myBigNum1 < myBigNum2);
		}

		[Test ()]
		public void lessThanThouOrEqual  ()
		{
			BigNum myBigNum1 = new BigNum ("3");
			BigNum myBigNum2 = new BigNum ("3");

			Assert.IsTrue (myBigNum1 <= myBigNum2);
		}

		[Test ()]
		public void greaterThanThou  ()
		{
			BigNum myBigNum1 = new BigNum ("1");
			BigNum myBigNum2 = new BigNum ("3");

			Assert.IsTrue (myBigNum2 > myBigNum1);
		}

		[Test ()]
		public void greaterThanThouOrEqual  ()
		{
			BigNum myBigNum1 = new BigNum ("3");
			BigNum myBigNum2 = new BigNum ("3");

			Assert.IsTrue (myBigNum2 >= myBigNum1);
		}

			
		/*
		[Test ()]
		public void  ()
		{
		}
		*/
	}
}

