using Airslip.Common.Security.Enums;
using Airslip.Common.Security.Implementations;
using FluentAssertions;
using System.Text.RegularExpressions;
using System.Web;
using Xunit;

namespace Airslip.Common.Security.Tests;

public class StringCipherTests
{
    private const string PassPhrase = "SECRET_KEY";
        
    [Fact]
    public void Can_encrypt_and_decrypt_string_value()
    {
        string str = "{\"userType\":2,\"entityId\":\"entity-id\"}";
        string encryptedString = StringCipher.Encrypt(str, PassPhrase);
        string decryptedString = StringCipher.Decrypt(encryptedString, PassPhrase);
        decryptedString.Should().Be(str);
    }
        
    [Fact]
    public void Can_encrypt_and_decrypt_string_value_for_url()
    {
        string str = "{\"userType\":2,\"entityId\":\"entity-id\"}";
        string encryptedString = StringCipher.EncryptForUrl(str, PassPhrase);
            
        // Happens automatically during API request
        string urlDecodedEncryptedString = HttpUtility.UrlDecode(encryptedString);
            
        string decryptedString = StringCipher.Decrypt(urlDecodedEncryptedString, PassPhrase);
        decryptedString.Should().Be(str);
    }
        
    [Fact]
    public void Can_encrypt_and_decrypt_string_value_using_hex_conversion()
    {
        string str = "{\"userType\":2,\"entityId\":\"entity-id\"}";
        string encryptedString = StringCipher.Encrypt(str, PassPhrase,  conversionType: ConversionType.Hex);
        Regex alphanumericRegex = new("^[a-zA-Z0-9]*$");
        bool onlyContainsAlphanumericValues = alphanumericRegex.IsMatch(encryptedString);
        onlyContainsAlphanumericValues.Should().BeTrue();
        string decryptedString = StringCipher.Decrypt(encryptedString, PassPhrase, conversionType: ConversionType.Hex);
        decryptedString.Should().Be(str);
    }
        
    [Fact]
    public void Can_encrypt_and_decrypt_string_value_for_url_using_hex_conversion()
    {
        string str = "{\"userType\":2,\"entityId\":\"entity-id\"}";
        string encryptedString = StringCipher.EncryptForUrl(str, PassPhrase, conversionType: ConversionType.Hex);
        Regex alphanumericRegex = new("^[a-zA-Z0-9]*$");
        bool onlyContainsAlphanumericValues = alphanumericRegex.IsMatch(encryptedString);
        onlyContainsAlphanumericValues.Should().BeTrue();
        // Happens automatically during API request
        string urlDecodedEncryptedString = HttpUtility.UrlDecode(encryptedString);
            
        string decryptedString = StringCipher.Decrypt(urlDecodedEncryptedString, PassPhrase, conversionType: ConversionType.Hex);
        decryptedString.Should().Be(str);
    }
}