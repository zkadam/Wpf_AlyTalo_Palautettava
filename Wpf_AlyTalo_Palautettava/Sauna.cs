using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_AlyTalo_Palautettava
{
    class Sauna
    {
        public Boolean switched { get; set; }
        public double lampotila { get; set; }



        public void SaunaPaalle()
	    {
            switched = true;


	    }
        public void SaunaPois()
        {
            switched = false;

        }


    }
}

