using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHype.Infrastructure.Clients.FreeToPlay.Casting
{
    public class RamConversor
    {

        public static int? ConvertRamStringToMb(string? ramString)
        {
            if (string.IsNullOrWhiteSpace(ramString)) return null;

            var s = ramString.Trim().ToUpperInvariant();

            var firstDigit = new string(s.SkipWhile(ch => !char.IsDigit(ch))
                                     .TakeWhile(char.IsDigit)
                                     .ToArray());

            if (!int.TryParse(firstDigit, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                return null;

            if (s.Contains("GB") && !s.Contains("MB")) return value * 1024;
            if (s.Contains("MB")) return value;

            return null;
        }
    }
}


