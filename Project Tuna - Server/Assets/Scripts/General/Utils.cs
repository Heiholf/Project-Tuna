using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Tuna
{
    public static class Utils
    {
        public static string AddEnumerationToString(string baseString)
        {
            Regex regex = new Regex(@".* - (?<number>\d+)");
            Match match = regex.Match(baseString);
            if (match.Success)
            {
                int number = int.Parse(match.Groups["number"].Value);
                string newName = baseString.Substring(0, match.Groups["number"].Index) + (number + 1);
                baseString = newName;
            }
            else
            {
                baseString += " - 2";
            }

            return baseString;
        }
    }

}

