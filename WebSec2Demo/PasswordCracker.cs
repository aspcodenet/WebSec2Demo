using System;
using System.IO;

namespace WebSec2Demo
{
    public class PasswordCracker
    {



        // På samma sätt som https://en.wikipedia.org/wiki/List_of_data_breaches
        //DVS Säg att vi fått tag på 
        /*
         *
         *Id	UserId	HashedPassword
        2	stefan	5F4DCC3B5AA765D61D8327DEB882CF99
        3	oliver	6DBD0FE19C9A301C4708287780DF41A2
        4	josefine	C1A93BDFFE7D5062BA3489BBD21E59DC
        */
        public string Solve(string hashedPassword)
        {
            Setup();
            using (var f = File.OpenText("hashes.txt"))
            {
                while (true)
                {
                    var line = f.ReadLine();
                    if (line == null) break;
                    var parts = line.Split(':');
                    var hashPart = parts[1];
                    if (hashPart == hashedPassword)
                    {
                        return parts[0];
                    }
                }

            }


            //Ok - nu ska vi KODA logik för att göra det omöjliga
            return null;
        }

        private void Setup()
        {
            if (!File.Exists("hashes.txt"))
            {
                using (var hashes = File.AppendText("hashes.txt"))
                {
                    using (var f = File.OpenText("commonPasswords.txt"))
                    {
                        while (true)
                        {
                            var line = f.ReadLine();
                            if (line == null) break;
                            hashes.WriteLine(line + ":" + PasswordHasher.CreateHash(line));
                        }
                    }

                }
            }

        }
    }
}