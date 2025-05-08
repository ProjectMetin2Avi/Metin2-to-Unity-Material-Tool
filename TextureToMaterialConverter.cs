using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class TextureToMaterialConverter : EditorWindow
{
    private string textureFolderPath = "Assets/Textures";
    private string materialFolderPath = "Assets/Materials";
    private Shader defaultShader;

    [Header("Social Links")]
    [SerializeField] private string iconsFolderPath = "Assets/Tools/Icons";
    [SerializeField] private Texture2D GitHubIcon;
    [SerializeField] private Texture2D InstagramIcon;
    [SerializeField] private Texture2D DiscordIcon;
    [SerializeField] private Texture2D YouTubeIcon;
    [SerializeField] private Texture2D Metin2DownloadsIcon;
    [SerializeField] private Texture2D M2DevIcon;
    [SerializeField] private Texture2D TurkmmoIcon;
    private readonly string GitHubURL = "https://github.com/ProjectMetin2Avi";
    private readonly string instagramURL = "https://www.instagram.com/metin2.avi/";
    private readonly string discordURL = "https://discord.gg/WZMzMgPp38";
    private readonly string youtubeURL = "https://www.youtube.com/@project_avi";
    private readonly string Metin2DownloadsURL = "https://www.metin2downloads.to/cms/user/30621-metin2avi/";
    private readonly string M2DevURL = "https://metin2.dev/profile/53064-metin2avi/";
    private readonly string TurkmmoURL = "https://forum.turkmmo.com/uye/165187-trmove/";

    [MenuItem("Tools/Texture to Material Converter - @Metin2Avi")]
    public static void ShowWindow()
    {
        GetWindow<TextureToMaterialConverter>("Texture to Material");
    }

    private void OnGUI()
    {
        DrawSocialLinks();
        GUILayout.Label("Texture to Material Converter - @Metin2Avi", EditorStyles.boldLabel);

        textureFolderPath = EditorGUILayout.TextField("Texture Folder:", textureFolderPath);
        materialFolderPath = EditorGUILayout.TextField("Material Folder:", materialFolderPath);

        defaultShader = (Shader)EditorGUILayout.ObjectField(
            "Default Shader:",
            defaultShader ? defaultShader : Shader.Find("Standard"),
            typeof(Shader),
            false
        );

        if (GUILayout.Button("Generate Materials"))
        {
            CreateMaterials();
        }
    }

    private void DrawSocialLinks()
    {
        EditorGUILayout.Space(20);
        GUILayout.Label("Follow/Contact", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (DiscordIcon && GUILayout.Button(new GUIContent(GitHubIcon, "GitHub"), GUILayout.Width(40), GUILayout.Height(40)))
            Application.OpenURL(GitHubURL);

        if (DiscordIcon && GUILayout.Button(new GUIContent(InstagramIcon, "Instagram"), GUILayout.Width(40), GUILayout.Height(40)))
            Application.OpenURL(instagramURL);

        if (DiscordIcon && GUILayout.Button(new GUIContent(DiscordIcon, "Discord"), GUILayout.Width(40), GUILayout.Height(40)))
            Application.OpenURL(discordURL);

        if (YouTubeIcon && GUILayout.Button(new GUIContent(YouTubeIcon, "YouTube"), GUILayout.Width(40), GUILayout.Height(40)))
            Application.OpenURL(youtubeURL);

        if (Metin2DownloadsIcon && GUILayout.Button(new GUIContent(Metin2DownloadsIcon, "Metin2Downloads"), GUILayout.Width(40), GUILayout.Height(40)))
            Application.OpenURL(Metin2DownloadsURL);

        if (M2DevIcon && GUILayout.Button(new GUIContent(M2DevIcon, "M2Dev"), GUILayout.Width(40), GUILayout.Height(40)))
            Application.OpenURL(M2DevURL);

        if (TurkmmoIcon && GUILayout.Button(new GUIContent(TurkmmoIcon, "Turkmmo"), GUILayout.Width(40), GUILayout.Height(40)))
            Application.OpenURL(TurkmmoURL);

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void CreateMaterials()
    {
        if (!Directory.Exists(materialFolderPath))
        {
            Directory.CreateDirectory(materialFolderPath);
        }

        string[] textureFiles = Directory.GetFiles(textureFolderPath, "*.png");
        textureFiles = textureFiles.Concat(Directory.GetFiles(textureFolderPath, "*.jpg")).ToArray();

        foreach (string texturePath in textureFiles)
        {
            string assetPath = texturePath.Replace('\\', '/');
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

            if (texture != null)
            {
                string materialName = Path.GetFileNameWithoutExtension(texturePath);
                string materialPath = $"{materialFolderPath}/{materialName}.mat";

                if (File.Exists(materialPath))
                {
                    Debug.Log($"Material already exists: {materialName}");
                    continue;
                }

                Material material = new Material(defaultShader);
                material.mainTexture = texture;

                AssetDatabase.CreateAsset(material, materialPath);
                Debug.Log($"Material created: {materialName}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Process completed!");
    }
}
