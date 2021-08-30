using System;
using TraderBlotter.Api.Utilities;

namespace LicenseGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
			try
			{
				Console.WriteLine(HelperMethods.GetLicensekey());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadLine();
        }
    }
}
