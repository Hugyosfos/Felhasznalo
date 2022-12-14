using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Felhasznalo
{
    internal class FELHASZNALO
    {
        private int id;
        private string név;
        private DateTime szulDatum;
        private string profilkep;

        public FELHASZNALO(int id, string név, DateTime szulDatum, string profilkep)
        {
            Id = id;
            Név = név;
            SzulDatum = szulDatum;
            Profilkep = profilkep;
        }

        public int Id { get => id; set => id = value; }
        public string Név { get => név; set => név = value; }
        public DateTime SzulDatum { get => szulDatum; set => szulDatum = value; }
        public string Profilkep { get => profilkep; set => profilkep = value; }

        public override string ToString()
        {
            return "Név" + Név + "Születési idő" + SzulDatum + "ProfilKép" + Profilkep;
        }
    }

   

   
}
