# TiffReader
## 
## Installation

## Usage
### Converting all images in a tiff file to a RGB-jpeg
This will convert the images in a tiff file to RGB-jpeg files, no matter what original fileformat/color components was.  
This is added as easy way to get output that is readable from other packages.  
```c#
            using var stream = File.OpenRead(file);
            var reader = new TiffReader(stream);
            Console.WriteLine($"Extracting from : {file}");
            var fileNo = 0;
            foreach (var image in reader.Read())
            {
                using var outstream =  File.OpenWrite($"out-{fileNo++}.jpg");
                image.WriteToRgbJpeg(outstream);
            }
```
### Get raw, uncompressed data
You can get the raw, uncompressed data from the tiff file by using the RawData property.  
This gives you access to converting this to your file format of choice.  
Please note that this does not do any kind of color conversion, you will need to do that yourself in this case.  
```c#
            using var stream = File.OpenRead(file);
            var reader = new TiffReader(stream);
            Console.WriteLine($"Extracting from : {file}");
            var fileNo = 0;
            foreach (var image in reader.Read())
            {
                File.WriteAllBytes("file.data",image.RawData);
            }
```

## External Dependencies
Currently, Fantasista.TiffReader is using the following packages:  
 * BitMiracle.LibJpeg.NET (MIT License)
