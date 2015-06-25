using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducentKonsument {
    class Program {
        static void Main(string[] args) {
            var producentKonsument = new PK();
            Console.WriteLine(producentKonsument.HasloKonsumenta);
            while(true) {
                var tasks = new List<Task>();
                tasks.Add(Task.Run(() => {
                    producentKonsument.producent();
                }));
                Console.WriteLine(producentKonsument.haslo);
                tasks.Add(Task.Run(() => {
                    producentKonsument.konsument();
                }));
                tasks.Add(Task.Delay(100));
                Task.WaitAll(tasks.ToArray());
                //Console.ReadKey();
                if(producentKonsument.koniec())
                    break;
            }
            Console.ReadKey();
        }
    }
    public class PK {
        const int n = 4;
        Queue<String> bufor;
        bool zakoncz;
        int buforMax;
        const string alfabet = "abcd";
        const string koniecHasel = "----";
        public string haslo;
        private string hasloKonsumenta;
        const int maxLiczbaHasel=100;
        int licznikHasel;

        public string HasloKonsumenta {
            get { return hasloKonsumenta; }
            private set { hasloKonsumenta = value; }
        }

        public PK() {
            Random random = new Random();
            haslo = "";
            buforMax = 1;
            bufor = new Queue<string>();
            zakoncz = false;
            hasloKonsumenta = new string(Enumerable.Repeat(alfabet, n).Select(
                s => s[random.Next(alfabet.Length)]).ToArray());
            licznikHasel = 0; 
        }
        public void generuj() {
            if(licznikHasel >= maxLiczbaHasel)
                haslo = koniecHasel;
            else {
                Random random = new Random();
                haslo = new string(Enumerable.Repeat(alfabet, n).Select(
                    s => s[random.Next(alfabet.Length)]).ToArray());
            }
        }
        bool buforpusty() {
            if(bufor.Count == 0)
                return true;
            else
                return false;
        }
        bool buforpelny() {
            if(bufor.Count == buforMax)
                return true;
            else
                return false;
        }
        public void producent() {
            if(!buforpelny()) {
                generuj();
                lock(this) {
                    bufor.Enqueue(haslo);
                    ++licznikHasel;
                }
            }
        }
        public void konsument() {
            if(!buforpusty()) {
                lock(this) {
                    var tmp = bufor.Dequeue();
                    if(hasloKonsumenta.Equals(tmp)) {
                        Console.WriteLine("Trafiony!\n{0} == {1}", HasloKonsumenta, tmp);
                        zakoncz = true;
                    }
                    else if(tmp.Equals(koniecHasel)) {
                        Console.WriteLine("Koniec Hasel. Program zakonczony");
                        zakoncz = true;
                    }
                }
            }
        }
        public bool koniec() {
            if(zakoncz)
                return true;
            return false;
        }

        string permutacja() {
            return "";
        }
    }
}
