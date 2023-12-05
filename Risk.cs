using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM
{
    public class Risk
    {
        private const double Tout = 8;
        private const double Tin = 16;
        private const double Vout = 1.4;
        private const double Vin = 0.63;
        private const double Ef = 350;
        private const double Ed = 30;
        private const double Bw = 70;
        private const double At = 70;

        public static double CarcinogenicRisk (double ca, double sf, int pop)
        {
            return ((ca * Tout * Vout) + (ca * Tin * Vin)) * Ef * Ed * sf * pop / (Bw * At * 365);
        }

        public static double NonCarcinogenicRisk (double c, double rfc)
        {
            return c / rfc;
        }
    }
}
