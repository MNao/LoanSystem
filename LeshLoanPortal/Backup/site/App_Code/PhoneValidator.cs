using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;


public class PhoneValidator
{
    private string okNumber = "";
    private ArrayList validNumbers, invalidNumbers;
    private Databasefile dp;
    public bool PhoneNumbersOk(string numbers)
    {
        bool phonesok = false;
        if (!numbers.Trim().Equals(""))
        {
            string[] stringSeparators = new string[] { ",", "\r\n" };
            string[] phones = numbers.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            validNumbers = new ArrayList();
            invalidNumbers = new ArrayList();
            foreach (string number in phones)
            {
                if (!number.Trim().Equals(""))
                {
                    if (NumberFormatIsValid(number.Trim()))
                    {
                        //Console.WriteLine(number.Trim() + "'s format is ok");
                        if (!NumberContainsLetters(okNumber.Trim()))
                        {
                            if (NetworkCodeOk(okNumber))
                            {
                                validNumbers.Add(okNumber.Trim());

                            }
                            else
                            {
                                invalidNumbers.Add(number.Trim());
                            }
                        }
                        else
                        {
                            //Console.WriteLine(okNumber + " Contains Letters");
                            invalidNumbers.Add(number.Trim());
                        }
                    }
                    else
                    {
                        //Console.WriteLine(number + " has an invalid number format");
                        invalidNumbers.Add(number.Trim());
                    }
                    if (invalidNumbers.Count > 0)
                    {
                        phonesok = false;
                    }
                    else
                    {
                        phonesok = true;
                    }
                }
            }
        }
        else
        {
            phonesok = true;
        }
        return phonesok;
    }

    private bool NumberIsBlacklisted(string okNumber)
    {
        bool blacklisted = false;
        try
        {
            dp = new Databasefile();
            ArrayList blacklistedNumbers = dp.GetBlackListedNumbers();
            if (blacklistedNumbers.Contains(okNumber))
            {
                blacklisted = true;
            }
            else
            {
                blacklisted = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return blacklisted;
    }
    public ArrayList GetInvalidNumbers()
    {
        return invalidNumbers;
    }
    public ArrayList GetValidNumbers()
    {
        return validNumbers;
    }

    private bool NumberContainsLetters(string number)
    {
        bool containsLetters = false;
        ArrayList digits = new ArrayList();
        digits.Add('0');
        digits.Add('1');
        digits.Add('2');
        digits.Add('3');
        digits.Add('4');
        digits.Add('5');
        digits.Add('6');
        digits.Add('7');
        digits.Add('8');
        digits.Add('9');
        char[] chars = number.ToCharArray();
        foreach (char c in chars)
        {
            if (!digits.Contains(c))
            {
                containsLetters = true;
                break;
            }
        }
        return containsLetters;
    }

    private bool NumberFormatIsValid(string number)
    {
        bool isValid = false;
        okNumber = "";
        if (number.Trim().StartsWith("000256") && number.Length == 15)
        {
            okNumber = number.Remove(0, 6);
            isValid = true;
        }
        else if (number.Trim().StartsWith("00256") && number.Length == 14)
        {
            okNumber = number.Remove(0, 5);
            isValid = true;
        }
        else if ((number.Trim().StartsWith("256") && number.Length == 12))
        {
            okNumber = number.Remove(0, 3);
            isValid = true;
        }
        else if ((number.Trim().StartsWith("0") && number.Length == 10))
        {
            okNumber = number.Remove(0, 1);
            isValid = true;
        }
        else if ((number.Trim().StartsWith("7") && number.Length == 9))
        {
            okNumber = number;
            isValid = true;
        }
        else if ((number.Trim().StartsWith("+") && number.Length == 13))
        {
            okNumber = number.Remove(0, 4);
            isValid = true;
        }
        else
        {
            okNumber = number;
            isValid = false;
        }
        return isValid;
    }
    public string Format(string number)
    {
        okNumber = "";
        if (number.Trim().StartsWith("000256") && number.Length == 15)
        {
            okNumber = number.Remove(0, 3);
        }
        else if (number.Trim().StartsWith("00256") && number.Length == 14)
        {
            okNumber = number.Remove(0, 2);
        }
        else if ((number.Trim().StartsWith("256") && number.Length == 12))
        {
            okNumber = number;
        }
        else if ((number.Trim().StartsWith("0") && number.Length == 10))
        {
            okNumber = number.Remove(0, 1);
            okNumber = "256" + okNumber;
        }
        else if ((number.Trim().StartsWith("7") && number.Length == 9))
        {
            okNumber = "256" + number;
        }
        else if ((number.Trim().StartsWith("+") && number.Length == 13))
        {
            okNumber = number.Remove(0, 1);
        }
        else
        {
            okNumber = number;
        }
        return okNumber;
    }

    private bool NetworkCodeOk(string okNumber)
    {
        bool ok = false;
        string code = okNumber.Substring(0, 3);
        dp = new Databasefile();
        Hashtable networkCodes;
        networkCodes = dp.GetNetworkCodes();
        ArrayList codes = new ArrayList(networkCodes.Keys);
        if (codes.Contains(code))
        {
            ok = true;
        }
        else
        {
            ok = false;
        }
        return ok;
    }
    public Hashtable CheckNumbers(ArrayList numbers)
    {
        Hashtable networkCount = new Hashtable();
        try
        {
            dp = new Databasefile();
            Hashtable networkRates = dp.GetNetworkRates();
            Hashtable networkCodes = dp.GetNetworkCodes();
            ArrayList networks = new ArrayList(networkRates.Keys);
            foreach (string network in networks)
            {
                networkCount.Add(network, 0);
            }
            foreach (string number in numbers)
            {
                string code = number.Substring(0, 3);
                string network = networkCodes[code].ToString();
                int count = int.Parse(networkCount[network].ToString());
                int intCount = count + 1;
                networkCount[network] = intCount;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return networkCount;
    }
    public DataTable CalculateNetworkCost(DataTable dt)
    {
        //DataTable dt1 = dt;
        try
        {
            dp = new Databasefile();
            Hashtable networkRates = dp.GetNetworkRates();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string network = dr["Network"].ToString();
                int networkCount = int.Parse(dr["TotalSms"].ToString());
                int networkRate = int.Parse(networkRates[network].ToString());
                int totalCost = networkCount * networkRate;
                dr["TotalCost"] = totalCost.ToString("0,0");
                dr["Network"] = network;
                dr["TotalSms"] = networkCount.ToString("0,0");
                //dt.Rows.RemoveAt(0);
                //dt.Rows.Add(dr);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
}

