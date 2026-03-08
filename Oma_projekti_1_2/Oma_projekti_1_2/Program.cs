using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oma_projekti_1_2
{
    class FileService   // FileService: käsittelee tiedostojen lukemisen ja kirjoittamisen
    {
        public void Luo_Tiedosto(string minun_file)  // Luo tiedosto, jos sitä ei vielä ole
        {
            if (!File.Exists(minun_file))
            {
                using (FileStream fs = File.Create(minun_file))
                {
                }
            }
        }
        public List<string> Tiedostosta_Listaan(string minun_file)  // Lue tiedosto ja palauta rivit listana
        {
            List<string> merkkijonot = new List<string>();
            Luo_Tiedosto(minun_file);
            try
            {
                using (StreamReader reader = new StreamReader(minun_file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        merkkijonot.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Virhe: {ex.Message}");
            }
            return merkkijonot;
        }
        public void Listasta_Tiedostoon(List<string> merkkijonot, string minun_file)  // Kirjoita lista tiedostoon
        {
            Luo_Tiedosto(minun_file);
            try
            {
                using (StreamWriter writer = new StreamWriter(minun_file))
                {
                    foreach (string line in merkkijonot)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Virhe: {ex.Message}");
            }
        }

    }
    class RektioService  // RektioService: hallitsee rektioita 
    {
        private List<string> rektiot_list;
        private FileService file_service;
        private string tiedosto_name;

        public RektioService(string rektiot_file)  // Konstruktorissa luetaan rektiot tiedostosta
        {
            tiedosto_name = rektiot_file;
            file_service = new FileService();
            rektiot_list = file_service.Tiedostosta_Listaan(tiedosto_name);
        }

        public List<string> Saada_Lista()  // Palauttaa koko listan
        {
            return rektiot_list;
        }

        public void Lisa_Tiedostoon(string uusi_rektio, string uusi_esimerkki)  // Lisää uusi rektio listaan ja tiedostoon
        {
            string rivi = $"{uusi_rektio.ToUpper()}:{uusi_esimerkki.ToLower()};";
            rivi = rivi.Replace(" ", "");
            if (rektiot_list.Contains(rivi))
            {
                Console.WriteLine($"{rivi} on jo listassa");
            }
            else
            {
                rektiot_list.Add(rivi);
                Console.WriteLine($"\u001b[93mLisättiin uusi rektio: {rivi}\u001b[0m");
            }
            file_service.Listasta_Tiedostoon(rektiot_list, tiedosto_name);
        }

        public void Pois_Tiedostosta(string pois_rektio_ja_esimerkki)  // Poistaa reaktion listasta ja tiedostosta
        {
            if (rektiot_list.Remove(pois_rektio_ja_esimerkki))
            {
                Console.WriteLine($"\u001b[31mPoistetiin rektio: {pois_rektio_ja_esimerkki}\u001b[0m");
            }
            else
            {
                Console.WriteLine($"{pois_rektio_ja_esimerkki} ei ole listassa");
            }
            file_service.Listasta_Tiedostoon(rektiot_list, tiedosto_name);
        }
    }
    class SanaService  // SanaService: hallitsee sanoja, sanaluokkia ja niiden rektioita
    {
        private List<string> sanat_list;
        private FileService file_service;
        private string tiedosto_name;

        public SanaService(string sanat_file)  // Konstruktorissa luetaan sanat tiedostosta
        {
            tiedosto_name = sanat_file;
            file_service = new FileService();
            sanat_list = file_service.Tiedostosta_Listaan(tiedosto_name);
        }

        public List<string> Saada_Lista()  // Palauttaa listan kaikista sanoista
        {
            return sanat_list;
        }

        public void Lisa_Tiedostoon(string uusi_sana, string uusi_sanaluokka, string rektio_ja_esimerkki, string sana_ja_esimerkki, string malli_ja_käännös)
        // Lisää uusi sana listaan ja tiedostoon
        {
            string rivi_osa = $"{uusi_sana.ToUpper()}|{uusi_sanaluokka.ToLower()}|{rektio_ja_esimerkki}|";
            rivi_osa = rivi_osa.Replace(" ", "");
            string rivi_koko = rivi_osa + sana_ja_esimerkki + "|" + malli_ja_käännös;
            List<string> rivit_osat = new List<string>();
            foreach (string line in sanat_list)
            {
                string[] osa = line.Split('|');
                string sana_luokka_rektio = osa[0] + "|" + osa[1] + "|" + osa[2] + "|";
                rivit_osat.Add(sana_luokka_rektio);
            }
            if (rivit_osat.Contains(rivi_osa))
            {
                if (sanat_list.Contains(rivi_koko))
                {
                    Console.WriteLine("\u001b[31mTämä sana on jo listassa, jossa on sama rektio ja sama esimerkki.\u001b[0m");
                }
                else
                {
                    Console.WriteLine("\u001b[31mTämä sana on jo listassa, jossa on sama rektio.\u001b[0m");
                }
            }
            else
            {
                sanat_list.Add(rivi_koko);
                Console.WriteLine($"\u001b[93mLisättiin uusi sana: {rivi_koko}\u001b[0m");
            }
            file_service.Listasta_Tiedostoon(sanat_list, tiedosto_name);
        }

        public void Pois_Tiedostosta(string pois_sana_sanaluokka_rektio)  // Poistaa sanan listasta ja tiedostosta
        {
            if (sanat_list.Remove(pois_sana_sanaluokka_rektio))
            {
                Console.WriteLine($"\u001b[31mPoistetiin sana: {pois_sana_sanaluokka_rektio}\u001b[0m");
            }
            else
            {
                Console.WriteLine($"{pois_sana_sanaluokka_rektio} ei ole listassa");
            }
            file_service.Listasta_Tiedostoon(sanat_list, tiedosto_name);
        }
        public string Sanan_Tarkistus (string name_sana)  // Palauttaa sanan sanaluokan
        {
            string sanaluokka = "";
            name_sana = name_sana.ToUpper();
            foreach (string s in sanat_list)
            {
                if (s.ToUpper().StartsWith(name_sana))
                {
                    string[] osa = s.Split('|');
                    sanaluokka = osa[1];
                }
            }
            return sanaluokka;
        }
        public List<string> Ryhma_OikeaRek_SanEs(string rivi_satunainen_sana)  // Palauttaa oikeat rektiot ja esimerkit sanalle
        {
            string[] osa = rivi_satunainen_sana.Split('|');
            string sana = osa[0];
            List<string> ryhma_Rek_SanEs = new List<string>();

            foreach (string s in sanat_list)
            {
                if (s.ToUpper().StartsWith(sana))
                {
                    string[] osat = s.Split('|');
                    string rek_sanEs = osat[2] + "|" + osat[3];
                    ryhma_Rek_SanEs.Add(rek_sanEs);
                }
            }
            return ryhma_Rek_SanEs;
        }
        public List<string> Ryhma_OikeaRek(List<string> ryhma_oikRek_sanEs)  // Palauttaa vain oikeat rektiot listana
        {
            List<string> ryhma_oikRek = new List<string>();

            foreach (string s in ryhma_oikRek_sanEs)
            {
                string[] osat = s.Split('|');
                ryhma_oikRek.Add(osat[0]);
            }
            return ryhma_oikRek;
        }
    }
    class QuizEngine  // QuizEngine: pelin logiikka ja satunnaisten sanojen valinta
    {
        private SanaService sana_service;
        private RektioService rektio_service;
        private Random random;

        public QuizEngine(string rektiot_file, string sanat_file)
        {
            sana_service = new SanaService(sanat_file);
            rektio_service = new RektioService(rektiot_file);
            random = new Random();
        }

        public List<string> Saada_Lista_sanat()  // Palauttaa listan kaikista sanoista
        {
            return sana_service.Saada_Lista();
        }

        public string Rand_Sana_Sanaluokan_mukaan(string sanaluokka)  // Valitsee satunnaisen sanan tietyn sanaluokan mukaan
        {
            List<string> kaikki_sanat = sana_service.Saada_Lista();
            List<string> sanat_sanaluoka = new List<string>();

            foreach (string rivi in kaikki_sanat)
            {
                string[] osa = rivi.Split('|');
                if (osa[1] == sanaluokka)
                {
                    sanat_sanaluoka.Add(rivi);
                }
            }
            if (sanat_sanaluoka.Count == 0)
            {
                Console.WriteLine("Tietokannassa ei ole vielä sanoja tällä sanaluokalla.");
                return "";
            }
            else
            {
                int rand_sana_index = random.Next(0, sanat_sanaluoka.Count());
                return sanat_sanaluoka[rand_sana_index];
            }
        }
        public string Sana_osa_0(string rivi)  // Palauttaa sanan osan 0 (varsinaisen sanan)
        {
            string[] osa = rivi.Split('|');
            return osa[0];
        }
        public string Oikea_Rektio(string rivi)  // Palauttaa oikean reaktion sanalle
        {
            string[] osa = rivi.Split('|');
            return osa[2];
        }

        public List<string> Vaarat_Rektiot(List<string> ryhma_oikRek, int maara)  // Luo listan vääristä vaihtoehdoista
        {
            List<string> rektiot_list = new List<string>(rektio_service.Saada_Lista());

            foreach (string line in ryhma_oikRek)
            {
                rektiot_list.Remove(line);

                if (rektiot_list.Count < maara)
                {
                    Console.WriteLine("Ei ole tarpeeksi rektioita virheellisiin vastauksiin.");
                    return new List<string>();
                }
            }

            List<string> vaarat_rektiot = new List<string>();
            for (int i = 0; i < maara; i++)
            {
                int rand_sana_index = random.Next(0, rektiot_list.Count());
                string valittu = rektiot_list[rand_sana_index];
                vaarat_rektiot.Add(valittu);
                rektiot_list.Remove(valittu);
            }
            return vaarat_rektiot;
        }

        public List<string> Vaihtoehtoot(string rivi, List<string> ryhma_oikRek_sanEs, int vaarat_maara)  // Luo vaihtoehdot kysymykseen (oikea + väärät)
        {
            string oikea_rektio = Oikea_Rektio(rivi);
            List<string> ryhma_oikRek = sana_service.Ryhma_OikeaRek(ryhma_oikRek_sanEs);
            List<string> vaarat_rektiot = Vaarat_Rektiot(ryhma_oikRek, vaarat_maara);
            List<string> vaihtoehtoot = new List<string>(vaarat_rektiot);
            vaihtoehtoot.Add(oikea_rektio);
            vaihtoehtoot = vaihtoehtoot.OrderBy(x => random.Next()).ToList();
            return vaihtoehtoot;
        }

        public string Valinta_sanaluokka(int numero_sanaluokka)  // Palauttaa sanaluokan numeron mukaan
        {
            string name_sanaluokka = "";
            switch (numero_sanaluokka)
            {
                case 1:
                    name_sanaluokka = "verbi";
                    break;
                case 2:
                    name_sanaluokka = "adjektiivi";
                    break;
                case 3:
                    name_sanaluokka = "substantiivi";
                    break;
                case 4:
                    name_sanaluokka = "postpositio";
                    break;
                case 5:
                    name_sanaluokka = "prepositio";
                    break;
            }
            return name_sanaluokka;
        }
        public string Syotetty_Rektio(string syotetty_rektio)  // Tarkistaa syötetyn reaktion ja palauttaa sen rivin
        {
            syotetty_rektio = syotetty_rektio.ToUpper();
            List<string> rektiot_list = rektio_service.Saada_Lista();

            string rektio_ja_esimerkki = "";
            foreach (string s in rektiot_list)
            {
                if (s.ToUpper().StartsWith(syotetty_rektio))
                {
                    rektio_ja_esimerkki = s;
                }
            }
            if (rektio_ja_esimerkki == "")
            {
                Console.WriteLine("Rektio-tiedosta ei ole tällaista rektioa. Lisää tämä rektio ensin rektio-tiedostoon.");
            }
            return rektio_ja_esimerkki;
        }
        public void Syote_Rek_SanEs_Lisa_Tiedostoon(string name_sana, string name_sanaluokka)  // Kysyy käyttäjältä esimerkin ja lisää sanan tiedostoon
        {
            Console.WriteLine("Syötä esimerkki sanan käytöstä:");
            string sana_ja_esimerkki = Console.ReadLine();
            Console.WriteLine("Syötä malli (esimerkki sanan käytöstä):");
            string malli_ja_käännös = Console.ReadLine();
            Console.WriteLine("Syötä rektio:");
            string name_rektio = Console.ReadLine();
            string rektio_ja_esimerkki = Syotetty_Rektio(name_rektio);
            if (rektio_ja_esimerkki != "")
            {
                sana_service.Lisa_Tiedostoon(name_sana, name_sanaluokka, rektio_ja_esimerkki, sana_ja_esimerkki, malli_ja_käännös);
            }
        }
    }
    class Tulosta  // Tulosta: konsolin tulostukset ja valikot
    {
        private SanaService sana_service;
        private RektioService rektio_service;

        public Tulosta(string rektiot_file, string sanat_file)
        {
            sana_service = new SanaService(sanat_file);
            rektio_service = new RektioService(rektiot_file);
        }

        public void Päävalikko()  // Päävalikon tulostus
        {
            Console.WriteLine();
            Console.WriteLine("\u001b[32mValitse toiminto:");
            Console.WriteLine("1. Pelaa");
            Console.WriteLine("2. Lisää sana");
            Console.WriteLine("3. Poista sana");
            Console.WriteLine("4. Lisää rektio");
            Console.WriteLine("5. Poista rektio");
            Console.WriteLine("6. Poistu\u001b[0m");
            Console.WriteLine();
        }
        public void Virheellinen_syöte()  // Virheellinen syöte
        {
            Console.WriteLine();
            Console.WriteLine("\u001b[31mVirheellinen syöte. \u001b[0m");
            Console.WriteLine();
        }
        public void Sanaluokan_valinta()  // Valikko sanaluokan valintaan
        {
            Console.WriteLine();
            Console.WriteLine("\u001b[32mVälitse sanaluokka:");
            Console.WriteLine("1. verbi");
            Console.WriteLine("2. adjektiivi");
            Console.WriteLine("3. substantiivi");
            Console.WriteLine("4. postpositio");
            Console.WriteLine("5. prepositio\u001b[0m");
        }
        public void Kyllä_Ei()  // Kyllä / Ei valikko
        {
            Console.WriteLine("\u001b[36mValitse toiminto:\u001b[0m");
            Console.WriteLine("1. \u001b[31mKyllä\u001b[0m");
            Console.WriteLine("2. \u001b[93mEi\u001b[0m");
        }
        public void Jatkamisvalikko()  // Jatkamisvalikko pelin jälkeen
        {
            Console.WriteLine("\u001b[32mVälitse toiminto:");
            Console.WriteLine("1. Pysyä tässä valikossa");
            Console.WriteLine("2. Palaa päävalikkoon");
            Console.WriteLine("3. Poistu\u001b[0m");
        }
        public void Peli_Valikko()  // Pelivalikko
        {
            Console.WriteLine();
            Console.WriteLine("\u001b[32mValitse sanaluokka:");
            Console.WriteLine("1. Verbin rektio");
            Console.WriteLine("2. Adjektiivin rektio");
            Console.WriteLine("3. Substantiivin rektio");
            Console.WriteLine("4. Postposition rektio");
            Console.WriteLine("5. Preposition rektio");
            Console.WriteLine("6. Poistu \u001b[0m");
            Console.WriteLine();
        }
        public void Vaihtoehtoot(string ensimäinen, string toinen, string kolmas, string neljäs)  // Tulostaa vaihtoehdot kysymykseen
        {
            Console.WriteLine();
            Console.WriteLine("\u001b[36mValitse rektio: \u001b[0m");
            Console.WriteLine();
            Console.WriteLine($"1. \u001b[33m{ensimäinen}\u001b[0m");
            Console.WriteLine($"2. \u001b[33m{toinen}\u001b[0m");
            Console.WriteLine($"3. \u001b[33m{kolmas}\u001b[0m");
            Console.WriteLine($"4. \u001b[33m{neljäs}\u001b[0m");
        }
        public void Sanat_List()   // Tulostaa listan olemassa olevista sanoista
        {
            Console.WriteLine("\u001b[36mKatso lista sanoista, jotka ovat jo olemassa:\u001b[0m");
            List<string> sanat_list = sana_service.Saada_Lista();
            sanat_list.ForEach(x => Console.WriteLine(x));
        }
        public void Rektiot_List()  // Tulostaa listan olemassa olevista rektioista
        {
            Console.WriteLine("\u001b[36mKatso lista rektioista, jotka ovat jo olemassa:\u001b[0m");
            List<string> rektiot_list = rektio_service.Saada_Lista();
            rektiot_list.ForEach(x => Console.WriteLine(x));
        }
    }
    class Input  // Input: käsittelee käyttäjän syötteen
    {
        public int Kysy_Numero(int min, int max)  // Kysyy numeron tietyltä väliltä
        {
            Console.WriteLine();
            Console.WriteLine($"Syötä valinta väliltä {min}-{max}: ");
            Console.WriteLine("**************************");
            bool validInput = int.TryParse(Console.ReadLine(), out int choice);
            if ((validInput) && (choice >= min) && (choice <= max))
            {
                return choice;
            }
            else
            {
                return 0;
            }
        }
    }
    class Peli  // Peli: yhdistää QuizEngine ja Tulosta, hallitsee pelin kulun
    {
        private QuizEngine quizEngine;
        private Tulosta tulosta;
        private Input input;

        public Peli(string rektiot_file, string sanat_file)  
        {
            quizEngine = new QuizEngine(rektiot_file, sanat_file);
            input = new Input();
            tulosta = new Tulosta(rektiot_file, sanat_file);
        }
        public int Pelaa(string rivi_satunnainen, List<string> ryhma_oikRek_sanEs)  // Pelin pääsilmukka yhdelle sanalle
        {
            int m_1 = 0;
            string[] osa = rivi_satunnainen.Split('|');
            Console.WriteLine($"\u001b[35m      {osa[0]}\u001b[0m");
            Console.WriteLine($"   {osa[4]}");
            string oikeus_vastaus = osa[2];
            string vastaus_oikRek_sanEs = osa[2] + "|" + osa[3];
            List<string> vaihtoehtoot = quizEngine.Vaihtoehtoot(rivi_satunnainen, ryhma_oikRek_sanEs, 3);
            tulosta.Vaihtoehtoot(vaihtoehtoot[0], vaihtoehtoot[1], vaihtoehtoot[2], vaihtoehtoot[3]);
            Console.WriteLine();
            Console.WriteLine("***Jos et halua jatkaa, kirjoita mikä tahansa merkki.");
            int choice_vaihtoehto = input.Kysy_Numero(1, 4);
            string minun_vaihtoehto = "";
            if (choice_vaihtoehto != 0)
            {
                switch (choice_vaihtoehto)
                {
                    case 1:
                        minun_vaihtoehto = vaihtoehtoot[0];
                        break;
                    case 2:
                        minun_vaihtoehto = vaihtoehtoot[1];
                        break;
                    case 3:
                        minun_vaihtoehto = vaihtoehtoot[2];
                        break;
                    case 4:
                        minun_vaihtoehto = vaihtoehtoot[3];
                        break;
                }
                if (minun_vaihtoehto == oikeus_vastaus)
                {
                    Console.WriteLine();
                    Console.WriteLine("\u001b[93mOikein! ");
                    Console.Write($"Oikeus vastaus: {oikeus_vastaus}\u001b[0m");
                    Console.WriteLine($"\u001b[35m  {osa[3]}\u001b[0m");
                    ryhma_oikRek_sanEs.Remove(vastaus_oikRek_sanEs);
                    Console.WriteLine("\u001b[36mToisia rektioja:");
                    ryhma_oikRek_sanEs.ForEach(x => Console.WriteLine(x));
                    Console.WriteLine("\u001b[0m");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\u001b[31mVäärin \u001b[0m ");
                    Console.Write($"\u001b[93mOikeus vastaus: {oikeus_vastaus}\u001b[0m");
                    Console.WriteLine($"\u001b[35m  {osa[3]}\u001b[0m");
                    ryhma_oikRek_sanEs.Remove(vastaus_oikRek_sanEs);
                    Console.WriteLine("\u001b[36mToisia rektioja:");
                    ryhma_oikRek_sanEs.ForEach(x => Console.WriteLine(x));
                    Console.WriteLine("\u001b[0m");
                }
            }
            else
            {
                m_1 = 1;
            }
            return m_1;
        }
        public int Jatka()  // Jatkamisvalikko pelin jälkeen
        {
            int m_1 = 3;
            while (m_1 == 3)
            {
                tulosta.Jatkamisvalikko();
                int choice_jatkaa = input.Kysy_Numero(1, 3);
                if (choice_jatkaa != 0)
                {
                    switch (choice_jatkaa)
                    {
                        case 1:
                            m_1 = 0;
                            break;
                        case 2:
                            m_1 = 1;
                            break;
                        case 3:
                            m_1 = 2;
                            break;
                    }
                }
                else
                {
                    tulosta.Virheellinen_syöte();
                }
            }
            return m_1;
        }
    }

    internal class Program  // Program: sovelluksen käynnistys
    {
        static void Main(string[] args)
        {
            // Luo kaikki tarvittavat objektit ja käynnistää päävalikon
            QuizEngine quizEngine = new QuizEngine("rektiot.txt", "sanat.txt");
            SanaService sanaService = new SanaService("sanat.txt");
            RektioService rektioService = new RektioService("rektiot.txt");
            Tulosta tulosta = new Tulosta("rektiot.txt", "sanat.txt");
            Input input = new Input();
            Peli peli = new Peli("rektiot.txt", "sanat.txt");

            // Pääsilmukka päävalikolle
            int m_1 = 1;  // Hallitsee silmukan jatkamisen
            while (m_1 == 1)  
            {   
                tulosta.Päävalikko();   // Tulostaa päävalikon
                int choice_päävalikko = input.Kysy_Numero(1, 6);   // Kysyy käyttäjältä valintaa

                if (choice_päävalikko != 0)
                {
                    switch (choice_päävalikko)
                    {
                        case 1:  // Pelaa peli
                            {
                                m_1 = 0;
                                while (m_1 == 0)
                                { 
                                Console.WriteLine("\u001b[31mPelissä on vain yksi oikea vastaus ja");
                                Console.WriteLine("loput 3 ovat vääriä.\u001b[0m");
                                tulosta.Peli_Valikko();  // Näyttää sanaluokkavalikon
                                int choice_sanaluokka = input.Kysy_Numero(1, 6);
                                if (choice_sanaluokka != 0)
                                {
                                    switch (choice_sanaluokka)
                                    {
                                        case 1:  // Verbit
                                            while (m_1 == 0)
                                            {
                                                // Valitaan satunnainen verbi
                                                string rivi_satunnainen_verbi = quizEngine.Rand_Sana_Sanaluokan_mukaan("verbi");
                                                List<string> ryhma_oikRek_sanEs = sanaService.Ryhma_OikeaRek_SanEs(rivi_satunnainen_verbi);
                                                // Käynnistetään peli
                                                m_1 = peli.Pelaa(rivi_satunnainen_verbi, ryhma_oikRek_sanEs);
                                                if (m_1 == 1)
                                                {
                                                    m_1 = peli.Jatka();  // Kysytään jatketaanko
                                                }
                                            }
                                            break;
                                        case 2:  // Adjektiivit
                                            while (m_1 == 0)
                                            {
                                                string rivi_satunnainen_adjektiivi = quizEngine.Rand_Sana_Sanaluokan_mukaan("adjektiivi");
                                                List<string> ryhma_oikRek_sanEs = sanaService.Ryhma_OikeaRek_SanEs(rivi_satunnainen_adjektiivi);
                                                m_1 = peli.Pelaa(rivi_satunnainen_adjektiivi, ryhma_oikRek_sanEs);
                                                if (m_1 == 1)
                                                {
                                                    m_1 = peli.Jatka();
                                                }
                                            }
                                            break;
                                        case 3:  // Substantiivit
                                            while (m_1 == 0)
                                            {
                                                string rivi_satunnainen_substantiivi = quizEngine.Rand_Sana_Sanaluokan_mukaan("substantiivi");
                                                List<string> ryhma_oikRek_sanEs = sanaService.Ryhma_OikeaRek_SanEs(rivi_satunnainen_substantiivi);
                                                m_1 = peli.Pelaa(rivi_satunnainen_substantiivi, ryhma_oikRek_sanEs);
                                                if (m_1 == 1)
                                                {
                                                    m_1 = peli.Jatka();
                                                }
                                            }
                                            break;
                                        case 4:  // Postpositiot
                                            while (m_1 == 0)
                                            {
                                                string rivi_satunnainen_postpositio = quizEngine.Rand_Sana_Sanaluokan_mukaan("postpositio");
                                                List<string> ryhma_oikRek_sanEs = sanaService.Ryhma_OikeaRek_SanEs(rivi_satunnainen_postpositio);
                                                m_1 = peli.Pelaa(rivi_satunnainen_postpositio, ryhma_oikRek_sanEs);
                                                if (m_1 == 1)
                                                {
                                                    m_1 = peli.Jatka();
                                                }
                                            }
                                            break;
                                        case 5:  // Prepositiot
                                            while (m_1 == 0)
                                            {
                                                string rivi_satunnainen_prepositio = quizEngine.Rand_Sana_Sanaluokan_mukaan("prepositio");
                                                List<string> ryhma_oikRek_sanEs = sanaService.Ryhma_OikeaRek_SanEs(rivi_satunnainen_prepositio);
                                                m_1 = peli.Pelaa(rivi_satunnainen_prepositio, ryhma_oikRek_sanEs);
                                                if (m_1 == 1)
                                                {
                                                    m_1 = peli.Jatka();
                                                }
                                            }
                                            break;
                                        case 6:  // Poistu
                                            m_1 = 2;
                                            break;
                                    }
                                }
                                else
                                {
                                    tulosta.Virheellinen_syöte();  // Virheellinen valinta
                                        m_1 = 0;  // Takaisin sanaluokkavalikkoon
                                    }
                                }
                            }
                            break;
                        case 2:  // Lisää uusi sana
                            {
                                m_1 = 0;
                                tulosta.Sanat_List();  // Näyttää olemassa olevat sanat

                                while (m_1 == 0)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("\u001b[36mSyötä uusi sana:\u001b[0m");
                                    Console.WriteLine("***Jos et halua jatkaa, syötä 3 pistettä.");
                                    string name_sana = Console.ReadLine();
                                    if ((name_sana != "...")&&(name_sana != "")) 
                                    {
                                        string sanaluokka = sanaService.Sanan_Tarkistus(name_sana);
                                        if (sanaluokka != "")
                                        {
                                            // Sana on jo listassa
                                            Console.WriteLine($"Tämä sana on jo listassa. \u001b[36mSanaluokka on {sanaluokka}\u001b[0m");
                                            quizEngine.Syote_Rek_SanEs_Lisa_Tiedostoon(name_sana, sanaluokka);
                                        }
                                        else
                                        {
                                            tulosta.Sanaluokan_valinta();  // Valitaan sanaluokka
                                            int choice_sanaluokka = input.Kysy_Numero(1, 5);
                                            if (choice_sanaluokka != 0)
                                            {
                                            string name_sanaluokka = quizEngine.Valinta_sanaluokka(choice_sanaluokka);
                                            quizEngine.Syote_Rek_SanEs_Lisa_Tiedostoon(name_sana, name_sanaluokka);
                                            }
                                            else
                                            {
                                                tulosta.Virheellinen_syöte();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        m_1 = 1;  // Lopettaa lisäyksen
                                    }
                                }
                            }
                            break;
                        case 3:  // Poista sana listasta
                            {
                                tulosta.Sanat_List();  // Näyttää kaikki sanat

                                List<string> sanat_list = sanaService.Saada_Lista();  // Haetaan sanat muistilistaan
                                Console.WriteLine("\u001b[36mSyötä sana, jonka haluat poistaa:\u001b[0m");
                                string name_sana = Console.ReadLine();
                                if (name_sana != "")
                                {
                                    name_sana = name_sana.ToUpper();  // Muutetaan vertailua varten isoiksi kirjaimiksi
                                    string sana_sanaluokka_rektio = "";
                                    foreach (string s in sanat_list)   // Etsitään listasta rivi, joka alkaa annetulla sanalla
                                    {
                                        if (s.ToUpper().StartsWith(name_sana))
                                        {
                                            sana_sanaluokka_rektio = s;
                                        }
                                    }
                                    if (sana_sanaluokka_rektio != "")
                                    {
                                        // Näytetään poistettava rivi käyttäjälle
                                        Console.WriteLine($"Katso rivi, jonka haluat poistaa: \u001b[31m{sana_sanaluokka_rektio}\u001b[0m");
                                        tulosta.Kyllä_Ei();  // Varmistuskysymys
                                        int choice_kyllä_ei = input.Kysy_Numero(1, 2);

                                        if (choice_kyllä_ei != 0)
                                        {
                                            switch (choice_kyllä_ei)
                                            {
                                                case 1:  // Poistetaan sana tiedostosta
                                                    sanaService.Pois_Tiedostosta(sana_sanaluokka_rektio);
                                                    break;
                                                case 2:  // Perutaan poisto
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            tulosta.Virheellinen_syöte();
                                        }
                                    }
                                    else
                                    {  // Sana ei löytynyt listasta
                                        Console.WriteLine($"{name_sana} ei ole listassa");
                                    }
                                }
                            }
                            break;
                        case 4:  // Lisää uusi rektio
                            {
                                m_1 = 0;
                                tulosta.Rektiot_List();  // Näyttää olemassa olevat rektiot
                                
                                // Silmukka mahdollistaa useiden rektioiden lisäämisen
                                while (m_1 == 0)
                                {
                                    Console.WriteLine("\u001b[36mSyötä uusi rektio:\u001b[0m");
                                    Console.WriteLine("***Jos et halua jatkaa, syötä 3 pistettä.");
                                    string name_rektio = Console.ReadLine();
                                    if (name_rektio != "...")
                                    {   // Kysytään esimerkkilause rektiolle
                                        Console.WriteLine("Syötä rektion esimerkki:");
                                        string name_rektion_esimerkki = Console.ReadLine();
                                        rektioService.Lisa_Tiedostoon(name_rektio, name_rektion_esimerkki);
                                    }
                                    else m_1 = 1;  // Lopetetaan lisäys
                                }
                            }
                            break;
                        case 5:  // Poista rektio listasta
                            {
                                tulosta.Rektiot_List();  // Näyttää kaikki rektiot
                                List<string> rektiot_list = rektioService.Saada_Lista();  // Haetaan lista
                                Console.WriteLine("\u001b[36mSyötä rektio, jonka haluat poistaa:\u001b[0m");
                                string name_rektio = Console.ReadLine();
                                if (name_rektio != "")
                                {
                                    name_rektio = name_rektio.ToUpper();
                                    string rektio_ja_esimerkki = "";
                                    foreach (string s in rektiot_list)  // Etsitään annettua rektiota vastaava rivi
                                    {
                                        if (s.ToUpper().StartsWith(name_rektio))
                                        {
                                            rektio_ja_esimerkki = s;
                                        }
                                    }
                                    if (rektio_ja_esimerkki != "")  // Näytetään poistettava rivi ja pyydetään vahvistus
                                    {
                                        Console.WriteLine($"Katso rivi, jonka haluat poistaa: \u001b[31m{rektio_ja_esimerkki}\u001b[0m");
                                        tulosta.Kyllä_Ei();
                                        int choice_kyllä_ei = input.Kysy_Numero(1, 2);

                                        if (choice_kyllä_ei != 0)
                                        {
                                            switch (choice_kyllä_ei)
                                            {
                                                case 1:  // Poistetaan rektio tiedostosta
                                                    rektioService.Pois_Tiedostosta(rektio_ja_esimerkki);
                                                    break;
                                                case 2:  // Perutaan poisto
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            tulosta.Virheellinen_syöte();
                                        }
                                    }
                                    else
                                    {  // Rektio ei löytynyt
                                        Console.WriteLine($"{name_rektio} ei ole listassa");
                                    }
                                }
                            }
                            break;
                        case 6:  // Lopettaa ohjelman pääsilmukan
                            m_1 = 2;
                            break;
                    }
                }
                else
                {
                    tulosta.Virheellinen_syöte();
                    m_1 = 1;
                }
            }

        }
    }
}
