using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Менеджер
{
    public interface IPartNoMask
    {
        void PartNoMask(ref string PartNo, int PartNoLength)
        {
            var newVal = Regex.Replace(PartNo, @"[^0-9A-Z]", "");
            if (PartNoLength != newVal.Length && !string.IsNullOrEmpty(newVal))
            {
                PartNoLength = newVal.Length;
                PartNo = string.Empty;
                if (newVal.Length <= 7)
                {
                    PartNo = Regex.Replace(newVal, @"([0-9A-Z]{7})", "$1");
                }
                else
                if (newVal.Length >= 8)
                {
                    PartNo = Regex.Replace(newVal, @"([0-9A-Z]{7})([0-9A-Z]{1})", "$1/$2");
                }
            }
        }
    }
}
