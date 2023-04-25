# TextureAtlasSplitter
Console application for split TextureAtlas on different images.

TextureAtlas is xml file which has next format: 

```
<TextureAtlas imagePath='atlas.png'>
    <SubTexture name='texture_1' x='0'  y='0' width='50' height='50'/>
    <SubTexture name='texture_2' x='50' y='0' width='20' height='30'/>
</TextureAtlas>
```

Please, use __path to the xml__ file as first and __single parameter__ when you call this application.