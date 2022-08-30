using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static string SelectedLanguage { get; private set; }

    private static TranslationDictionary localizationDefault;

    private static TranslationDictionary localiztion;

    [SerializeField]
    private TextAsset localizationFileDefault;

    [SerializeField]
    private TextAsset[] localizationFiles;

    private int activeLanguage = 0;

    private void Awake()
    {
        if (localizationFileDefault == null)
            LoadDefaultLocaliztion();

        if (localiztion == null)
            LoadLocaliztion(activeLanguage);

        //DebugGetPhrases();
    }
    private void LoadDefaultLocaliztion()
    {
        string text = localizationFileDefault.text;
        localizationDefault = JsonUtility.FromJson<TranslationDictionary>(text);
    }

    public void LoadLocaliztion(int index)
    {
        string text = localizationFiles[index].text;
        localiztion = JsonUtility.FromJson<TranslationDictionary>(text);
    }
    public static string GetPhrase(int index)
    {
        return localiztion.Phrases[index];
    }

    public class TranslationDictionary
    {
        public string Language { get; private set; }
        public string LanguageName { get; private set; }
        public List<string> Phrases;
        public int version;
    }

    public void DebugGetPhrases()
    {
        StringBuilder tmp =  new StringBuilder();
        
        for (int i = 0; i < localiztion.Phrases.Count; i++)
        {
            tmp.Append(i + ") " + localiztion.Phrases[i] + "\n");
        }

        Debug.Log(tmp.ToString());
    }
}
