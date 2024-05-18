using System.Text.RegularExpressions;

namespace Scanner.Toolkit
{
    public static class StringHelper
    {

        public static string UseRegexSubString(string regex, string value)
        {
            string str = "";
            MatchCollection matchCollection = new Regex(regex).Matches(value);
            if (matchCollection.Count > 0)
            {
                str = matchCollection[0].Groups[0].Value;
            }
            return str;
        }
    }
}
