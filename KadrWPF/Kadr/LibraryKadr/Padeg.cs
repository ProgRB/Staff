using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;

namespace LibraryKadr
{
    public class Padeg
    {
        public enum Type_value
        {
            Mans_Family,Womens_Family,
            Mans_Name,Womens_Name,
            Mans_SName,Womens_SName,
            Word_Combination
        }
        public enum Type_word
        {
            TheNoun,
            Adjectiv
        }
        private static HashSet<char> glasnie_char = new HashSet<char>(new Char[] { 'А', 'Е', 'Ё', 'И', 'О', 'У', 'Ы', 'Э', 'Ю', 'Я' });
        private static HashSet<char> shipyashie_gluchie = new HashSet<char>(new Char[] { 'Ж', 'Ш', 'Ч', 'Щ', 'Г', 'К', 'Х' });
        private static HashSet<char> shipyashie = new HashSet<char>(new Char[] { 'Ж', 'Ш', 'Ч', 'Щ', 'Ц' });
        private static HashSet<char> soglasn = new HashSet<char>(new Char[] { 'Б', 'В', 'Г', 'Д', 'Ж', 'З', 'Й', 'К', 'Л', 'М', 'Н', 'П', 'Р', 'С', 'Т', 'Ф', 'Х', 'Ц', 'Ш', 'Щ' });
        private static Type_word Word(string value)
        {
            if (value.Length > 1)
            {
                value = value.ToUpper();
                string scc= value.Substring(value.Length-2,2);
                if (scc=="ИЙ" || scc == "ЫЙ"||scc=="" || scc == "ОЙ"||scc=="" || scc == "АЯ"  || scc == "ЯЯ")
                    return Type_word.Adjectiv;
            }
            return Type_word.TheNoun;
        }
        /// <summary>
        /// Функция родительного падежа для слова
        /// </summary>
        /// <param name="value">cуществительное</param>
        /// <returns>Возвращает существительное в род.п.</returns>
        private static string WordToRoditeln(string s)
        {
            OracleDataReader r = new OracleCommand(string.Format("select * from {0}.PADEG_EXCEPT where UPPER(val)=UPPER('{1}') and type_word=6",Connect.Schema , s), Connect.CurConnect).ExecuteReader();
            if (r.Read())
            {
                string st = r["ROD_P"].ToString();
                return st;
            }
            string copy_st = s;
            s = s.ToUpper();
            char sC=(s.Length>0?s[s.Length-1]:Convert.ToChar(0)),
                sCc=(s.Length>1?s[s.Length-2]:Convert.ToChar(0)),
                sCcc=(s.Length>2?s[s.Length-3]:Convert.ToChar(0))
                ;
            string sCC=(s.Length>2?s.Substring(s.Length-2,2):"");
            if (sCC == "ОЙ") s = s.Substring(0, s.Length - 2) + "ОГО";
            else
                if (sCC == "ЫЙ") s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc) ? "ЕГО" : "ОГО");
                else
                    if (sCC == "ИЙ") s = s.Substring(0, s.Length - 2) + ((new HashSet<char>() { 'Г', 'К', 'Х' }).Contains(sCcc) ? "ОГО" : "ЕГО");
                    else
                        if (sCC == "АЯ") s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc) ? "ЕЙ" : "ОЙ");
                        else
                            if (sCC == "ЯЯ") s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc) ? "ЕЙ" : "ЕЙ");
                            else
            if (glasnie_char.Contains(sC))
            {
                switch (sC )
                {
                    case 'А':  s = s.Substring(0, s.Length - 1) + (shipyashie_gluchie.Contains(sCc) ? "И" : "Ы");break;
                    case 'O': s = s.Substring(0, s.Length - 1) + 'А'; break;
                    case 'Я': s = s.Substring(0, s.Length - 1) + 'И'; break;
                    default: break;
                }                 
            }
            else
                if (soglasn.Contains(sC))
                {
                    s = s + 'А';
                }
                else
                    if (sC=='Ь') //значит слово оканчивается на мягкий знак.
                    s = s.Substring(0, s.Length - 1) + 'Я';
            int i = 0;
            while (i < s.Length && i < copy_st.Length && copy_st[i].ToString().ToUpper() == s[i].ToString())
                ++i;
            if (i>0)
                if (copy_st[i-1]>64 && copy_st[i-1]<91)
                    if (i<s.Length) s=copy_st.Substring(0,i)+s.Substring(i).ToUpper();
                    else s=copy_st;
                else
                    if (i<s.Length) s=copy_st.Substring(0,i)+s.Substring(i).ToLower();
                    else s=copy_st;
            return s;
        }
        /// <summary>
        /// Функция дательного падежа для существительного
        /// </summary>
        /// <param name="value">cуществительное</param>
        /// <returns>Возвращает существительное в род.п.</returns>
        private static string WordToDateln(string s)
        {
            OracleDataReader r = new OracleCommand(string.Format("select * from {0}.PADEG_EXCEPT where UPPER(val)=UPPER('{1}') and type_word=6", Connect.Schema, s), Connect.CurConnect).ExecuteReader();
            if (r.Read())
            {
                string st = r["DAT_P"].ToString();
                return (st[0] + st.Substring(1, st.Length - 1).ToLower());
            }
            string copy_st = s;
            s = s.ToUpper();
            char sC = (s.Length > 0 ? s[s.Length - 1] : Convert.ToChar(0)),
                sCc = (s.Length > 1 ? s[s.Length - 2] : Convert.ToChar(0)),
                sCcc = (s.Length > 2 ? s[s.Length - 3] : Convert.ToChar(0))
                ;
            string sCC = (s.Length > 2 ? s.Substring(s.Length - 2, 2) : "");
            if (sCC == "ОЙ") s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc) ? "ЕМУ" : "ОМУ");
            else
                if (sCC == "ЫЙ") s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc) ? "ЕМУ" : "ОМУ");
                else
                    if (sCC == "ИЙ") s = s.Substring(0, s.Length - 2) + ((new HashSet<char>() { 'Г', 'К', 'Х' }).Contains(sCcc) ? "ОМУ" : "ЕМУ");
                    else
                        if (sCC == "АЯ") s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc) ? "ЕЙ" : "ОЙ");
                        else
                            if (sCC == "ЯЯ") s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc) ? "ЕЙ" : "ЕЙ");
                            else
            if (glasnie_char.Contains(sC))
            {
                switch (sC)
                {
                    case 'А': s = s.Substring(0, s.Length - 1) + "E"; break;
                    case 'O': s = s.Substring(0, s.Length - 1) + 'У'; break;
                    case 'Я': s = s.Substring(0, s.Length - 1) + 'Е'; break;
                    default: break;
                }
            }
            else
                if (soglasn.Contains(sC))
                {
                    s = s + 'У';
                }
                else//значит слово оканчивается на мягкий знак.
                    if (sC == 'Ь') s = s.Substring(0, s.Length - 1) + 'Ю';
            int i = 0;
            while (i < s.Length && i < copy_st.Length && copy_st[i].ToString().ToUpper() == s[i].ToString())
                ++i;
            if (i > 0)
                if (copy_st[i - 1] > 64 && copy_st[i - 1] < 91)
                    if (i < s.Length) s = copy_st.Substring(0, i) + s.Substring(i).ToUpper();
                    else s = copy_st;
                else
                    if (i < s.Length) s = copy_st.Substring(0, i) + s.Substring(i).ToLower();
                    else s = copy_st;
            return s;
        }
        /// <summary>
        /// Функция винительного падежа для существительного
        /// </summary>
        /// <param name="value">cуществительное</param>
        /// <returns>Возвращает существительное в род.п.</returns>
        private static string WordToViniteln(string s)
        {
            OracleDataReader r = new OracleCommand(string.Format("select * from {0}.PADEG_EXCEPT where UPPER(val)=UPPER('{1}') and type_word=6", Connect.Schema, s), Connect.CurConnect).ExecuteReader();
            if (r.Read())
            {
                string st = r["VIN_P"].ToString();
                return (st[0] + st.Substring(1, st.Length - 1).ToLower());
            }
            string copy_st = s;
            s = s.ToUpper();
            char sC = (s.Length > 0 ? s[s.Length - 1] : Convert.ToChar(0)),
                sCc = (s.Length > 1 ? s[s.Length - 2] : Convert.ToChar(0)),
                sCcc = (s.Length > 2 ? s[s.Length - 3] : Convert.ToChar(0))
                ;
            string sCC = (s.Length > 2 ? s.Substring(s.Length - 2, 2) : "");
            if (sCC == "ОЙ") s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc) ? "ЕГО" : "ОГО");
            else
                if (sCC == "ЫЙ") s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc) ? "ЕГО" : "ОГО");
                else
                    if (sCC == "ИЙ") s = s.Substring(0, s.Length - 2) + ((new HashSet<char>() { 'Г', 'К', 'Х' }).Contains(sCcc) ? "ОГО" : "ЕГО");
                    else
                        if (sCC == "АЯ") s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc) ? "УЮ" : "УЮ");
                        else
                            if (sCC == "ЯЯ") s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc) ? "ЮЮ" : "ЮЮ");
                            else
            if (glasnie_char.Contains(sC))
            {
                switch (sC)
                {
                    case 'А': s = s.Substring(0, s.Length - 1) + "У"; break;
                    case 'O': s = s.Substring(0, s.Length - 1) + 'О'; break;
                    case 'Я': s = s.Substring(0, s.Length - 1) + 'Ю'; break;
                    default: break;
                }
            }
            else
                if (soglasn.Contains(sC))
                {
                    s = s + 'А';
                }
                else//значит слово оканчивается на мягкий знак.
                    if (sC == 'Ь') s = s.Substring(0, s.Length - 1) + 'Я';
            int i = 0;
            while (i < s.Length && i < copy_st.Length && copy_st[i].ToString().ToUpper() == s[i].ToString())
                ++i;
            if (i > 0)
                if (copy_st[i - 1] > 64 && copy_st[i - 1] < 91)
                    if (i < s.Length) s = copy_st.Substring(0, i) + s.Substring(i).ToUpper();
                    else s = copy_st;
                else
                    if (i < s.Length) s = copy_st.Substring(0, i) + s.Substring(i).ToLower();
                    else s = copy_st;
            return s;
        }

        /// <summary>
        /// Функция получения родительного падежа от фамилии, имени, отчества или словосочетания
        /// </summary>
        /// <param name="_connection"></param>
        /// <param name="_schema"></param>
        /// <param name="s">строка входная</param>
        /// <param name="_type_val">тип строки -  имя(муж, жен), фамилия и тд.</param>
        /// <returns>Возвращает слова в родительно падеже</returns>
        public static string Roditeln(string s, Type_value _type_val)
        {
            int k;
            switch (_type_val)
            {
                case Type_value.Mans_Family: k = 0; break;
                case Type_value.Womens_Family: k = 1; break;
                case Type_value.Mans_Name: k = 2; break;
                case Type_value.Womens_Name: k = 3; break;
                case Type_value.Mans_SName: k = 4; break;
                case Type_value.Womens_SName: k = 5; break;
                default: k = 6; break;
            }
            OracleDataReader r = new OracleCommand(string.Format("select * from {0}.PADEG_EXCEPT where UPPER(VAL)=UPPER('{1}') and type_WORD={2}",Connect.Schema,s,k),Connect.CurConnect).ExecuteReader();
            if (r.Read())
            {
                string st = r["ROD_P"].ToString();
                return (st[0] + st.Substring(1, st.Length - 1).ToLower());
            }
            string str_copy = s;
             s=s.ToUpper();
            if (_type_val== Type_value.Mans_Family)
            {
                string sCC, sC,sCc,sCcc;
                sCcc=(s.Length>3?s.Substring(s.Length-3,1):"");
                sCC=(s.Length>2?s.Substring(s.Length-2,2):"");
                sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                switch (sCC)
                {
                    case "ОВ": { s = s + "А"; }; break;
                    case "ЕВ": { s = s + "А"; }; break;
                    case "ИН": { s = s + "А"; }; break; //если после шипящих то :
                    case "ОЙ": {s =s.Substring(0,s.Length-2)+"ОГО";}break;
                    case "ЫЙ": { s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc[0]) ? "ЕГО" : "ОГО"); } break;
                    case "ИЙ": { s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc[0]) ? "ЕГО" : "ОГО"); } break;
                    default:
                        switch (sC)
                        {
                            case "Е": break;
                            case "Ё": break;
                            case "Э": break;
                            case "И": break;
                            case "Ы": break;
                            case "У": break;
                            case "Ю": break;
                            case "О": break;
                            case "Я": s = s.Substring(0,s.Length - 1) + "И"; break;
                            case "Ь": s = s.Substring(0,s.Length - 1) + "Я"; break;
                            case "Й": s = s.Substring(0,s.Length - 1) + "Я"; break;
                            case "A": if (!glasnie_char.Contains(sCc[0])) s = s.Substring(0,s.Length - 1) + "Ы"; break;
                        }; break;
                }
            }
            else
            if (_type_val==Type_value.Womens_Family)
            {
                string sCCС, sCC, sC, sCc, sCcc;
                sCCС = (s.Length > 3 ? s.Substring(s.Length - 3, 3) : "");
                sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                sCC = (s.Length > 2 ? s.Substring(s.Length - 2, 2) : "");
                sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                sCcc = (s.Length > 3 ? s.Substring(s.Length - 3, 1) : "");
                switch (sCCС)
                {
                    case "ОВА": { s = s.Substring(0,s.Length-1) + "ОЙ"; }; break;
                    case "ЕВА": { s = s.Substring(0, s.Length - 1) + "ОЙ"; }; break;
                    case "ИНА": { s = s.Substring(0, s.Length - 1) + "ОЙ"; }; break;
                    default://после шипящих если
                        if (sCC == "АЯ") { s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc[0]) ? "ЕЙ" : "ОЙ"); } 
                            else if (sCC == "ЯЯ") s = s.Substring(0, s.Length - 2) + "ЕЙ";
                        else
                        switch (sC)
                        {
                            case "Е": break;
                            case "Ё": break;
                            case "Э": break;
                            case "И": break;
                            case "Ы": break;
                            case "У": break;
                            case "Ю": break;
                            case "О": break;
                            case "Я": break;                            
                            case "Й": break;
                            case "A": if (!glasnie_char.Contains(sCc[0])) s = s.Substring(0,s.Length - 1) + "Ы"; break;
                        }; break;
                }
            }
            else
                if (_type_val==Type_value.Mans_Name)
                {
                    string sC,sCc;
                    sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                    sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                    switch (sC)
                    {                       
                        case "Я": s=s.Substring(0,s.Length-1)+"И"; break;
                        case "А": s=s.Substring(0,s.Length-1)+(shipyashie_gluchie.Contains(sCc[0])? "И": "Ы");break;
                        case "Ь": s=s.Substring(0,s.Length-1)+"Я"; break;
                        case "Й": s=s.Substring(0,s.Length-1)+"Я"; break;
                        default: if (soglasn.Contains(sC[0])) s = s + "А"; break;
                    }
                }
                else 
                    if (_type_val== Type_value.Womens_Name)
                    {
                        string sC,sCc;
                        sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                        sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                        switch (sC)
                        {                       
                            case "Я": s=s.Substring(0,s.Length-1)+"И"; break;
                            case "А": s=s.Substring(0,s.Length-1)+(shipyashie_gluchie.Contains(sCc[0])? "И": "Ы");break;
                            case "Ь": s=s.Substring(0,s.Length-1)+"И"; break;
                            case "Й": s=s.Substring(0,s.Length-1)+"И"; break;
                            default: if (soglasn.Contains(sC[0])) s = s + (shipyashie_gluchie.Contains(sCc[0]) ? "И" : "Ы"); break;
                        }
                    }
                    else
                        if (_type_val== Type_value.Mans_SName)
                        {
                            if (s.Length>0 && s[s.Length-1]=='Ч') s=s+'А';
                        }
                        else 
                            if (_type_val== Type_value.Womens_SName)
                            {
                                if (s.Length>1 && s.Substring(s.Length-2,2)=="НА") s=s.Substring(0,s.Length-1)+'Ы';
                            }
                            else if (_type_val == Type_value.Word_Combination)//если комбинация слов, то разбиваем на слова предложение.
                            {
                                s = str_copy;
                                List<string> l1= new List<string>();
                                bool fl=true;//признак что впереди было прилагательное или что надо продолжать сколнение
                                string st = "";
                                s += ' ';
                                for (int i = 0; i < s.Length; i++)                                    
                                    if (s[i] == '-')
                                    {
                                        st = st.Trim();
                                        if (st != "")
                                        {
                                            fl = true;
                                            l1.Add(WordToRoditeln(st));
                                            l1.Add("-");
                                        }
                                        st = "";
                                    }
                                    else
                                        if (s[i] == ' ')
                                        {
                                            
                                                st = st.Trim();
                                                if (st.Length > 0)
                                                {
                                                    if (fl)
                                                    {
                                                        if (Word(st) == Type_word.Adjectiv)
                                                            fl = true;
                                                        else fl = false;
                                                        l1.Add(WordToRoditeln(st));
                                                    }
                                                    else
                                                    {
                                                        l1.Add(st);
                                                    }
                                                }
                                                st = "";
                                        }
                                        else st += s[i];
                                s = (l1.Count>0?l1[0]:"");
                                for (int i = 1; i < l1.Count; i++)
                                    if (l1[i] != "-" && l1[i - 1] != "-")
                                        s += ' ' + l1[i];
                                    else
                                        s += l1[i];
                                return s;        
                            }
            if (s.Length > 0)
                s = s[0] + s.Substring(1, s.Length - 1).ToLower();
            return s;
        }
        public static string Viniteln(string s, Type_value _type_val)
        {
            int k;
            switch (_type_val)
            {
                case Type_value.Mans_Family: k = 0; break;
                case Type_value.Womens_Family: k = 1; break;
                case Type_value.Mans_Name: k = 2; break;
                case Type_value.Womens_Name: k = 3; break;
                case Type_value.Mans_SName: k = 4; break;
                case Type_value.Womens_SName: k = 5; break;
                default: k = 6; break;
            }
            OracleDataReader r = new OracleCommand(string.Format("select * from {0}.PADEG_EXCEPT where UPPER(VAL)=UPPER('{1}') and type_WORD={2}", Connect.Schema, s, k), Connect.CurConnect).ExecuteReader();
            if (r.Read())
            {
                string st=r["VIN_P"].ToString();
                return (st[0]+st.Substring(1,st.Length-1).ToLower());
            }
            string copy_st = s;
            s = s.ToUpper();
            if (_type_val == Type_value.Mans_Family)
            {
                string sCC, sC, sCc, sCcc;
                sCC = (s.Length > 2 ? s.Substring(s.Length - 2, 2) : "");
                sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                sCcc = (s.Length > 3 ? s.Substring(s.Length - 3, 1) : "");
                switch (sCC)
                {
                    case "ОВ": { s = s + "А"; }; break;
                    case "ЕВ": { s = s + "А"; }; break;
                    case "ИН": { s = s + "А"; }; break;
                    case "ОЙ": { s = s.Substring(0, s.Length - 2) + "ОГО"; } break;
                    case "ЫЙ": { s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc[0]) ? "ЕГО" : "ОГО"); } break;
                    case "ИЙ": { s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc[0]) ? "ЕГО" : "ОГО"); } break;
                    default:
                        switch (sC)
                        {
                            case "Я": s = s.Substring(0, s.Length - 1) + "Ю"; break;
                            case "Ь": s = s.Substring(0, s.Length - 1) + "Я"; break;
                            case "Й": s = s.Substring(0, s.Length - 1) + "Я"; break;
                            case "A": if (!glasnie_char.Contains(sCc[0])) s = s.Substring(0, s.Length - 1) + "У"; break;
                        }; break;
                }
            }
            else
                if (_type_val == Type_value.Womens_Family)
                {
                    string sCCС, sCC, sC, sCc;
                    sCCС = (s.Length > 3 ? s.Substring(s.Length - 3, 3) : "");
                    sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                    sCC = (s.Length > 2 ? s.Substring(s.Length - 2, 2) : "");
                    sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                    switch (sCCС)
                    {
                        case "ОВА": { s = s.Substring(0, s.Length - 1) + "У"; }; break;
                        case "ЕВА": { s = s.Substring(0, s.Length - 1) + "У"; }; break;
                        case "ИНА": { s = s.Substring(0, s.Length - 1) + "У"; }; break;
                        default:
                            if (sCC == "АЯ") { s = s.Substring(0, s.Length - 2) + "УЮ"; }
                            else if (sCC == "ЯЯ") s = s.Substring(0, s.Length - 2) + "ЮЮ";
                            else
                                switch (sC)
                                {
                                    case "A": if (!glasnie_char.Contains(sCc[0])) s = s.Substring(0, s.Length - 1) + "У"; break;
                                }; break;
                    }
                }
                else
                    if (_type_val == Type_value.Mans_Name)
                    {
                        string sC, sCc;
                        sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                        sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                        switch (sC)
                        {
                            case "Я": s = s.Substring(0, s.Length - 1) + "Ю"; break;
                            case "А": s = s.Substring(0, s.Length - 1) + "У"; break;
                            case "Ь": s = s.Substring(0, s.Length - 1) + "Я"; break;
                            case "Й": s = s.Substring(0, s.Length - 1) + "Я"; break;
                            default: if (soglasn.Contains(sC[0])) s = s + "А"; break;
                        }
                    }
                    else
                        if (_type_val == Type_value.Womens_Name)
                        {
                            string sC, sCc;
                            sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                            sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                            switch (sC)
                            {
                                case "Я": s = s.Substring(0, s.Length - 1) + "Ю"; break;
                                case "А": s = s.Substring(0, s.Length - 1) + "У"; break;
                                case "Ь": s = s.Substring(0, s.Length - 1) + "Ь"; break;
                                case "Й": s = s.Substring(0, s.Length - 1) + "Ю"; break;
                                default: if (soglasn.Contains(sC[0])) s = s + "У"; break;
                            }
                        }
                        else
                            if (_type_val == Type_value.Mans_SName)
                            {
                                if (s.Length > 0 && s[s.Length - 1] == 'Ч') s = s + 'А';
                            }
                            else
                                if (_type_val == Type_value.Womens_SName)
                                {
                                    if (s.Length > 1 && s.Substring(s.Length - 2, 2) == "НА") s = s.Substring(0, s.Length - 1) + 'У';
                                }
                                else if (_type_val == Type_value.Word_Combination)//если комбинация слов, то разбиваем на слова предложение.
                                {
                                    s = copy_st;
                                    List<string> l1 = new List<string>();
                                    bool fl = true;//признак что впереди было прилагательное или что надо продолжать сколнение
                                    string st = "";
                                    s += ' ';
                                    for (int i = 0; i < s.Length; i++)
                                        if (s[i] == '-')
                                        {
                                            st = st.Trim();
                                            if (st != "")
                                            {
                                                fl = true;
                                                l1.Add(WordToViniteln(st));
                                                l1.Add("-");
                                            }
                                            st = "";
                                        }
                                        else
                                            if (s[i] == ' ')
                                            {

                                                st = st.Trim();
                                                if (st.Length > 0)
                                                {
                                                    if (fl)
                                                    {
                                                        if (Word(st) == Type_word.Adjectiv)
                                                            fl = true;
                                                        else fl = false;
                                                        l1.Add(WordToViniteln(st));
                                                    }
                                                    else
                                                    {
                                                        l1.Add(st);
                                                    }
                                                }
                                                st = "";
                                            }
                                            else st += s[i];
                                    s = (l1.Count > 0 ? l1[0] : "");
                                    for (int i = 1; i < l1.Count; i++)
                                        if (l1[i] != "-" && l1[i - 1] != "-")
                                            s += ' ' + l1[i];
                                        else
                                            s += l1[i];
                                    return s;
                                }
            if (s.Length > 0)
                s = s[0] + s.Substring(1, s.Length - 1).ToLower();
            return s;
        }
        public static string Dateln(string s, Type_value _type_val)
        {
            int k;
            switch (_type_val)
            {
                case Type_value.Mans_Family: k = 0; break;
                case Type_value.Womens_Family: k = 1; break;
                case Type_value.Mans_Name: k = 2; break;
                case Type_value.Womens_Name: k = 3; break;
                case Type_value.Mans_SName: k = 4; break;
                case Type_value.Womens_SName: k = 5; break;
                default: k = 6; break;
            }
            OracleDataReader r = new OracleCommand(string.Format("select * from {0}.PADEG_EXCEPT where UPPER(val)=UPPER('{1}') and type_WORD={2}", Connect.Schema, s, k), Connect.CurConnect).ExecuteReader();
            if (r.Read())
            {
                string st = r["dat_p"].ToString();
                return (st[0] + st.Substring(1, st.Length - 1).ToLower());
            }
            string copy_st = s;
            s = s.ToUpper();
            if (_type_val == Type_value.Mans_Family)
            {
                string sCC, sC, sCc, sCcc;
                sCcc = (s.Length > 3 ? s.Substring(s.Length - 3, 1) : "");
                sCC = (s.Length > 2 ? s.Substring(s.Length - 2, 2) : "");
                sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                switch (sCC)
                {
                    case "ОВ": { s = s + "У"; }; break;
                    case "ЕВ": { s = s + "У"; }; break;
                    case "ИН": { s = s + "У"; }; break; //если после шипящих то :
                    case "ОЙ": { s = s.Substring(0, s.Length - 2) + "ОМУ"; } break;
                    case "ЫЙ": { s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc[0]) ? "ЕМУ" : "ОМУ"); } break;
                    case "ИЙ": { s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc[0]) ? "ЕМУ" : "ОМУ"); } break;
                    default:
                        switch (sC)
                        {
                            case "Я": s = s.Substring(0, s.Length - 1) + "Е"; break;
                            case "Ь": s = s.Substring(0, s.Length - 1) + "И"; break;
                            case "Й": s = s.Substring(0, s.Length - 1) + "Ю"; break;
                            case "A": if (!glasnie_char.Contains(sCc[0])) s = s.Substring(0, s.Length - 1) + "Е"; break;
                        }; break;
                }
            }
            else
                if (_type_val == Type_value.Womens_Family)
                {
                    string sCCС, sCC, sC, sCc, sCcc;
                    sCCС = (s.Length > 3 ? s.Substring(s.Length - 3, 3) : "");
                    sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                    sCC = (s.Length > 2 ? s.Substring(s.Length - 2, 2) : "");
                    sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                    sCcc = (s.Length > 3 ? s.Substring(s.Length - 3, 1) : "");
                    switch (sCCС)
                    {
                        case "ОВА": { s = s.Substring(0, s.Length - 1) + "ОЙ"; }; break;
                        case "ЕВА": { s = s.Substring(0, s.Length - 1) + "ОЙ"; }; break;
                        case "ИНА": { s = s.Substring(0, s.Length - 1) + "ОЙ"; }; break;
                        default://после шипящих если
                            if (sCC == "АЯ") { s = s.Substring(0, s.Length - 2) + (shipyashie.Contains(sCcc[0]) ? "ЕЙ" : "ОЙ"); }
                            else if (sCC == "ЯЯ") s = s.Substring(0, s.Length - 2) + "ЕЙ";
                            else
                                switch (sC)
                                {
                                    case "A": if (!glasnie_char.Contains(sCc[0])) s = s.Substring(0, s.Length - 1) + "Е"; break;
                                }; break;
                    }
                }
                else
                    if (_type_val == Type_value.Mans_Name)
                    {
                        string sC, sCc;
                        sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                        sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                        switch (sC)
                        {
                            case "Я": s = s.Substring(0, s.Length - 1) + "Е"; break;
                            case "А": s = s.Substring(0, s.Length - 1) + "Е"; break;
                            case "Ь": s = s.Substring(0, s.Length - 1) + "Ю"; break;
                            case "Й": s = s.Substring(0, s.Length - 1) + "Ю"; break;
                            default: if (soglasn.Contains(sC[0])) s = s + "У"; break;
                        }
                    }
                    else
                        if (_type_val == Type_value.Womens_Name)
                        {
                            string sC, sCc;
                            sC = (s.Length > 1 ? s.Substring(s.Length - 1, 1) : "");
                            sCc = (s.Length > 2 ? s.Substring(s.Length - 2, 1) : "");
                            switch (sC)
                            {
                                case "Я": s = s.Substring(0, s.Length - 1) + "Е"; break;
                                case "А": s = s.Substring(0, s.Length - 1) + "Е"; break;
                                case "Ь": s = s.Substring(0, s.Length - 1) + "И"; break;
                                case "Й": s = s.Substring(0, s.Length - 1) + "И"; break;
                                default: if (soglasn.Contains(sC[0])) s = s + "Е"; break;
                            }
                        }
                        else
                            if (_type_val == Type_value.Mans_SName)
                            {
                                if (s.Length > 0 && s[s.Length - 1] == 'Ч') s = s + 'У';
                            }
                            else
                                if (_type_val == Type_value.Womens_SName)
                                {
                                    if (s.Length > 1 && s.Substring(s.Length - 2, 2) == "НА") s = s.Substring(0, s.Length - 1) + 'Е';
                                }
                                else if (_type_val == Type_value.Word_Combination)//если комбинация слов, то разбиваем на слова предложение.
                                {
                                    s = copy_st;
                                    List<string> l1 = new List<string>();
                                    bool fl = true;//признак что впереди было прилагательное или что надо продолжать сколнение
                                    string st = "";
                                    s += ' ';
                                    for (int i = 0; i < s.Length; i++)
                                        if (s[i] == '-')
                                        {
                                            st = st.Trim();
                                            if (st != "")
                                            {
                                                fl = true;
                                                l1.Add(WordToDateln(st));
                                                l1.Add("-");
                                            }
                                            st = "";
                                        }
                                        else
                                            if (s[i] == ' ')
                                            {

                                                st = st.Trim();
                                                if (st.Length > 0)
                                                {
                                                    if (fl)
                                                    {
                                                        if (Word(st) == Type_word.Adjectiv)
                                                            fl = true;
                                                        else fl = false;
                                                        l1.Add(WordToDateln(st));
                                                    }
                                                    else
                                                    {
                                                        l1.Add(st);
                                                    }
                                                }
                                                st = "";
                                            }
                                            else st += s[i];
                                    s = (l1.Count > 0 ? l1[0] : "");
                                    for (int i = 1; i < l1.Count; i++)
                                        if (l1[i] != "-" && l1[i - 1] != "-")
                                            s += ' ' + l1[i];
                                        else
                                            s += l1[i];
                                    return s;

                                }
            if (s.Length > 0)
                s = s[0] + s.Substring(1, s.Length - 1).ToLower();
            return s;
        }

    }
}
