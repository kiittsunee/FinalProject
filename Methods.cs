﻿namespace TodoApi.Controllers
{
    public class Methods
    {
        public class TranslitMethods
        {
            public class Translitter
            {
                private List<TranslitSymbol> TranslitSymbols { get; set; }

                /// <summary>
                /// Метод транслитерации русского текста
                /// </summary>
                /// <param name="source">Исходная строка на русском</param>
                /// <param name="type">Тип транслитерации, гост или ISO</param>
                /// <returns></returns>
                public string Translit(string source, TranslitType type)
                {
                    var result = "";

                    for (var i = 0; i < source.Length; i++)
                    {
                        result = result +
                                 (TranslitSymbols.FirstOrDefault(x => x.SymbolRus == source[i].ToString() && x.TranslitType == type) == null
                            ? source[i].ToString()
                            : TranslitSymbols.First(x => x.SymbolRus == source[i].ToString() && x.TranslitType == type).SymbolEng);
                    }

                    return result;
                }

                // Конструктор - При создании заполняем справочники сопоставлений
                public Translitter()
                {
                    this.TranslitSymbols = new List<TranslitSymbol>();
                    var gost = "а:a,б:b,в:v,г:g,д:d,е:e,ё:jo,ж:zh,з:z,и:i,й:jj,к:k,л:l,м:m,н:n,о:o,п:p,р:r,с:s,т:t,у:u,ф:f,х:kh,ц:c,ч:ch,ш:sh,щ:shh,ъ:\",ы:y,ь:',э:eh,ю:ju,я:ja";
                    var iso = "а:a,б:b,в:v,г:g,д:d,е:e,ё:yo,ж:zh,з:z,и:i,й:j,к:k,л:l,м:m,н:n,о:o,п:p,р:r,с:s,т:t,у:u,ф:f,х:h,ц:c,ч:ch,ш:sh,щ:shh,ъ:\",ы:y,ь:',э:e,ю:yu,я:ya";

                    // Заполняем сопоставления по ГОСТ
                    foreach (string item in gost.Split(","))
                    {
                        string[] symbols = item.Split(":");
                        this.TranslitSymbols.Add(new TranslitSymbol
                        {
                            TranslitType = TranslitType.Gost,
                            SymbolRus = symbols[0].ToLower(),
                            SymbolEng = symbols[1].ToLower()
                        });
                        this.TranslitSymbols.Add(new TranslitSymbol
                        {
                            TranslitType = TranslitType.Gost,
                            SymbolRus = symbols[0].ToUpper(),
                            SymbolEng = symbols[1].ToUpper()
                        });
                    }

                    // Заполняем сопоставления по ISO
                    foreach (string item in iso.Split(","))
                    {
                        string[] symbols = item.Split(":");
                        this.TranslitSymbols.Add(new TranslitSymbol
                        {
                            TranslitType = TranslitType.Iso,
                            SymbolRus = symbols[0].ToLower(),
                            SymbolEng = symbols[1].ToLower()
                        });
                        this.TranslitSymbols.Add(new TranslitSymbol
                        {
                            TranslitType = TranslitType.Iso,
                            SymbolRus = symbols[0].ToUpper(),
                            SymbolEng = symbols[1].ToUpper()
                        });
                    }

                }
            }

            // Перечисление типов транскрипций
            public enum TranslitType
            {
                Gost, Iso
            }

            // Описание элемента справочника транскрипций
            private class TranslitSymbol
            {
                public TranslitType TranslitType { get; set; }
                public string SymbolRus { get; set; }
                public string SymbolEng { get; set; }
            }
        }

        public string TranslitName(string firstname, string name, string surname)
        {
            TranslitMethods.Translitter trn = new TranslitMethods.Translitter();
            string word = $"{firstname}{name[0]}{surname[0]}";
            string word2 = trn.Translit(word, TranslitMethods.TranslitType.Gost);
            string result = word2.ToLower();
            return result; 
        }

        public string GetPass()
        {
            int[] arr = new int[6]; // сделаем длину пароля в 16 символов
            Random rnd = new Random();
            string Password = "";

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rnd.Next(33, 125);
                Password += (char)arr[i];
            }
            return Password;
        }
    }

}

