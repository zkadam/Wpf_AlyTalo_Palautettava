using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wpf_AlyTalo_Palautettava
{
    class Thermostat
    {

        public int Temperature { get; set; }



        public int TavoiteLampo
        {

            get
            {
                return tavoiteLampo;
            }
            set
            {
                if (Regex.IsMatch(value.ToString(), "^[0-9]{1,2}$"))
                {
                    tavoiteLampo = value;
                }
                else
                {
                    tavoiteLampo = 20;
                    throw new ArgumentOutOfRangeException();
                }
            }

        }
        private int tavoiteLampo;
    }
}
