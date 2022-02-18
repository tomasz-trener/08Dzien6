using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P04AplikacjaZawodnicy.Core
{
    public class ZawodnicyRepository : IDostepDoDanych
    {
        public void Dodaj(Zawodnik z)
        {
            PolaczenieZBaza pzb = new PolaczenieZBaza();
            string sql = string.Format("insert into zawodnicy (imie, nazwisko, kraj, data_ur, wzrost,waga) values('{0}', '{1}', '{2}', '{3}', {4}, {5})",
                z.Imie, z.Nazwisko, z.Kraj, z.DataUrodzeniaFormat, z.Wzrost.ToString(), z.Waga.ToString());

            pzb.WykonajPolecenieSQL(sql);
        }

        public void Edytuj(Zawodnik z)
        {
            PolaczenieZBaza pzb = new PolaczenieZBaza();

            string sql = string.Format("update zawodnicy set imie='{0}',nazwisko = '{1}',kraj = '{2}',data_ur='{3}',wzrost={4}, waga={5} where id_zawodnika = {6}",
                z.Imie, z.Nazwisko, z.Kraj, z.DataUrodzeniaFormat, z.Wzrost.ToString(), z.Waga.ToString(), z.Id_zawodnika);

            pzb.WykonajPolecenieSQL(sql);
        }

        public Zawodnik PodajZawodnika(int id)
        {
            PolaczenieZBaza pzb = new PolaczenieZBaza();
            object[][] wynik = pzb.WykonajPolecenieSQL($"SELECT id_zawodnika, id_trenera,imie,nazwisko,kraj,data_ur,wzrost,waga from zawodnicy where id_zawodnika={id}");
            return MapujZawodnik(wynik)[0];
        }

        public void Usun(int id)
        {
            PolaczenieZBaza pzb = new PolaczenieZBaza();
            pzb.WykonajPolecenieSQL("delete zawodnicy where id_zawodnika = " + id);

        }

        public Zawodnik[] WygenerujZawodnikow()
        {
            PolaczenieZBaza pzb = new PolaczenieZBaza();
            object[][] wynik = pzb.WykonajPolecenieSQL("SELECT id_zawodnika, id_trenera,imie,nazwisko,kraj,data_ur,wzrost,waga from zawodnicy");
            return MapujZawodnik(wynik);
        }

        public Zawodnik[] WygenerujZawodnikow(int numerStrony, int wielkoscStrony,string filtr)
        {
            PolaczenieZBaza pzb = new PolaczenieZBaza();
            string sql = $@"SELECT id_zawodnika, id_trenera,imie,nazwisko,kraj,data_ur,wzrost,waga from zawodnicy 
                        where imie like '%{filtr}% '
                        or nazwisko like  '%{filtr}%'
                        or kraj like '%{filtr}%'
                        or data_ur like '%{filtr}%'
                        or wzrost like '%{filtr}%'
                        or waga like '%{filtr}%'
                        ORDER by id_zawodnika
                        OFFSET({numerStrony} - 1) * 5 ROWS
                        FETCH NEXT {wielkoscStrony} ROWS ONLY";

            object[][] wynik = pzb.WykonajPolecenieSQL(sql);
            return MapujZawodnik(wynik);
        }

        public Zawodnik[] WygenerujZawodnikow(string nazwaKolumnySortowanie)
        {
            PolaczenieZBaza pzb = new PolaczenieZBaza();
            object[][] wynik = pzb.WykonajPolecenieSQL("SELECT id_zawodnika, id_trenera,imie,nazwisko,kraj,data_ur,wzrost,waga from zawodnicy order by " + nazwaKolumnySortowanie);
            return MapujZawodnik(wynik);
        }


        public DaneWykresu WygenerujWykres(RodzajDanych rodzajDanych)
        {
            PolaczenieZBaza pzb = new PolaczenieZBaza();
            object[][] wynik = pzb.WykonajPolecenieSQL($"SELECT imie + ' ' + nazwisko, {rodzajDanych} FROM zawodnicy");

            string[] etykiedy = new string[wynik.Length];
            int[] wartosci = new int[wynik.Length];

            for (int i = 0; i < wynik.Length; i++)
            {
                etykiedy[i] = Convert.ToString(wynik[i][0]);
                wartosci[i] = Convert.ToInt32(wynik[i][1]);
            }
            return new DaneWykresu()
            {
                OsX = etykiedy,
                OsY = wartosci
            };


        }

        public Zawodnik[] Filtruj(string filtr)
        {
            PolaczenieZBaza pzb = new PolaczenieZBaza();
            string sql = $@"SELECT id_zawodnika, id_trenera,imie,nazwisko,kraj,data_ur,wzrost,waga
                        from zawodnicy
                        where imie like '%{filtr}% '
                        or nazwisko like  '%{filtr}%'
                        or kraj like '%{filtr}%'
                        or data_ur like '%{filtr}%'
                        or wzrost like '%{filtr}%'
                        or waga like '%{filtr}%'";
            object[][] wynik = pzb.WykonajPolecenieSQL(sql);
            return MapujZawodnik(wynik);
        }


        private Zawodnik[] MapujZawodnik(object[][] wynik)
        {
            Zawodnik[] zawodnicy = new Zawodnik[wynik.Length];
            for (int i = 0; i < zawodnicy.Length; i++)
            {
                Zawodnik z = new Zawodnik();

                z.Id_zawodnika = (int)wynik[i][0];

                if (wynik[i][1] != DBNull.Value)
                    z.Id_trenera = (int)wynik[i][1];

                z.Imie = (string)wynik[i][2];
                z.Nazwisko = (string)wynik[i][3];
                z.Kraj = (string)wynik[i][4];

                if (wynik[i][5] != DBNull.Value)
                    z.DataUrodzenia = (DateTime)wynik[i][5];
                if (wynik[i][6] != DBNull.Value)
                    z.Wzrost = (int)wynik[i][6];
                if (wynik[i][7] != DBNull.Value)
                    z.Waga = (int)wynik[i][7];

                zawodnicy[i] = z;
            }
            return zawodnicy;
        }
    }
}
