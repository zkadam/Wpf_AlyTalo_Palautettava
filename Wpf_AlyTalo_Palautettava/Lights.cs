using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_AlyTalo_Palautettava
{
    class Lights
    {
        public Boolean Switched { get; set; }
        public string Dimmer { get; set; }


        public void ValotPaalle() //Tämä on metodi
        {
            //------//
            Switched = true;
            Dimmer = "1";
        }

        public void ValotPois()
        {
            Switched = false;
            Dimmer = "0";
        }
    }
}
