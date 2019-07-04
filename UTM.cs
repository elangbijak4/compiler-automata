using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace compiler
{
    class UTM
    {
        private ArrayList pita1 = new ArrayList();
        private ArrayList pita2 = new ArrayList();
        private ArrayList pita3 = new ArrayList();
        private ArrayList pita4 = new ArrayList();
        private int head_pita1;
        private int head_pita2;
        private int head_pita3;
        private int head_pita4;
        private string state_UTM;
        private string state_user;
        private String state_awal;
        private String state_akhir;

        //Himpunan variabel untuk mengakses pita
        private string bufferT1;
        private string[] bufferT2;
        private string[] bufferT3;
        private int bufferT4;

        //Himpunan variabel untuk mengakses instruksi individual
        private string komponen_state_sekarang; //0
        private string komponen_input; //1
        private int komponen_ukuran_input; //2
        private string komponen_state_berikut; //3
        private string komponen_output; //4
        private int komponen_geseran_head; //5


        public string ConvertArrayListToString(ArrayList bufferarray)
        {
            string buffer4 = "";
            foreach (char i in bufferarray)
            {
                buffer4 = buffer4 + i;
            }

            return buffer4;
        }

        public string ConvertArrayListToString2(ArrayList bufferarray)
        {
            string buffer4 = "";
            for(int i=0; i < bufferarray.Count; i++)
            {
                buffer4 = buffer4 + bufferarray[i];
            }

            return buffer4;
        }

        public string ConvertArrayListToString_bykonektor(ArrayList bufferarray, char konektor)
        {
            string buffer4 = "";
            for (int i = 0; i < bufferarray.Count; i++)
            {
                buffer4 = buffer4 + bufferarray[i] + konektor;
            }
            buffer4 = buffer4.TrimEnd(konektor);
            return buffer4;
        }

        public void Inisialisasi()
        {
            /* Tugas method ini:
             * (1).
             * Mempersiapkan seluruh pita dengan cara memanggil kelas register bahasa
             * Sehingga user bisa memasukkan seluruh informasi tentang bahasanya.
             * Setelah user selesai memasukkan info bahasa, maka method ini mengambil nilai2 pada data class register bahasa untuk dimasukkan ke
             * pita-pita UTM, yaitu: 
             * Informasi state dan dipindahkan ke array state_user.
             * Informasi state awal lalu memindahkannya ke array string_awal dan menyalin ke array pita4
             * Informasi state akhir (accept states) lalu memindahkannya ke array state_akhir.
             * Informasi enkoding instruksi-instruksi mesin Turing lalu memindahkannya ke array pita2
             * .
             * Setelah itu, Semua variabel head_pita1, head_pita2, head_pita3, head_pita4 direset ke nilai 0.
             * 
             * (2).
             * Menginisiasi state UTM dengan state 0 atau 1 untuk digunakan method simulasi.
             * Dalam hal ini, variabel state_UTM direset ke 0.
             * 
             * (3).
             * Menjalankan method input untuk menerima input dari user.
             * Setelah user memasukkan maka method ini menyalin semua input ke array pita1.
             * 
             * (4).
             * Method menginisiasi objek untuk class respon.
             * 
             * (5).
             * Method dialihkan tugasnya ke method simulasi.
             */

            //Memanggil class bahasa untuk memnita user memasukkan bahasa yang ingin digunakan:
            Language_profil profil;
            profil = new Language_profil();
            profil.Register_bahasa();

            //Inisialisasi pita
            state_user = profil.state;
            state_awal = profil.state_awal;
            state_akhir = profil.state_accept;
            pita1 = null;
            pita2 = profil.enkoding_ut_pita2;
            pita3 = null;
            pita4.Add(profil.automata[4]);

            //Reset semua head pita
            head_pita1 = 0;
            head_pita2 = 0;
            head_pita3 = 0;
            head_pita4 = 0;

            //Reset pointer state UTM
            state_UTM = "1";

            //Gunakan kelas inputan untuk menerima input user:
            Console.Clear();
            Console.WriteLine("Mesin telah disiapkan untuk menerima input anda. Silahkan memasukkan input di bawah ini:");
            Inputclass input_user;
            input_user = new Inputclass();
            input_user.Set_input();
            input_user.Enkoding();
            pita1 = input_user.Enkodingkalimat;

            //Proses inisiasi kelas respon belum dapat dilakukan karena Kelasnya belum ada.



            //Konsol sementara untuk melihat hasil kerja kelas Inisialisasi:
            Console.WriteLine("\nHasil inisiasliasi pita:");
            Console.WriteLine("state_user = {0}", state_user);
            Console.WriteLine("state_awal = {0}", state_awal);
            Console.WriteLine("state_akhir = {0}", state_akhir);
            if (pita1 == null) Console.WriteLine("pita1 = null"); else Console.WriteLine("pita1 = {0}", ConvertArrayListToString(pita1));
            Console.WriteLine("pita2 = {0}", ConvertArrayListToString2(pita2)); 
            if (pita3 == null) Console.WriteLine("pita3 = null"); else Console.WriteLine("pita3 = not null");
            Console.WriteLine("pita4[0] = {0}", pita4[0]);
            Console.WriteLine("state_user = {0}", state_user);
            
            Console.WriteLine("\nHasil reset head semua pita:");
            Console.WriteLine("head_pita1 = {0}", head_pita1);
            Console.WriteLine("head_pita2 = {0}", head_pita2);
            Console.WriteLine("head_pita3 = {0}", head_pita3);
            Console.WriteLine("head_pita4 = {0}", head_pita4);

            Console.WriteLine("\nHasil inisialisasi counter state UTM:");
            Console.WriteLine("state_UTM = {0}", state_UTM);

            Console.WriteLine("\nInShaa Allah hasil selanjutnya adalah pekerjaan method simulasi:");
            Console.ReadLine();
        }

        private string[] Pembaca_instruksi(ArrayList pita_instruksi, int nomor_sel)
        {
            string buffer_instruksi;
            string[] instruksi;
            
            buffer_instruksi = pita_instruksi[nomor_sel].ToString().TrimEnd(';');
            instruksi = buffer_instruksi.Split(',');

            
            return instruksi;
        }
        
        public ArrayList Penghilang_item_ganda(ArrayList arrayList)
        {
            ArrayList arrayList2 = new ArrayList();
            string SarrayList = ConvertArrayListToString_bykonektor(arrayList, ';');
            string[] SarrayList2 = SarrayList.Split(';');
            SarrayList2 = SarrayList2.Distinct().ToArray();
            for(int i=0; i< SarrayList2.Length; i++) arrayList2.Add(SarrayList2[i]);

            return arrayList2;
        }

        public string ConvertStringArrayToString(string[] word)
        {
            string word_string = "";
            foreach (string i in word)
            {
                word_string = word_string + i;
            }
            return word_string;
        }

        public string ConvertIntArrayToString(int[] word)
        {
            string word_string = "";
            foreach (int i in word)
            {
                word_string = word_string + i;
            }
            return word_string;
        }

        public string ConvertStringArrayToString_bykonektor(string[] word, char konektor)
        {
            string word_string = "";
            foreach (string i in word)
            {
                word_string = word_string + i + konektor;
            }
            word_string = word_string.TrimEnd(konektor);
            return word_string;
        }

        public void Cek_point_trace(string point, string beginend)
        {
            Console.WriteLine("\nIni di {0} Proses sekarang di state_UTM {1}\n", beginend, point);
        }

        public void Simulasi()
        {
            /* Tugas method ini adalah:
             * (1).
             * Big picture method ini adalah berupa switch.
             * Nilai switch berubah menurut perubahan nilai pada variabel state_UTM.
             * Setiap nilai state_UTM menggambarkan prosedur yang harus dikerjakan kepada pita-pita.
             * Setiap blok dalam switch adalah blok kode.
             * Blok kode secara umum bisa merubah nilai state_UTM, sehingga dapat menjalankan prosedur berikut.
             * Variabel State_UTM adalah analogi dengan program counter pada prosesor komputer biasa.
             * .
             * Setiap blok kode dalam switch adalah algoritma tersendiri.
             * .
             * (2).
             * Setiap switch mencapai HALT pada state accept, maka rencananya dia memanggil method di dalam class respon
             * untuk menerjemahkan hasil dari state accept tersebut.
             * class respon dijalankan objeknya diawal agar tidak harus selalu menginisiasi objeknya, tetapi cukup sekali.
             * .
             * (3).
             * Selesai.
             */

            Boolean HALT = false;
            string state_accept_tercapai;
            ArrayList buffer_ut_disalin_keT3 = new ArrayList();
            
            //Baca terlebih dulu state_UTM
            do
            {
                Cek_point_trace(state_UTM, "begin do");
                
                switch (state_UTM)
                {
                    case "1":
                        Cek_point_trace(state_UTM, "begin");
                        //Cek apakah state pada pita4 adalah state accept atau bukan?
                        bufferT4 = int.Parse(pita4[head_pita4].ToString());
                        if ((bufferT4 > (int)pita2[1]) & (head_pita1>pita1.Count-1))
                        {
                            HALT = true;
                            state_accept_tercapai = state_user[int.Parse(pita4[head_pita4].ToString())].ToString();
                            state_UTM = "HALT";
                            Console.WriteLine("State accept telah tercapai, yaitu state; {0} ", bufferT4);
                            //LAPORAN:
                            Console.WriteLine("LAPORAN = Nilai bufferT4:{0}  dan (int)pita2[1]:{1} dan state_UTM:{2} dan HALT:{3} dan state_accept_tercapai:{4}", bufferT4, (int)pita2[1], state_UTM, HALT, state_accept_tercapai);
                        }
                        else state_UTM = "2a";

                        //LAPORAN:
                        Console.WriteLine("LAPORAN = Nilai bufferT4:{0}  dan (int)pita2[1]:{1} dan state_UTM:{2}", bufferT4, (int)pita2[1], state_UTM);
                        Cek_point_trace(state_UTM,"end");
                        break;
                    case "2a":
                        Cek_point_trace(state_UTM, "begin");
                        if (head_pita1>pita1.Count-1)
                        {
                            state_UTM = "HALT";
                            Console.WriteLine("Head pita 1 telah menunjuk sel yang melampaui panjang input");
                            //LAPORAN:
                            Console.WriteLine("LAPORAN = Nilai head_pita1:{0}  dan pita1.Count-1:{1} dan state_UTM:{2}", head_pita1, pita1.Count - 1, state_UTM);
                        }
                        else
                        {
                            bufferT1 = pita1[head_pita1].ToString();
                            state_UTM = "2b";

                            //LAPORAN:
                            Console.WriteLine("LAPORAN = Nilai bufferT1:{0} state_UTM:{1}", bufferT1, state_UTM);
                        }
                        Cek_point_trace(state_UTM, "end");
                        break;
                    case "2b":
                        Cek_point_trace(state_UTM, "begin");
                        if (pita3 != null)
                        {
                            bufferT3 = Pembaca_instruksi(pita3, head_pita3);
                            komponen_state_sekarang = bufferT3[0];
                            komponen_input = bufferT3[1];
                            bufferT4 = int.Parse(pita4[head_pita4].ToString());

                            //LAPORAN:
                            Console.WriteLine("LAPORAN (pita3 != null) = Nilai bufferT3:{0}  dan komponen_state_sekarang:{1} dan state_UTM:{2} dan komponen_input:{3} dan bufferT4:{4}", bufferT3, komponen_state_sekarang, state_UTM, komponen_input, bufferT4);
                            //Di sini ada pertanyaan, bagaimana jika tidak?
                            Console.WriteLine("\nCEK MANAKALA PITA3 TIDAK NULL KARENA PROSES BERULANG");
                            Console.WriteLine("bufferT4.ToString()={0}", bufferT4.ToString());
                            Console.WriteLine("komponen_state_sekarang = {0}", komponen_state_sekarang);
                            Console.WriteLine("pita1[head_pita1].ToString().ToArray()[0] = {0}", pita1[head_pita1].ToString().ToArray()[0]);
                            Console.WriteLine("komponen_input[0] = {0} \n", komponen_input[0]);

                            if ((bufferT4.ToString() == komponen_state_sekarang) & (pita1[head_pita1].ToString().ToArray()[0] == komponen_input[0])) state_UTM = "5"; else
                            {
                                //Bagian ini baru (bagian else), tidak diperhitungkan di dalam algoritma asli di disertasi, karena itu
                                //perlu DIAWASI dampaknya, mungkin ini redundan dengan case "4". Ini dibuat untuk menjawab: bagaiana jika tidak?
                                state_UTM = "3";
                                pita3 = null;
                                pita3 = new ArrayList();
                                buffer_ut_disalin_keT3 = null;
                                buffer_ut_disalin_keT3 = new ArrayList();

                                head_pita3 = 0;
                                //LAPORAN:
                                Console.WriteLine("LAPORAN (bufferT4.ToString() == komponen_state_sekarang) & = Nilai pita3:{0}  dan head_pita3:{1} dan state_UTM:{2}", pita3, head_pita3, state_UTM);

                            }
                        }
                        else
                        {
                            state_UTM = "3";
                            buffer_ut_disalin_keT3 = null;
                            buffer_ut_disalin_keT3 = new ArrayList();

                            //LAPORAN
                            Console.WriteLine("LAPORAN else = state_UTM:{0}", state_UTM);
                        }
                            
                        Cek_point_trace(state_UTM, "end");
                        break;
                    case "3":
                        Cek_point_trace(state_UTM, "begin");

                        //Membaca semua instruksi di pita2 untuk diperiksa cocok atau tidak atau HALT
                        for (int i=2; i < pita2.Count; i++)
                        {
                            head_pita2 = i; //Untuk konsistensi teks algoritma di disertasi
                            bufferT2 = Pembaca_instruksi(pita2, head_pita2);
                            komponen_state_sekarang = bufferT2[0];
                            komponen_input = bufferT2[1];
                            bufferT4 = int.Parse(pita4[head_pita4].ToString());
                            
                            
                            //CEK PEMBAGIAN STRING DALAM bufferT2:
                            int p = 0;
                            foreach(string u in bufferT2)
                            {
                                Console.WriteLine("Cek nilai item yaitu bufferT2[{0}] = {1}",p,u); p++;
                            }
                            //LAPORAN
                            Console.WriteLine("LAPORAN awal for (int i=2; i < pita2.Count - 2; i++):");
                            Console.WriteLine("LAPORAN head_pita2:{0} dan bufferT2:{1} dan komponen_state_sekarang:{2} dan komponen_input:{3} dan bufferT4:{4} ", head_pita2,ConvertStringArrayToString(bufferT2), komponen_state_sekarang, komponen_input, bufferT4);


                            //CEK KONDISI LOGIK:
                            Console.WriteLine("\nbufferT4.ToString() = {0}", bufferT4.ToString());
                            Console.WriteLine("komponen_state_sekarang = {0}", komponen_state_sekarang);
                            Console.WriteLine("pita1[head_pita1].ToString().ToArray()[0] = {0}", pita1[head_pita1].ToString().ToArray()[0]);
                            Console.WriteLine("komponen_input[0] = {0} \n", komponen_input[0]);
                            if ((bufferT4.ToString() == komponen_state_sekarang) & (pita1[head_pita1].ToString().ToArray()[0] == komponen_input[0]))
                            {
                                buffer_ut_disalin_keT3.Add(ConvertStringArrayToString_bykonektor(bufferT2,','));
                            }

                        }
                        

                        if (buffer_ut_disalin_keT3.Count == 0)
                        {
                            state_UTM = "HALT";
                            Console.WriteLine("Pencarian instruksi di pita2 tidak menemukan instruksi yang cocok dengan state sekarang (current state) atau cocok dengan input");
                        }
                        else
                        {
                            state_UTM = "4";
                        }
                        //LAPORAN
                        Console.WriteLine("\nLAPORAN nilai buffer_ut_disalin_keT3 = {0} ", ConvertArrayListToString2(buffer_ut_disalin_keT3));
                        Console.WriteLine("LAPORAN nilai state_UTM = {0} \n", state_UTM);

                        Cek_point_trace(state_UTM, "end");
                        break;
                    case "4":
                        Cek_point_trace(state_UTM, "begin");
                        pita3 = null;
                        pita3 = new ArrayList();
                        head_pita3 = 0;
                        //ArrayList buffer_ut_disalin_keT3_terurut = new ArrayList();
                        string[] buffer_sementara = new string[6]; //CEK DISINI,MUNGKIN BUKAN 7 TETAPI 6
                        int[] urut_ukuran = new int[buffer_ut_disalin_keT3.Count];

                        //LAPORAN
                        Console.WriteLine("LAPORAN awal state 4 = pita3:{0} dan head_pita3:{1} dan state_UTM:{2} dan buffer_sementara.Length:{3} dan urut_ukuran.Length:{4} ", ConvertArrayListToString2(pita3), head_pita3, state_UTM,ConvertStringArrayToString(buffer_sementara), urut_ukuran.Length);
                        //Proses mengurutkan semua instruksi yang ditemukan dan telah disimpan pada buffer_ut_disalin_keT3
                        {
                            for (int i = 0; i < buffer_ut_disalin_keT3.Count; i++)
                            {
                                buffer_sementara = Pembaca_instruksi(buffer_ut_disalin_keT3, i);
                                Console.WriteLine("nilai buffer_sementara = {0}", ConvertStringArrayToString(buffer_sementara));
                                
                                int.TryParse(buffer_sementara[2], out komponen_ukuran_input); //terjadi lewat error index disini
                                urut_ukuran[i] = komponen_ukuran_input;
                                //LAPORAN
                                Console.WriteLine("LAPORAN di for (int i = 0; i < buf..... = buffer_sementara:{0} dan buffer_sementara[2]:{1} dan komponen_ukuran_input:{2} ", ConvertStringArrayToString(buffer_sementara), int.Parse(buffer_sementara[2]), komponen_ukuran_input);
                            }
                            
                            //LAPORAN
                            Console.WriteLine("LAPORAN hasil urut ukuran = urut_ukuran_sebelum_urut:{0} dan state_UTM:{1}", ConvertIntArrayToString(urut_ukuran), state_UTM);
                            urut_ukuran = urut_ukuran.OrderByDescending(j => j).ToArray();
                            //LAPORAN
                            Console.WriteLine("LAPORAN hasil urut ukuran = urut_ukuran_setelah_urut:{0} dan state_UTM:{1}", ConvertIntArrayToString(urut_ukuran), state_UTM);

                            ArrayList pita3_sementara = new ArrayList();
                            for (int k = 0; k < urut_ukuran.Length; k++)
                            {
                                for (int i = 0; i < buffer_ut_disalin_keT3.Count; i++)
                                {
                                    buffer_sementara = Pembaca_instruksi(buffer_ut_disalin_keT3, i);

                                    //CEK NILAI BUFFER SEMENTARA
                                    int e = 0;
                                    foreach(string y in buffer_sementara)
                                    {
                                        Console.WriteLine("nilai buffer_sementara[{0}] = {1}",e,y);
                                        e++;
                                    }


                                    int.TryParse(buffer_sementara[2], out komponen_ukuran_input);
                                    if (urut_ukuran[k] == komponen_ukuran_input)
                                    {
                                        pita3_sementara.Add(ConvertStringArrayToString_bykonektor(buffer_sementara,','));
                                    }
                                }
                            }
                            pita3 = Penghilang_item_ganda(pita3_sementara); //APAKAH INI COCOK?

                            //LAPORAN
                            Console.WriteLine("LAPORAN operasi Penghilang_item_ganda = pita3:{0} dan state_UTM:{1}",ConvertArrayListToString2(pita3), state_UTM);
                            Console.WriteLine("LAPORAN operasi Penghilang_item_ganda = pita3_sementara:{0} dan state_UTM:{1}", ConvertArrayListToString2(pita3_sementara), state_UTM);
                        }
                        
                        head_pita2 = 0;
                        head_pita3 = 0;
                        state_UTM = "5";

                        //LAPORAN
                        Console.WriteLine("LAPORAN setelah state 4 selesai = head_pita2:{0} dan head_pita3:{1} dan state_UTM:{2}", head_pita2, head_pita3, state_UTM);

                        Cek_point_trace(state_UTM, "end");
                        break;
                    case "5":
                        Cek_point_trace(state_UTM, "begin");
                        string[] buffer_sementara2 = new string[6];
                        bufferT4 = int.Parse(pita4[head_pita4].ToString());
                        Boolean apakah_halt = true;

                        //LAPORAN:
                        Console.WriteLine("LAPORAN di awal state 5 selesai: head_pita4:{0}, bufferT4:{1}, apakah_halt:{2}, buffer_sementara2.Length:{3}", head_pita4, bufferT4, apakah_halt, buffer_sementara2.Length);

                        //Proses eksekusi instruksi di pita3, mulai head_pita3=0 (sudah ditentukan di state 4) yaitu mulai dari sel pertama pita3.
                        for (int i=head_pita3; i < pita3.Count; i++)
                        {
                            //Baca instruksi di sel ke-i, hanya perlu baca current_state nya dan input
                            buffer_sementara2 = Pembaca_instruksi(pita3, i);
                            
                            
                            //UJI BUFFER_SEMENTARA:
                            Console.WriteLine("\nNilai buffer_sementara {0}", ConvertStringArrayToString(buffer_sementara2));
                            int e = 0;
                            foreach (string y in buffer_sementara2)
                            {
                                Console.WriteLine("nilai buffer_sementara2[{0}] = {1}", e, y);
                                e++;
                            }


                            komponen_state_sekarang = buffer_sementara2[0];
                            komponen_input = buffer_sementara2[1];
                            komponen_state_berikut = buffer_sementara2[3];
                            komponen_output = buffer_sementara2[4];
                            int.TryParse(buffer_sementara2[5], out komponen_geseran_head);
                            string[] tampung_baca_pita1_sementara = new string[komponen_input.Length];
                            //CEK INSTRUKSI YANG DIBACA DI SEL KE-i DI PITA3:
                            Console.WriteLine("\nCEK INSTRUKSI YANG DIBACA DI SEL KE-i YAITU KE-{0} DI PITA3:",i);
                            Console.WriteLine("buffer_sementara = {0}", ConvertStringArrayToString(buffer_sementara2));
                            Console.WriteLine("komponen_state_sekarang = {0}", komponen_state_sekarang);
                            Console.WriteLine("komponen_input = {0}", komponen_input);
                            Console.WriteLine("komponen_state_berikut = {0}", komponen_state_berikut);
                            Console.WriteLine("komponen_output = {0}", komponen_output);
                            Console.WriteLine("komponen_geseran_head = {0}", komponen_geseran_head);
                            Console.WriteLine("tampung_baca_pita1_sementara.Length = {0} \n", tampung_baca_pita1_sementara.Length);

                            //Baca pita1 sebanyak ukuran komponen input, tapi ingat pointer head_pita1 jangan dirubah atau digeser karena ini cuma digunakan sementara.
                            //CEK SEBELUM ERROR:
                            Console.WriteLine("\nCEK SEBELUM ERROR:", i);
                            Console.WriteLine("head_pita1 = {0}", head_pita1);
                            Console.WriteLine("komponen_input = {0}", komponen_input);
                            Console.WriteLine("komponen_input.Length + head_pita1 = {0}\n", head_pita1+ komponen_input.Length);
                            
                            for (int h= head_pita1;h< komponen_input.Length + head_pita1; h++)
                            {
                                Console.WriteLine("Nilai h = {0}", h);
                                Console.WriteLine("Nilai h-head_pita1= {0}", h-head_pita1);
                            }

                            for (int h = head_pita1; h < komponen_input.Length + head_pita1; h++)
                            {
                                if (h<pita1.Count) tampung_baca_pita1_sementara[h - head_pita1] = pita1[h].ToString(); else
                                {
                                    tampung_baca_pita1_sementara[h - head_pita1] = ";"; //Perhatikan di masa depan pengisian ini, apakah bukan diisi " " saja yang bagus?
                                }
                            }
                                
                            //CEK INPUT YANG DIBACA DI SEL KE-i DI PITA1:
                            Console.WriteLine("\nCEK INPUT YANG DIBACA DI SEL KE-h YAITU KE-{0} DI PITA1:", head_pita1);
                            Console.WriteLine("tampung_baca_pita1_sementara = {0}", ConvertStringArrayToString(tampung_baca_pita1_sementara));

                            if (komponen_input == ConvertStringArrayToString(tampung_baca_pita1_sementara))
                            {
                                //Berpindah ke state berikut:
                                pita4[0] = komponen_state_berikut;
                                //CEK PERPINDAHAN STATE BERIKUT DI PITA4:
                                Console.WriteLine("\nCEK PERPINDAHAN STATE BERIKUT DI PITA4:");
                                Console.WriteLine("pita4[0] = {0}", pita4[0]);

                                //menulis output di pita1:
                                //CEK PENULISAN OUTPUT DI PITA1:
                                Console.WriteLine("\nCEK PENULISAN OUTPUT DI PITA1:");
                                for (int h = head_pita1; h < komponen_output.Length + head_pita1; h++)
                                {
                                    if (h < pita1.Count) pita1[h] = komponen_output[h - head_pita1]; else pita1.Add(komponen_output[h - head_pita1]);
                                    Console.WriteLine("pita1[{0}] = {1}", h,pita1[h]);
                                }




                                //Menggeser head_pita1 sejauh d?
                                //CEK PERGESERAN d DI PITA1:
                                Console.WriteLine("\nCEK PERGESERAN d DI PITA1:");
                                Console.WriteLine("head_pita1 sebelumnya: {0}", head_pita1);
                                head_pita1 =head_pita1+ komponen_geseran_head;
                                Console.WriteLine("head_pita1 setelah digeser sejauh d={0} adalah: {1}", komponen_geseran_head, head_pita1);

                                apakah_halt = false; //Jika ketemu instruksinya dan 
                                //CEK PERGESERAN d DI PITA1:
                                Console.WriteLine("\nCEK NILAI apakah_halt YAITU :{0}", apakah_halt);
                                Console.WriteLine("KALAU FALSE MAKA BERARTI INSTRUKSI KETEMU DAN TERJADI TRANSISI STATE");
                                break;
                            }
                        }

                        if (apakah_halt == true)
                        {
                            state_UTM = "HALT";
                            Console.WriteLine("Pencarian instruksi di pita3 tidak menemukan input yang cocok dengan instruksi");
                        }
                            else state_UTM = "6";
                        Cek_point_trace(state_UTM, "end");
                        break;
                    case "6":
                        Cek_point_trace(state_UTM, "begin");
                        state_UTM = "1";
                        Cek_point_trace(state_UTM, "end");
                        break;
                    case "HALT":
                        Cek_point_trace(state_UTM, "begin");
                        Console.WriteLine("Mesin telah HALT, mesin halt di posisi state {0}", pita4[0]);
                        HALT = true;
                        Cek_point_trace(state_UTM, "end");
                        break;
                    default:
                        Cek_point_trace(state_UTM, "begin");
                        Console.WriteLine("Maaf state mesin tidak dikenal, cek perubahan nilai pointer state_UTM di dalam setiap case.");
                        Cek_point_trace(state_UTM, "end");
                        break;
                }

                //if (HALT) Console.WriteLine("Mesin telah HALT, state accept tercapai. State accept adalah {0}", (int)pita2[1]);
                Cek_point_trace(state_UTM, "end do");
            } while (!HALT);
            Cek_point_trace(state_UTM, "diluar do");
            
        }

    }
}
