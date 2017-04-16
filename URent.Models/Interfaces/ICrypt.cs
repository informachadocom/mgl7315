namespace URent.Models.Interfaces
{
    public interface  ICrypt
    {
        string Encrypt(string plainText, string passPhrase);
        string Decrypt(string cipherText, string passPhrase);
    }
}