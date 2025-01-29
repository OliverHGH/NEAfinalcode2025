using UnityEngine;

public class ciphergeneration 
{
    string plaintext = "lookbehindyou";//plaintext to be encoded
    string[] keys = new string[] { "blink","help","stone","lonely"};//list of possible keys
    public string keytodisplay, encodedtext;
   public void CreateCipher()
    {
        int length = keys.Length;
        string key = keys[Random.Range(0,length)];//random key selected
        int keylength = key.Length;
        int plaintextlenth = plaintext.Length;
        string fulllengthkey = "";
        for (int i = 0; i <  (plaintextlenth / keylength); i++)
        {
            fulllengthkey += key;
        }
        for (int i = 0;i < plaintextlenth % keylength; i++)
        {
            fulllengthkey += key[i];//repeats key until it has the same length as plaintext
        }
        string ciphertext = "";
        for(int i = 0; i < plaintextlenth; i++)
        {
            int charnum = (CharToInt(plaintext[i])+ CharToInt(fulllengthkey[i]))%26;//adds together pos of chacters eg b + d = 2+4=6=f
            ciphertext += IntToChar(charnum);// adds chaacter to ciphertext
        }
        encodedtext = ciphertext;//sets ciphertext
        keytodisplay = key;
    }

    int CharToInt(char ch)//finds position in alphabet of character
    {
        return (int)ch- (int)('a') ;
    }

    char IntToChar(int ch)
    {//finds chacter at inputted position in alphabet
        return (char)(ch+ (int)('a') );
    }
}
