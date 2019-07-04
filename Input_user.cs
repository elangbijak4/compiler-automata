using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace compiler
{
    class Inputclass
    {
        /* Kelas ini memiliki tugas utama adalah untuk menerima input user dari berbagai macam bentuk
         * Baik itu input berbentuk gambar (misal marker atau markerless) atau suara atau sinyal dan sebagainya bentuk
         * Kemudian kelas ini bertugas melakukan enkoding semua bentuk input itu ke bentuk teks alphanumerik sebagai enkoding
         * input untuk dimasukkan ke dalam compiler atau interpreter UTM (class UTM).
         * Pada dasarnya kelas input ini bersifat universal,akan tetapi untuk sementara, untuk PoC dan sebagai prototype,
         * input dianggap juga hanya inputan teks ke konsol. Sehingga enkoding adalah bersifat pemetaan identitas I yaitu I(x)=x
         * .
         * (1).
         * Cara kerja dari kelas ini adalah:
         * Menyediakan array untuk menampung input user (sebenarnya umum, baik alphanumerik atau data citra, audio atau sinyal ddan sebagainya)
         * Menyediakan array untuk menampung hasil enkoding input ke alphanumerik.
         * .
         * (2).
         * Menyediakan method untuk memeriksa sintaks inputan. Untuk sementara adalah memeriksa sintaks teks inputan.
         * .
         * (3).
         * Menyediakan method untuk melakukan proses enkoding input ke alphanumerik.
         * sementara bersifat identitas saja, I(x)=x
         * .
         * (4).
         * Selesai.
         */

        private string inputuser;
        private ArrayList enkodinguser = new ArrayList();

        public string ConvertArrayListToString(ArrayList bufferarray)
        {
            string buffer4 = "";
            foreach (string i in bufferarray)
            {
                buffer4 = buffer4 + i ;
            }
            
            return buffer4;
        }

        private Boolean Cek_sintaks_inputan(string input)
        {
            Regex cek_sintaks = new Regex(@"[A-Za-z0-9\s;]+");
            
            if ((input != cek_sintaks.Match(input).ToString())|(input.Length==0))
            {
                return false;
            }
            else return true;
        }

        private string Perbaiki_input(string input)
        {
            //yang dimaksud memperbaiki input adalah menghilangkan semua space yang dobel atau di awal-akhir kalimat 
            //dan titik-koma dobel dan yang diawal input

            input = input.Trim(' ').TrimStart(';'); //untuk sementara trim sederhana seperti ini

            string[] buffer = input.Split(' ');
            ArrayList buffer2 = new ArrayList();

            //Proses menghilangkan space yang dobel
            foreach (string i in buffer)
            {
                if (i != "")
                {
                    buffer2.Add(i);
                    buffer2.Add(" ");
                }
            }
            input = ConvertArrayListToString(buffer2);
            input=input.Trim(' ');
            
            buffer2 = new ArrayList();
            buffer = null;

            buffer = input.Split(';');


            //Proses menghilangkan dobel titik-koma
            buffer = input.Split(';');
            foreach (string i in buffer)
            {
                if (i != "")
                {
                    buffer2.Add(i);
                    buffer2.Add(";");
                }
            }
            input = ConvertArrayListToString(buffer2);
            //input = input.Trim(';');

            //Console.WriteLine("\nSAYA DISINI 1: {0}\n", input);
            return input;
        }
        
        public void Set_input()
        {
            Boolean ok;
            do
            {
                Console.Write("Silahkan masukkan kalimat anda: ");
                string value = Console.ReadLine();
                value = Perbaiki_input(value); //ini untuk sementara manakala input masih berupa teks, jika bukan teks, perlu dipikirkan apa perbaikan perlu.

                //Cek keabsahan token
                ok = Cek_sintaks_inputan(value);
                if (!ok) Console.WriteLine("Input anda memiliki kesalahan sintaks, terdapat simbol token bukan alphanumerik atau spasi atau titik-koma");

                if (value != "")//sementara kosong gpp, tapi nanti harus ditinjau kelemahan ekspresi ini
                {
                    inputuser = value;
                }
                else ok = false;

            } while (!ok);
        }


        //profil ini hanya untuk dibaca oleh kelas UTM tetapi tidak untuk dirubah
        public ArrayList Enkodingkalimat
        {
            get { return enkodinguser; }
        }
        
        public void Enkoding()
        {
            //kode ini masih simpel karena belum di embed ke sistem AR
            //jadi input hanya berupa teks sehingga sama saja dengan enkodingnya.
            foreach(char i in inputuser)
            {
                enkodinguser.Add(i);
            }
        }
    }
}
