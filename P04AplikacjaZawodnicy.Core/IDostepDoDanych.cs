using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P04AplikacjaZawodnicy.Core
{
    // intrefejs to tylko zestawienie sygnatur metod, które koniecznie mają być zaimplementowane przez naszą klasę 
    public interface IDostepDoDanych
    {
        Zawodnik[] WygenerujZawodnikow();

        Zawodnik[] WygenerujZawodnikow(string nazwaKolumnySortowanie);
        void Dodaj(Zawodnik z);
        void Edytuj(Zawodnik z);
        void Usun(int id);
        Zawodnik PodajZawodnika(int id);

        Zawodnik[] Filtruj(string filtr);
        DaneWykresu WygenerujWykres(RodzajDanych rodzajDanych);
        Zawodnik[] WygenerujZawodnikow(int numerStrony, int wielkoscStrony, string filtr);

        //https://github.com/tomasz-trener/08Dzien6
    }
}
