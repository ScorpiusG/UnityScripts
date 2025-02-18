using System.Collections.Generic;
using UnityEngine;

public class StringExtensions
{
    public static string UppercaseFirst(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }

    public static char GetRandomVowel()
    {
        char[] vowels = "aeiou".ToCharArray();
        return vowels[Random.Range(0,vowels.Length)];
    }

    public static char GetRandomConsonant()
    {
        char[] consonants = "bcdfghjklmnpqrstvwxyz".ToCharArray();
        return consonants[Random.Range(0, consonants.Length)];
    }

    public static string GenerateProperNoun(int length)
    {
        string name = "";
        int consecutiveVowels = 0;
        int consecutiveConsonants = 0;
        for (int i = 0; i < length; i++)
        {
            float vowelChance = Random.Range(0f, 1f);
            if (consecutiveVowels < 2 &&
				(vowelChance < 0.5f || consecutiveConsonants >= 2))
            {
                name += GetRandomVowel();
                consecutiveVowels++;
				consecutiveConsonants = 0;
            }
            else
            {
                name += GetRandomConsonant();
                consecutiveConsonants++;
				consecutiveVowels = 0;
            }
        }

        return UppercaseFirst(name);
    }
}