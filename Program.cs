using System.Drawing;
using System.Runtime.Versioning;
using System.Xml;

internal class Program
{
    [SupportedOSPlatform("windows")]
    private static void Main(string[] args)
    {
        if (args == null || args.Length < 1 || args[0] == null)
        {
            Console.WriteLine("Please, use path to the xml file as first and single parameter when you call this application.");
            return;
        }

        try
        {
            bool fileDoesntExist = !File.Exists(args[0]);
            if (fileDoesntExist)
            {
                Console.WriteLine("File '{0}' does not exist. Please, specify correct path to file.", args[0]);
                return;
            }

            XmlReader xmlReader = XmlReader.Create(args[0]);
            Image? fullTextureAtlas = null;
            bool textureAtlasExist = false;
            bool subTextureExist = false;
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "TextureAtlas")
                {
                    textureAtlasExist = true;
                    string? imagePath = xmlReader.GetAttribute("imagePath");
                    if (xmlReader.HasAttributes && imagePath != null && imagePath != string.Empty)
                    {
                        string fullImagePath = imagePath.Contains(Path.DirectorySeparatorChar) ? imagePath : Path.GetDirectoryName(args[0]) + Path.DirectorySeparatorChar + imagePath;
                        if (File.Exists(fullImagePath))
                        {
                            fullTextureAtlas = Image.FromFile(fullImagePath);
                        }
                        else
                        {
                            Console.WriteLine("Image file '{0}' does not exist. Please, specify correct path to image file inside xml file.", fullImagePath);
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Attribute 'imagePath' for TextureAtlas does not exist. Please, specify correct xml file.", args[0]);
                        return;
                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "SubTexture")
                {
                    subTextureExist = true;
                    if (fullTextureAtlas != null)
                    {
                        // TODO read each element and create new image file. If error show it and go to next element. Do not exit 
                    }
                }
            }
            if (!textureAtlasExist)
            {
                Console.WriteLine("Element TextureAtlas does not exist. Please, specify correct xml file.", args[0]);
            }
            else if (!subTextureExist)
            {
                Console.WriteLine("No one element SubTexture does not exist. Please, specify correct xml file.", args[0]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}