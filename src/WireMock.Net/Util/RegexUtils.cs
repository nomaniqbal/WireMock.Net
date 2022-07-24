using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WireMock.RegularExpressions;

namespace WireMock.Util;

internal static class RegexUtils
{
    private static readonly TimeSpan RegexTimeOut = new(0, 0, 10);

    public static Dictionary<string, string> GetNamedGroups(Regex regex, string input)
    {
        var namedGroupsDictionary = new Dictionary<string, string>();

        GroupCollection groups = regex.Match(input).Groups;
        foreach (string groupName in regex.GetGroupNames())
        {
            if (groups[groupName].Captures.Count > 0)
            {
                namedGroupsDictionary.Add(groupName, groups[groupName].Value);
            }
        }

        return namedGroupsDictionary;
    }

    public static (bool IsValid, bool Result) MatchRegex(string pattern, string input, bool useRegexExtended = true)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            return (false, false);
        }

        try
        {
            if (useRegexExtended)
            {
                var r = new RegexExtended(pattern, RegexOptions.None, RegexTimeOut);
                return (true, r.IsMatch(input));
            }
            else
            {
                var r = new Regex(pattern, RegexOptions.None, RegexTimeOut);
                return (true, r.IsMatch(input));
            }
        }
        catch
        {
            return (false, false);
        }
    }
}