using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace compiler
{
    class Language_profil
    {
        public ArrayList automata = new ArrayList();
        private ArrayList enkoding_automata = new ArrayList();
        public ArrayList enkoding_ut_pita2 = new ArrayList();
        private Boolean kode_error;
        public string state;
        public string kode, nama, state_awal, state_accept;

        public string ConvertStringArrayToString(string[] word, char connector)
        {
            string word_string="";
            foreach(string i in word)
            {
                if(i!="") word_string = word_string + i + connector;
            }
            word_string = word_string.Trim(connector);
            return word_string;
        }

        private void Enkoding_state(string state, string state_awal, string state_accept)
        {
            //string[] parts;
            string[] state_reduce;
            string[] state_accept_pecah;
            //string[] state_reduce_split;
            ArrayList state_reduce_space = new ArrayList();
            
            ArrayList state_bukan_awal_akhir = new ArrayList();

            state_accept_pecah = state_accept.Split(',');


            state_reduce = state.Split(',');

            for (int i = 0; i < state_reduce.Length; i++)
            {
                if (state_reduce[i] == state_awal) { state_reduce[i] = ""; } else
                {
                    for (int j = 0; j < state_accept_pecah.Length; j++)
                    {
                        if (state_reduce[i] == state_accept_pecah[j]) state_reduce[i] = "";
                    }
                }
                
            }
            
            enkoding_automata.Add(state_awal);
            for (int i=0; i< state_reduce.Length; i++)
            {

                if (state_reduce[i] != "")
                {
                    enkoding_automata.Add(state_reduce[i]);
                }
            }
            
            for(int i=0;i< state_accept_pecah.Length; i++)
            {
                enkoding_automata.Add(state_accept_pecah[i]);
            }
        }

        private void Enkoding_instruction()
        {
            //method ini dimaksudkan untuk mengenkoding semua instruksi untuk dimasukkan ke pita 2 mesin UTM
            //method ini mungkin dipindahkan ke kelas UTM pada pengembangan lanjut, tetapi sementara disimpan pada kelas profil
            //konsekuensinya adalah bahwa kelas profil terikat dengan satu cara enkoding saja. InShaa Allah kedepannya ingin dibuat kelas profil
            //yang general yang dapat beradaptasi dengan berbagai jenis cara enkoding, mungkin method-method enkoding di dalam kelas profil 
            //dihilangkan dan dipindahkan ke kelas UTM yang lebih spesifik.
            //Secara intuitif, method ini hanya menerima hasil enkoding state method Enkoding_state dan memanfaatkan data state untuk digabung
            //semua menjadi arraylist enkoding instruksi sesuai cara enkoding di disertasi.
            string siap_parsing;
            string[] buffer;
            string[] buffer2 = { "0", "1", "2", "3", "4", "5"};

            /*
            //Pengingat:
            automata.Add(nama); //0
            automata.Add(state_awal); //1
            automata.Add(state_accept); //2
            automata.Add(state); //3
            automata.Add(nilai_start); //4
            automata.Add(jumlah_state); //5
            automata.Add(nilai_f); //6
            automata.Add(kode); //7 
            */
            
            enkoding_ut_pita2.Add(automata[5]); //sel pertama berisi enkoding jumlah state
            enkoding_ut_pita2.Add(automata[6]); //sel kedua berisi enkoding nilai index state accept terkecil

            //Berikutnya pengisian sel-sel berikut dengan enkoding instruksi-instruksi, sesuai disertasi
            siap_parsing = (string) automata[7];

            //pangkas dulu simbol ";" di ujung
            siap_parsing = siap_parsing.Trim(';');

            //pecah siap_parsing dalam instruksi-instruksi individual 
            string[] parsing = siap_parsing.Split(';');

            //enkoding tiap instruksi
            foreach(string i in parsing)
            {
                if (i != "") { //abaikan instruksi kosong

                    //pecah tiap instruksi ke dalam komponen-komponennya
                    buffer = i.Split(',');

                    //format buffer2 = { "state_sumber0", "input1", "ukuran2","state_berikut3", "output4", "d5" };
                    
                    string buffer3 = "";

                    //enkoding setiap komponen untuk sebuah instruksi, sebuah i yang diwakilkan ke arraystring string[] buffer 
                    //format buffer = {state_sumber0,input1,state_berikut2,output3,d4}
                    buffer2[0] = enkoding_automata.IndexOf(buffer[0]).ToString();
                    buffer2[1] = buffer[1];
                    buffer2[2] = buffer[1].Length.ToString();
                    buffer2[3] = enkoding_automata.IndexOf(buffer[2]).ToString();
                    buffer2[4] = buffer[3];
                    buffer2[5] = buffer[4];
                    
                    buffer3 = ConvertStringArrayToString(buffer2, ',') + ';';
                    enkoding_ut_pita2.Add(buffer3); //Sementara diisi yang buffer3 yang string, berikutnya mungkin lebih mudah buffer yang string[]
                    //enkoding_ut_pita2.Add(buffer2);
                }
            }
        }

        public string Cek_sintaks(string sintaks)
        {
            string[] parse;

            string state_trim = sintaks.Trim().Trim(','); //bersihkan dari spasi pada ujung2nya lalu bersihkan dari koma pada ujung2nya
            
            Regex cek0 = new Regex(@"\W*");
            Match p0;
            p0 = cek0.Match(state_trim);

            if ((state_trim.Length == 0)| (state_trim == p0.ToString()))
            {
                kode_error = true;
                Console.WriteLine("Maaf, sintaks tidak boleh kosong atau bukan huruf-huruf alphanumerik");
            }
            else
            {
                //cek apakah semua karakter state adalah sah?
                //gunakan regex
                parse = state_trim.Split(',');
                Regex cek = new Regex(@"\W");
                MatchCollection p;
                Boolean ok = true;
                foreach (string i in parse)
                {
                    //pertama: apakah alphanumerik, _ dan .?
                    //kedua: apakah tidak dobel? tetapi ini boleh saja dobel karena sepertinya tidak berpengaruh. 
                    //InShaa Allah untuk berikutnya nanti mungkin bagus dibatasi.
                    p = cek.Matches(i);
                    if (p.Count != 0)
                    {
                        foreach (Match h in p)
                        {
                            if(!((h.ToString()==".") | (h.ToString() == "_"))) //membolehkan _ dan . tapi tidak yang lain selain alphanumeric
                            {
                                kode_error = true;
                                if (ok) { Console.WriteLine("Maaf ada kesalahan memasukkan nama, rincian kesalahan anda adalah:"); ok = false; }
                                Console.WriteLine("kesalahan {0} adalah {1}", i, h);
                            }
                        }
                    }
                }
            }
            //Perbaiki sintaks: lakukan trim space dan koma atau secara umum \W lalu cek yang dobel lalu 
            if (!kode_error)
            {
                Boolean underscore=false;

                Regex cek_trim = new Regex(@"\W");
                MatchCollection k = cek_trim.Matches(sintaks);
                string buffer_state = sintaks.Trim('.');
                if (buffer_state[0] == '_') underscore=true;

                foreach (Match i in k)
                {
                    char y = i.ToString().ToCharArray()[0];
                    buffer_state = buffer_state.Trim(y);
                }
                string[] state_split= buffer_state.Split(',');
                state_split = state_split.Distinct().ToArray();
                buffer_state = ConvertStringArrayToString(state_split,',');

                state_split = buffer_state.Split('.');
                buffer_state = ConvertStringArrayToString(state_split, '.');

                state_split = buffer_state.Split('_');
                for(int i=0;i< state_split.Length;i++)
                {
                    state_split[i] = state_split[i].Trim('.');
                }
                buffer_state = ConvertStringArrayToString(state_split, '_');

                state_split = buffer_state.Split(',');
                for (int i = 0; i < state_split.Length; i++)
                {
                    state_split[i] = state_split[i].TrimEnd('_');
                }
                buffer_state = ConvertStringArrayToString(state_split, ',');

                string[] cek_lagi_hilangkan_dot = buffer_state.Split(',');
                int t = 0;
                foreach(string u in cek_lagi_hilangkan_dot)
                {
                    cek_lagi_hilangkan_dot[t] = u.Trim('.');
                    t++;
                }

                buffer_state = ConvertStringArrayToString(cek_lagi_hilangkan_dot, ',');

                //if (buffer_state[0] == '.') buffer_state=buffer_state.Trim('.');
                if (underscore) buffer_state = "_" + buffer_state;
                
                if (buffer_state != sintaks)
                {
                    string persetujuan;
                    Boolean setuju;
                    Console.WriteLine("\nSintaks anda telah diperbaki menjadi: {0}", buffer_state);
                    do
                    {
                        setuju = false;
                        Console.WriteLine("Konfrimasi persetujuan anda, ketik y jika setuju, jika tidak ketik n:");
                        persetujuan = Console.ReadLine();
                        if (!((persetujuan.ToUpper() == "Y") | (persetujuan.ToUpper() == "N"))) setuju = true;
                    } while (setuju);
                    if (persetujuan.ToUpper() == "N") kode_error = true;
                    if (persetujuan.ToUpper() == "Y") sintaks = buffer_state;
                }
            }
            return sintaks;
        }

        public string Input_dengan_cek_sintaks(string inputan,string keterangan)
        {
            do
            {
                kode_error = false;
                Console.WriteLine(keterangan);
                inputan = Console.ReadLine();
                inputan = Cek_sintaks(inputan);

            } while (kode_error);

            return inputan;
        }

        public string Cek_sintaks_instruksi(string sintaks_kode)
        {
            string[] buffer_split;
            ArrayList tampung_error = new ArrayList();
            ArrayList tampung_semua_0 = new ArrayList();
            ArrayList tampung_semua_2 = new ArrayList();

            //perbaiki dengan menambahkan titik-koma di akhir gramatika
            if (sintaks_kode[sintaks_kode.Length - 1] != ';') sintaks_kode = sintaks_kode + ';';

            //Persiapan cek per instruksi
            string kode_trim = sintaks_kode.Trim(';');
            string[] kode_split = kode_trim.Split(';');

            //cek ukuran tiap instruksi;
            foreach(string i in kode_split)
            {
                buffer_split = i.Split(','); //A,aaba,B,bbb,2;B,caaa,C,ab,-1;
                if (buffer_split.Length!=5)
                {
                    kode_error = true;
                    Console.WriteLine("\nInstruksi {0} tidak sesuai sintaks, harusnya berbentuk: state_asal,input,state_akhir,output,d",i);
                }
                else
                {
                    Regex cek_buffer = new Regex(@"[A-Za-z0-9_.-]+");
                    Regex cek_digit = new Regex(@"\d+|-\d+");
                    //Match k = cek_buffer.Match(buffer_split);
                    Console.WriteLine("\n");
                    foreach (string j in buffer_split)
                    {
                        if (j != cek_buffer.Match(j).ToString())
                        {
                            kode_error = true;
                            Console.WriteLine("Komponen {0} dari instruksi {1} tidak sesuai sintaks", j, i);
                        }
                    }
                    if(buffer_split[4]!= cek_digit.Match(buffer_split[4]).ToString())
                    {
                        kode_error = true;
                        Console.WriteLine("\nKomponen d pada instruksi bentuk (State_sumber,input,state_berikut,ouptut,d) \nharuslah berupa bilangan bulat positif atau negatif");
                    }
                    if (!state.Contains(buffer_split[0])) tampung_error.Add(buffer_split[0]);
                    if (!state.Contains(buffer_split[2])) tampung_error.Add(buffer_split[2]);
                    tampung_semua_0.Add(buffer_split[0]);
                    tampung_semua_2.Add(buffer_split[2]);
                }
            }

            if (tampung_error.Count > 0)
            {
                kode_error = true;
                Console.WriteLine("\nBeberapa state di dalam instruksi anda tidak termasuk dalam daftar state, yaitu:");
                foreach (string p in tampung_error)
                {
                    Console.WriteLine("{0}", p);
                }
                Console.WriteLine("\n");
            }
            if(!tampung_semua_0.Contains(state_awal))
            {
                kode_error = true;
                Console.WriteLine("\nState awal yang menerima input (berposisi sebagai state_asal dalam format \ninstruksi (state_asal,input,state_akhir,output,d)) belum ada di dalam instruksi-instruksi anda , silahkan tambahkan");
            }
            string[] tampung_accept = state_accept.Split(',');
            Boolean okbro=false;
            foreach(string n in tampung_accept)
            {
                if (tampung_semua_2.Contains(n)) okbro = true;
            }
            if(!okbro)
            {
                kode_error = true;
                Console.WriteLine("\nTak ada satupun state accept ada di dalam instruksi-instruksi anda dan berposisi sebagai state_akhir \ndalam format instruksi (state_asal,input,state_akhir,output,d), silahkan tambahkan minimal 1");
            }
            return sintaks_kode;
        }

        public void Register_bahasa()
        {
            //Buat array dinamis penyimpan instruksi-instruksi automata
            int nilai_f; //nilai index start accept terendah
            int nilai_start = 0; //nilai index start awal, default 0
            int jumlah_state = 0; //jumlah state
            Boolean ada = false;
            
            nama = Input_dengan_cek_sintaks(nama, "Nama bahasa berbasis automata anda, boleh lebih dari 1 nama tapi dipisah koma, \ncontoh L(gFSA) untuk bahasa berbasis automata gFSA:");
            state = Input_dengan_cek_sintaks(state, "\nSilahkan masukkan semua nama state dalam urutan tetapi dipisah koma, tanpa spasi.\nPenamaan yang dibolehkan adalah alphanumerik, _ dan . :");
            do
            {

                kode_error = false;
                state_awal = Input_dengan_cek_sintaks(state_awal, "\nMasukkan nama state awal (start state):");
                string[] uji = state_awal.Split(',');
                if (uji.Length > 1)
                {
                    kode_error = true;
                    Console.WriteLine("Maaf untuk sementara, state start tidak boleh lebih dari 1 state");
                }
                else 
                {
                    ada = false;
                    string[] state_split = state.Split(',');
                    foreach(string i in state_split)
                    {
                        if (state_awal == i) ada = true;
                    }
                    if (!ada)
                    {
                        kode_error = true;
                        Console.WriteLine("Maaf, state awal anda tidak ada dalam daftar state");
                    }
                        
                }
                
            } while (kode_error);

            //state_accept = Input_dengan_cek_sintaks(state_accept, "\nBerikutnya masukkan nama-nama state akhir, bisa lebih dari 1 (accept state):");
            do
            {

                kode_error = false;
                state_accept = Input_dengan_cek_sintaks(state_accept, "\nBerikutnya masukkan nama-nama state akhir, bisa lebih dari 1 (accept state):");
                string[] state_accept_split = state_accept.Split(',');

                ada = false;
                string[] state_split = state.Split(',');
                ArrayList cek_isi=new ArrayList();
                int h = 0;
                
                foreach (string j in state_accept_split) 
                {
                    if (!state_split.Contains(j))
                    {
                        cek_isi.Add(j);
                    }
                }
                if(cek_isi.Count>0)
                {
                    kode_error = true;
                    Console.WriteLine("Maaf, ada state accept yang tidak ada dalam daftar state yaitu: ");
                    foreach(string j in cek_isi)
                    {
                        Console.Write("{0}, ",j);
                    }
                    Console.WriteLine("\n");
                }

                if (state_accept_split.Contains(state_awal))
                {
                    kode_error = true;
                    Console.WriteLine("Maaf untuk sementara state awal dibatasi tidak boleh masuk dalam state akhir");
                    Console.WriteLine("yaitu state {0}", state_awal);
                }
                    
            } while (kode_error);

            //lakukan pengecekan apakah state awal,state akhir? yang diinputkan ada di dalam state?
            //pengecekan state awal, hanya boleh 1 state, dan tak boleh kosong
            //pengecekan state akhir minimal 1 state dan tak boleh kosong

            do
            {
                kode_error = false;
                Console.WriteLine("\nMasukkan automata anda disini (berbentuk instruksi-instruksi dipisahkan oleh titik-koma)");
                Console.WriteLine("Bentuk umum instruksi: (state_sumber,input,state_tujuan,output yang ditulis di pita,arah dan jumlah geser head);");
                Console.WriteLine("Contoh 1 instruksi: A,aaba,B,bbb,2;");
                Console.WriteLine("Contoh 2 instruksi: A,aaba,B,bbb,2;B,caaa,C,ab,-1;");
                Console.WriteLine("Automata anda:");
                kode = Console.ReadLine();
                kode = Cek_sintaks_instruksi(kode);

            } while (kode_error);
            
            Enkoding_state(state, state_awal, state_accept);
            jumlah_state=enkoding_automata.Count;
            string[] state_finish = state_accept.Split(',');
            nilai_start = enkoding_automata.IndexOf(state_awal);
            nilai_f = enkoding_automata.IndexOf(state_finish[0]);

            //setiap satu profil dalam mod 8
            automata.Add(nama); //8/0
            automata.Add(state_awal); //7/1
            automata.Add(state_accept); //6/2
            automata.Add(state); //5/3
            automata.Add(nilai_start); //4/4
            automata.Add(jumlah_state); //3/5
            automata.Add(nilai_f); //2/6
            automata.Add(kode); //1/7

            Console.WriteLine("\n");
            Console.WriteLine("Nama bahasa anda adalah:" + automata[automata.Count - 8]);
            Console.WriteLine("State awal adalah:" + automata[automata.Count - 7]);
            Console.WriteLine("State accept adalah:" + automata[automata.Count - 6]);
            Console.WriteLine("Seluruh state adalah:" + automata[automata.Count - 5]);
            Console.WriteLine("Nilai enkoding state awal adalah:" + automata[automata.Count - 4]);
            Console.WriteLine("Jumlah total state adalah:" + automata[automata.Count - 3]);
            Console.WriteLine("Nilai enkoding terkecil state akhir  state adalah:" + automata[automata.Count - 2]);
            Console.WriteLine("Barisan instruksi automata bahasa adalah:" + automata[automata.Count - 1]);
            
            Console.WriteLine("\n");
            Console.WriteLine("Nilai yang di enkoding untuk start state:" + enkoding_automata[0]);
            
            for(int i = 0; i < enkoding_automata.Count; i++)
            {
                Console.WriteLine("Nilai yang di enkoding untuk state {0}: {1}",i, enkoding_automata[i]);
            }
            
            Console.WriteLine("\nNilai enkoding untuk state accept terendah adalah:" + nilai_f);
            Console.WriteLine("Nilai enkoding untuk state awal adalah:" + nilai_start);

            Console.WriteLine("\n");
            Enkoding_instruction(); //Proses enkoding instruksi dilakukan

            Console.WriteLine("\n");
            for (int i = 0; i < enkoding_ut_pita2.Count ; i++)
            {
                Console.WriteLine("Nilai yang di enkoding untuk sel ke-{0} adalah: {1}", i, enkoding_ut_pita2[i]);
            }

            Console.ReadLine();
        }
    }
}
