using UnityEngine;
using System.Collections;
using System.IO;

public static class Country {
    public const string Austria = "AT";
    public const string Belgium = "BE";
    public const string Bulgaria = "BG";
    public const string Croatia = "HR";
    public const string Cyprus = "CY";
    public const string CzechRepublic = "CZ";
    public const string Denmark = "DK";
    public const string Estonia = "EE";
    public const string Finland = "FI";
    public const string France = "FR";
    public const string Germany = "DE";
    public const string Greece = "GR";
    public const string Hungary = "HU";
    public const string Ireland = "IE";
    public const string Italy = "IT";
    public const string Latvia = "LT";
    public const string Lithuania = "LT";
    public const string Luxembourg = "LU";
    public const string Malta = "MT";
    public const string Netherlands = "NL";
    public const string Poland = "PL";
    public const string Portugal = "PT";
    public const string Romania = "RO";
    public const string Slovakia = "SK";
    public const string Solvenia = "SI";
    public const string Spain = "ES";
    public const string Sweden = "SE";
    public const string UnitedKingdom = "GB";

    public static class Message
    {
        public const string Hello = "hello";
        public const string ThankYou = "thankyou";
        public const string LanguageName = "languagename";
    }

    /// <summary>
    /// Get the plaintext name of the country.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <returns>The country's name.</returns>
    public static string getName(string country)
    {
        TextAsset names = Resources.Load<TextAsset>("Country Names");

        if (names != null)
        {
            string[] seperators = { "\n", "\r", "\nr", "\rn" };
            string[] split = names.text.Split(seperators, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in split)
            {
                string[] splitLine = line.Split(',');
                if (splitLine[0].Equals(country) == true)
                {
                    return splitLine[1];
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Get the identifier code of the country.
    /// </summary>
    /// <param name="country">The name of the country.</param>
    /// <returns>The country's code.</returns>
    public static string getCode(string country)
    {
        TextAsset names = Resources.Load<TextAsset>("Country Names");

        if (names != null)
        {
            string[] seperators = { "\n", "\r", "\nr", "\rn" };
            string[] split = names.text.Split(seperators, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in split)
            {
                string[] splitLine = line.Split(',');
                if (splitLine[1].Equals(country) == true)
                {
                    return splitLine[0];
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Gets a message in a specific langage.
    /// </summary>
    /// <param name="country">The country code to translate to.</param>
    /// <param name="message">The message code to translate.</param>
    /// <returns>The translated string.</returns>
    public static string getTranslation(string country, string message) //TODO: Deprecated?
    {
        return getTranslation(country, 1, message);
    }

    /// <summary>
    /// Gets a message in a specific language.
    /// </summary>
    /// <param name="country">The country code of the language.</param>
    /// <param name="language">The one-based ID of the language within that country.</param>
    /// <param name="message">The message code to translate.</param>
    /// <returns>The translated string.</returns>
    public static string getTranslation(string country, int language, string message)
    {
        TextAsset lang = Resources.Load<TextAsset>("Localization/" + country + language);
        if (lang != null)
        {
            string[] seperators = { "\n", "\r", "\nr", "\rn" };
            string[] split = lang.text.Split(seperators, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in split)
            {
                string[] splitLine = line.Split(':');
                if (splitLine[0].Equals(message) == true)
                {
                    return splitLine[1];
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Get the number of languages in a specific country.
    /// </summary>
    /// <param name="country">The country code to check.</param>
    /// <returns>The number of languages used by that country.</returns>
    public static int getNumLanguages(string country)
    {
        int count = 0;
        while (Resources.Load<TextAsset>("Localization/" + country + (count + 1)) != null)
        {
            count++;
        }

        return count;
    }

    /// <summary>
    /// Gets the ozone level of a country.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <returns>The value of the ozone.</returns>
    public static float getOzone(string country)
    {
        TextAsset pollution = Resources.Load<TextAsset>("Pollution");
        string[] seperators = { "\n", "\r", "\nr", "\rn" };
        string[] split = pollution.text.Split(seperators, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in split)
        {
            string[] splitLine = line.Split(',');
            if (splitLine[0].Equals(country) == true)
            {
                return float.Parse(splitLine[1]);
            }
        }

        return -1;
    }

    /// <summary>
    /// Gets the year the ozone data was recorded.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <returns>The year the data was recorded.</returns>
    public static int getOzoneYear(string country)
    {
        TextAsset pollution = Resources.Load<TextAsset>("Pollution");
        string[] seperators = { "\n", "\r", "\nr", "\rn" };
        string[] split = pollution.text.Split(seperators, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in split)
        {
            string[] splitLine = line.Split(',');
            if (splitLine[0].Equals(country) == true)
            {
                return int.Parse(splitLine[2]);
            }
        }

        return -1;
    }

    /// <summary>
    /// Gets the level of particulate matter in a country.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <returns>The value of the particulate matter.</returns>
    public static float getParticulate(string country)
    {
        TextAsset pollution = Resources.Load<TextAsset>("Pollution");
        string[] seperators = { "\n", "\r", "\nr", "\rn" };
        string[] split = pollution.text.Split(seperators, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in split)
        {
            string[] splitLine = line.Split(',');
            if (splitLine[0].Equals(country) == true)
            {
                return float.Parse(splitLine[3]);
            }
        }

        return -1;
    }

    /// <summary>
    /// Gets the year the particulate matter data was recorded.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <returns>The year the data was recorded.</returns>
    public static int getParticulateYear(string country)
    {
       TextAsset pollution = Resources.Load<TextAsset>("Pollution");
        string[] seperators = { "\n", "\r", "\nr", "\rn" };
        string[] split = pollution.text.Split(seperators, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in split)
        {
            string[] splitLine = line.Split(',');
            if (splitLine[0].Equals(country) == true)
            {
                return int.Parse(splitLine[4]);
            }
        }

        return -1;
    }

    /// <summary>
    /// Checks whether the specified country has been explored by the player.
    /// </summary>
    /// <param name="country">The country code to check.</param>
    /// <returns>True if the player has explored the country, false otherwise.</returns>
    public static bool isExplored(string country)
    {
        if (File.Exists(Application.persistentDataPath + "/explored") == false)
        {
            return false;
        }

        StreamReader reader = new StreamReader(Application.persistentDataPath + "/explored");

        while (reader.EndOfStream == false)
        {
            if (reader.ReadLine() == country)
            {
                reader.Close();
                return true;
            }
        }

        reader.Close();
        return false;
    }

    /// <summary>
    /// Registers the country as having been explored by the player.
    /// </summary>
    /// <param name="country">The country code to register.</param>
    public static void setExplored(string country)
    {
        if (isExplored(country) == false)
        {
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/explored", true);
            writer.WriteLine(country);
            writer.Close();
        }
    }
}
