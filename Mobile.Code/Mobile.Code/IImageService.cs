using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Code
{
    public interface IImageService 
    {
        
        byte[] ResizeTheImage(byte[] imageData, float width, float height);
    
    }
}
