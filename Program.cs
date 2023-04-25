using System.Xml;

if (args == null || args.Length < 1 || args[0] == null) {
    Console.WriteLine("Please, use path to the xml file as first and single parameter when you call this application.");
    return;
}

if (!File.Exists(args[0])) {
    Console.WriteLine("File '{0}' does not exist. Please, specify correct path to file.", args[0]);
    return;
}

try {
    XmlReader xmlReader = XmlReader.Create(args[0]);
    byte[] imageBytes;
    bool textureAtlasExist = false;
    bool subTextureExist = false;
    bool imageLoaded = false;
    while (xmlReader.Read()) {
        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "TextureAtlas") {
            textureAtlasExist = true;
            string? imagePath = xmlReader.GetAttribute("imagePath");
            if (xmlReader.HasAttributes && imagePath != null && imagePath != string.Empty) {
                string fullImagePath = imagePath.Contains(Path.DirectorySeparatorChar) ? imagePath : Path.GetDirectoryName(args[0]) + Path.DirectorySeparatorChar + imagePath;
                if (File.Exists(fullImagePath)) {
                    try {
                        // TODO read from file to imageBytes
                        imageLoaded = true;
                        Console.WriteLine(fullImagePath);
                    } catch (Exception imageException) {
                        Console.WriteLine(imageException.Message);
                        return;
                    }
                } else {
                    Console.WriteLine("Image file '{0}' does not exist. Please, specify correct path to image file inside xml file.", fullImagePath);
                    return;
                }
            } else {
                Console.WriteLine("Attribute 'imagePath' for TextureAtlas does not exist. Please, specify correct xml file.", args[0]);
                return;
            }             
        } else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "SubTexture") {
            subTextureExist = true;
            if (imageLoaded) {
                // TODO read each element and create new image file. If error show it and go to next element. Do not exit 
            }
        }
    }
    if (!textureAtlasExist) {
        Console.WriteLine("Element TextureAtlas does not exist. Please, specify correct xml file.", args[0]);
    } else if (!subTextureExist) {
        Console.WriteLine("No one element SubTexture does not exist. Please, specify correct xml file.", args[0]);
    }
} catch (Exception ex) {
    Console.WriteLine(ex.Message);
}
