using System.Drawing;
using System.Drawing.Imaging;
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
            string? fileDirectory = Path.GetDirectoryName(args[0]);
            string directory = fileDirectory + Path.DirectorySeparatorChar + "textures" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(directory)) 
            {
                Directory.CreateDirectory(directory);
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
                        Console.WriteLine("Attribute 'imagePath' for TextureAtlas does not exist. Please, specify correct xml file.");
                        return;
                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "SubTexture")
                {
                    subTextureExist = true;
                    if (fullTextureAtlas != null)
                    {
                        // Read all SubTexture attributes
                        string? name = xmlReader.GetAttribute("name");
                        if (!xmlReader.HasAttributes || name == null || name == string.Empty) 
                        {
                            Console.WriteLine("Attribute 'name' for SubTexture does not exist.");
                            continue;
                        }
                        if (!xmlReader.HasAttributes || !Int32.TryParse(xmlReader.GetAttribute("x"), out int x))
                        {
                            Console.WriteLine("Attribute 'x' for SubTexture does not exist or not a number.");
                            continue;
                        }
                        if (!xmlReader.HasAttributes || !Int32.TryParse(xmlReader.GetAttribute("y"), out int y))
                        {
                            Console.WriteLine("Attribute 'y' for SubTexture does not exist or not a number.");
                            continue;
                        }
                        if (!xmlReader.HasAttributes || !Int32.TryParse(xmlReader.GetAttribute("width"), out int width))
                        {
                            Console.WriteLine("Attribute 'width' for SubTexture does not exist or not a number.");
                            continue;
                        }
                        if (!xmlReader.HasAttributes || !Int32.TryParse(xmlReader.GetAttribute("height"), out int height))
                        {
                            Console.WriteLine("Attribute 'height' for SubTexture does not exist or not a number.");
                            continue;
                        }

                        // Try to read SubTexture from TextureAtlas and write to file
                        try
                        {
                            Rectangle crop = new Rectangle(x, y, width, height);
                            Bitmap subTexture = new Bitmap(crop.Width, crop.Height);
                            using (var gr = Graphics.FromImage(subTexture))
                            {
                                gr.DrawImage(fullTextureAtlas, new Rectangle(0, 0, subTexture.Width, subTexture.Height), crop, GraphicsUnit.Pixel);
                            }
                            subTexture.Save(directory + name + ".png", ImageFormat.Png);
                        }
                        catch (Exception imageException)
                        {
                            Console.WriteLine(imageException.Message);
                        }
                    }
                }
            }
            if (!textureAtlasExist)
            {
                Console.WriteLine("Element TextureAtlas does not exist. Please, specify correct xml file.");
            }
            else if (!subTextureExist)
            {
                Console.WriteLine("No one element SubTexture does not exist. Please, specify correct xml file.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}