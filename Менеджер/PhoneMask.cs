using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Менеджер
{
    public interface IPhoneMask
    {
        void PhoneMask(ref string Phone , int PhoneLength)
        {
            var newVal = Regex.Replace(Phone, @"[^0-9]", "");
            if (PhoneLength != newVal.Length && !string.IsNullOrEmpty(newVal))
            {
                PhoneLength = newVal.Length;
                Phone = string.Empty;
                if (newVal.Length <= 3)
                {
                   Phone = Regex.Replace(newVal, @"(\d{3})", "+$1");
                }
                else if (newVal.Length <= 5)
                {
                    Phone = Regex.Replace(newVal, @"(\d{3})(\d{0,2})", "+$1($2)");
                }
                else if (newVal.Length <= 8)
                {
                    Phone = Regex.Replace(newVal, @"(\d{3})(\d{2})(\d{0,3})", "+$1($2)$3");
                }
                else if (newVal.Length <= 10)
                {
                   Phone = Regex.Replace(newVal, @"(\d{3})(\d{2})(\d{0,3})(\d{0,2})", "+$1($2)$3-$4");
                }
                else if (newVal.Length > 10)
                {
                  Phone = Regex.Replace(newVal, @"(\d{3})(\d{2})(\d{0,3})(\d{0,2})(\d{0,2})", "+$1($2)$3-$4-$5");
                }
            }
        }
    }
}
