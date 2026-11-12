using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LawyerProject.BLL
{
    public class Validation
    {
        //בדיקת תקינות מספרים ולוכסנים
        public static bool IsNumberAndProchent(char KeyAscii)
        {
            if ((KeyAscii >= '0') && (KeyAscii <= '9') ||
             (Convert.ToChar(KeyAscii) == 32) || (Convert.ToChar(KeyAscii) == 8) || (KeyAscii
              == '.') || (KeyAscii == '/') || (KeyAscii == ',') || (KeyAscii == '-'))
                return true;
            else
                return false;
        }
        //בדיקת מספר קטן מ- 100
        public static bool ChekNumAchuz(double num)
        {
            if (num <= 100) return true;
            else return false;
        }
        //בדיקת תקינות סיסמה חדשה
        public static bool ChekPassword(string str)
        {
            bool ok = true, okD = false, okl = false;
            if (str.Length < 8)
            {
                ok = false;
                MessageBox.Show("הסיסמה חייבת להכיל 8 תווים");
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str[i]) == true)
                {
                    okD = true;
                }
                if (char.IsLetter(str[i]) == true)
                {
                    okl = true;
                }
                if (okl == false)
                {
                    MessageBox.Show("הסיסמה חייבת להכיל אות באנגלית");
                    ok = false;
                }
                if (okD == false)
                {
                    MessageBox.Show("הסיסמה חייבת להכיל מספר");
                    ok = false;
                }
            }
            return ok;
        }
              //בדיקת תקינות תעודת זהות
              public static bool IsLegalId(string s)
              {
                    int x;
                    if (!int.TryParse(s, out x)) return false;
                    if (s.Length < 5 || s.Length > 9) return false;
                    for (int i = s.Length; i < 9; i++)
                        s = "0" + s;
                    int sum = 0;
                    for (int i = 0; i < 9; i++)
                    {
                        int k = ((i % 2) + 1) * (Convert.ToInt32(s[i]) - '0');
                        if (k > 9) k -= 9;
                        sum += k;
                    }
                    return sum % 10 == 0;
              }
                public static bool IsHebrew(string pattern)
                {
                    Regex reg = new Regex(@"\b[א-ת-\s ]+$");
                    Match match = reg.Match(pattern);
                    if (match.Success)
                        return true;  // correct
                    else
                        return false;  // is incorrect

                }
                //טלפון
                public static bool IsTelephone(string tel)
                {
                    string pattern = @"\b0[2 4 7 8 3 77 72 73]-[2-9]\d{6}$";
                    Regex reg = new Regex(pattern);
                    return reg.IsMatch(tel);
                }
                //פלאפון
                public static bool IsCellPhone(string tel)
                {
                    string pattern = tel;
                    Regex reg = new Regex(@"\b05[0 2 4 5 6 7 8 3]-[2-9]\d{6}$");
                    Match match = reg.Match(pattern);
                    if (match.Success)
                        return true;  // correct
                    else
                        return false;// is incorrect
                }
                //לידה תאריך לפי גיל חישוב//        
                public static int GetAge(DateTime d)
                {
                    DateTime t = DateTime.Today;
                    int age = t.Year - d.Year;
                    if (t < d.AddYears(age)) age--;
                    return age;
                }
                //בדיקה שהטקסט בפורמט של מייל       
                public static bool CheackMail(string email)
                {
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(email);
                    if (match.Success)
                        return true;  // correct
                    else
                        return false;  // is incorrect
                }
                //בלבד מספרים   //        
                public static bool IsNumber(string num)
                {
                    string pattern = num;
                    Regex reg = new Regex(@"\b[0-9-\s]+$");
                    Match match = reg.Match(pattern);
                    return match.Success;
                }
                // תו ומחיקת רווח מקש ,בעברית אותיות המאפשרת בדיקה         
                public static bool bdikat_text(char KeyAscii)
                {
                    if ((KeyAscii >= 'א') && (KeyAscii <= 'ת') ||
                    (Convert.ToChar(KeyAscii) == 8) || (Convert.ToChar(KeyAscii) == 32) ||
                    (KeyAscii >= 'A') && (KeyAscii <= 'Z') || (KeyAscii >= 'a') && (KeyAscii <=
                     'z'))
                        return true;
                    else
                        return false;
                }
                //ונקודה מספר הקשת המאפשרת בדיקה        
                public static bool bdikat_mispar(char KeyAscii)
                {
                    if ((KeyAscii >= '0') && (KeyAscii <= '9') ||
                    (Convert.ToChar(KeyAscii) == 32) || (Convert.ToChar(KeyAscii) == 8) || KeyAscii
                    == '.')
                        return true;
                    else
                        return false;
                }
            }
    }

