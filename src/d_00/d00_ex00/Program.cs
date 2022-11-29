using System;
using System.Globalization;
using System.Threading;

double sum;
double rate;
int term;

if ( args.Length < 3 ||!double.TryParse(args[0], out sum) || !double.TryParse(args[1], out rate) || !int.TryParse(args[2], out term)
    || sum <= 0 || term <= 0)
{
    Console.Error.WriteLine("Something went wrong. Check your input and retry.");
    return;
}

var dateTime = DateTime.Now;
var cultureInfo = new CultureInfo("en-GB");
var interestRate = rate / 12 / 100;
var monthlyPayment = sum * interestRate * Math.Pow(1 + interestRate, term) / (Math.Pow(1 + interestRate, term) - 1);
var totalDebtBalance = sum;
double principalDebit;
Thread.CurrentThread.CurrentCulture = cultureInfo;
for (int currentMonth = 1; currentMonth <= term; currentMonth++)
{
    var currentDateTime = dateTime.AddMonths(1);
    var monthlyPaymentInterest = totalDebtBalance * rate * (currentDateTime - dateTime).Days / (100 * (dateTime.AddYears(1) - dateTime).Days);
    if (currentMonth != term)
    {
        principalDebit = monthlyPayment - monthlyPaymentInterest;
        totalDebtBalance -= principalDebit;
    }
    else
    {
        monthlyPayment = totalDebtBalance + monthlyPaymentInterest;
        principalDebit = totalDebtBalance;
        totalDebtBalance = 0;
    }
    Console.WriteLine($"{currentMonth}\t" +
                      $"{dateTime.ToString("MM/dd/yyyy",cultureInfo)}\t" +
                      $"{monthlyPayment, -10:N2}\t" +
                      $"{principalDebit, -10:N2}\t" +
                      $"{monthlyPaymentInterest, -10:N2}" +
                      $"{totalDebtBalance, -10:N2}");
}