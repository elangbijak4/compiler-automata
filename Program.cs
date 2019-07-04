using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Flow dari program ini rencananya adalah:
            * User membuka aplikasi
            * Aplikasi lalu menjalanakan register bahasa
            * Meminta user meamsukkan bahasa yang ingin digunakan berupa bahasa yang gramatikanya dinyatakan ke dalam mesin Turing
            * User diminta memasukkan nama bahasa, himpunan state, state awal, state akhir, himpunan instruksi
            * Aplikasi lalu melakukan enkoding semua state dan instruksi dan menyimpannya ke dalam array dinamis milik objek dari 
            * Class Register bahasa. Setelah semua state dan instruksi dimasukkan, aplikasi mengumpulakn semua jenis simbol dari dalam instruksi
            * yang dimasukkan oleh user. Kumpulan simbol ini menjadi alphabet yang digunakan untuk memeriksa token yang dimasukkan user.
            * Pemeriksaan token yaitu aplikasi memeriksa inputan user dengan cara:
            * Aplikasi memeriksa setiap simbol apakah sesuai dengan semua simbol dalam instruksi?
            * Apakah juga sesuai dengan simbol-simbol khusus milik UTM pada pita1? misal simbol akhir inputan yaitu titik-koma dan simbol spasi
            * antar word yang diinputkan user.
            * Pada rencana lanjut, terdapat pita tambahan pada UTM yang menyimpan automata untuk memeriksa token.
            * Rencana secara umum dari compiler adalah sebagai beririku:
            * Jika sebuah bahasa memiliki n tingkatan grammar, misal tingkat pertama adalah grammar untuk menyusun token (grammar untuk word), tingkat kedua adalah grammar 
            * untuk menyusun kalimat dari kata/word (grammar untuk sentence), tingkatan grammar ketika adalah grammar untuk memeriksa paragraf (grammar untuk paragraf)
            * tingkatan grammar ke empat adalah grammar untuk memeriksa pasal, tingkatan berikut adaah grammar untuk memeriksa bab, 
            * tingkatan berikut adalah grammar untuk memeriksa buku, dan seterusnya....
            * Maka jika ada n tingkatan grammar maka ada n mesin Turing (MT) atau automata yang merepresentasikan tiap-tiap grammar.
            * Ini juga berarti ada n enkoding MT. Ini berarti ada n pita minimal yang harus disediakan oleh UTM untuk memeriksa bahasa tersebut.
            * 
            * Di dalam aplikasi ini, pemeriksaan token tidak menggunakan automata, hanya pemetaan biasa. yaitu pemetaan ke tabel (array) token, itu saja.
            * Tetapi rencananya pada perkembangan lanjut, dapat dibuat menggunakan automata atau mesin Turing.
            * 
            * Setelah pemeriksaan token:
            * Kemudian setelah itu aplikasi lalu menginisialisasi compiler (UTM) mengunakan informasi dari array dinamis tersebut.
            * inisialisasi ini adalah persiapan untuk pemeriksaan grammar pada level kalimat atau sentence.
            * inisialisasi adalah sebuah method yang dimiliki oleh class UTM/compiler.
            * 
            * Setelah inisialisasi selesai, compiler siap menerima input dari user.
            * Setelah user memasukkan input, aplikasi memanggil method simulasi pada class UTM lalu memulai melakukan pemeriksaan grammar pada input user.
            * Ini adalah proses kompilasi atau interpreteasi.
            * 
            * Hasilnya adalah pemetaan ke respon.
            * aplikasi memanggil kelas respon untuk memetakan state akhir hasil running compiler ke respon yang sesuai.
            * 
            * Proses selesai.
            */

            /*
            //Ujicoba input user
            string enkoding;
            Inputclass inputclass;
            inputclass = new Inputclass();
            inputclass.Kalimat="";
            //inputclass.Enkoding();
            enkoding = inputclass.Outputenkoding();
            Console.WriteLine("Ini adalah hasil enkoding input: " + enkoding);
            Console.ReadLine();
            */

            /*
            //Ujicoba kelas profil bahasa
            Language_profil profil;
            profil = new Language_profil();
            profil.Register_bahasa();
            */
            Console.WriteLine("                                                                                           ");
            Console.WriteLine("                                                                                           ");
            Console.WriteLine("                                                                                           ");
            Console.WriteLine("       ####################################################################################");
            Console.WriteLine("       ##                                                                                ##");
            Console.WriteLine("       ##       @                @       @@@@@@@@@   @@@@@@          @       @           ##");
            Console.WriteLine("       ##       @               @        @          @      @        @ @       @          ##");
            Console.WriteLine("       ##       @              @         @          @      @       @   @       @         ##");
            Console.WriteLine("       ##       @             @   @@@@@  @           @    @       @     @       @        ##");
            Console.WriteLine("       ##       @            @   @     @ @@@@@@       @          @       @       @       ##");
            Console.WriteLine("       ##       @            @   @     @ @             @@@@     @@@@@@@@@@@      @       ##");
            Console.WriteLine("       ##       @             @   @@@@@@ @                 @    @         @     @        ##");
            Console.WriteLine("       ##       @              @       @ @          @       @   @         @    @         ##");
            Console.WriteLine("       ##       @               @      @ @           @      @   @         @   @          ##");
            Console.WriteLine("       ##       @@@@@@@@@@@@     @ @@@@  @            @@@@@@    @         @  @           ##");
            Console.WriteLine("       ##                                                                                ##");
            Console.WriteLine("       ##                                  By.                                           ##");
            Console.WriteLine("       ##                              Aslan Alwi                                        ##");
            Console.WriteLine("       ##        Promotor: Dr. Azhari, MT  CoPromotor: Dr. Suprapto, M.Kom               ##");
            Console.WriteLine("       ##          Compiler for Every Language Representated By Automata                 ##");
            Console.WriteLine("       ## This is the Proof of Concept of Theorem 6.1 and Theorem 6.2 from Dissertation  ##");
            Console.WriteLine("       ##        and The Proof of Correctness (PoC) was stated in this Theorems          ##");
            Console.WriteLine("       ##                  Universitas Gadjah Mada Yogyakarta                            ##");
            Console.WriteLine("       ##                                                                                ##");
            Console.WriteLine("       ####################################################################################");
            
            Console.WriteLine("\nPetunjuk singkat:");
            Console.WriteLine("Masukkan terlebih dulu automata dari bahasa anda, meliputi nama bahasa, semua state dan semua instruksi");
            Console.WriteLine("Lalu setelah semua informasi masuk, maka compiler menginisiasi dirinya, dan kemudian bersiap menerima input");
            Console.WriteLine("atau kalimat anda, kemudian input anda diproses sesuai tatabahasa (automata) bahasa anda");

            Console.ReadLine();
            Console.Clear();


            //Uji coba kelas UTM
            UTM mesin;
            mesin = new UTM();

            mesin.Inisialisasi();
            mesin.Simulasi();

            Console.ReadLine();

            

        }
    }
}
