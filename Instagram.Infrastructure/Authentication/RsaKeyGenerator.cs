using System.Security.Cryptography;

namespace Instagram.Infrastructure.Authentication;

public class RsaKeyGenerator
{
    public static RSA? RsaKey { get; private set; }

    static RsaKeyGenerator()
    {
        ExportKeysIfNotExist();
        ImportKeys();
    }

    private static void ExportKeysIfNotExist()
    {
        var rsaKey = RSA.Create();
        var privateKey= rsaKey.ExportRSAPrivateKey();

        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        
        var keyPath = Path.Combine(baseDir, "rsa-key");
        if (!Path.Exists(keyPath))
            File.WriteAllBytes(keyPath , privateKey);
    }

    private static void ImportKeys()
    {
        var rsaKey = RSA.Create();
        
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var keyPath = Path.Combine(baseDir, "rsa-key");
        rsaKey.ImportRSAPrivateKey(File.ReadAllBytes(keyPath), out _);
        RsaKey = rsaKey;
    }
}