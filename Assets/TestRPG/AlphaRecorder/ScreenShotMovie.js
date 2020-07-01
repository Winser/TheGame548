import System.IO;

var folder = "ScreenshotFolder";
var frameRate = 25;
var framesToCapture = 25;

private var frame = 0;

private var realFolder = "";

function Start () {
    // Set the playback framerate!
    // (real time doesn't influence time anymore)
    Time.captureFramerate = frameRate;

    // Find a folder that doesn't exist yet by appending numbers!
    realFolder = folder;
    count = 1;
    while (System.IO.Directory.Exists(realFolder)) {
        realFolder = folder + count;
        count++;
    }
    // Create the folder
    System.IO.Directory.CreateDirectory(realFolder);    
}

function Update() {
	Capture();
}

function Capture () {
	if(frame < framesToCapture) {	
    	var name = String.Format("{0}/{1:D04} shot.png", realFolder, Time.frameCount );
  		yield WaitForEndOfFrame();
    
	    // Create a texture the size of the screen, RGB24 format
    	var width = Screen.width;
    	var height = Screen.height;
	    var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
	    // Read screen contents into the texture
	    tex.ReadPixels(Rect(0, 0, width, height), 0, 0);
	    tex.Apply();

	    var tex2 = new Texture2D(width / 2, height, TextureFormat.ARGB32, false);
	
		width = width / 2;
	    for (var y : int = 0; y < tex2.height; ++y) {
	    	for (var x : int = 0; x < tex2.width; ++x) {
	        	var alpha = tex.GetPixel(x + width, y).r - tex.GetPixel(x, y).r;
	        	alpha = 1.0 - alpha;
	        	if(alpha == 0) {
	        		color = Color.clear;
	        	} 
	        	else {
					color = tex.GetPixel(x, y) / alpha;
		        }
	            color.a = alpha;
	            tex2.SetPixel(x, y, color);
	        }
	    }
	    
	    pngShot = tex2.EncodeToPNG();
	    Destroy(tex);
	    Destroy(tex2);
	
	    File.WriteAllBytes(name, pngShot);
	    
	    Debug.Log("Frame " +frame);
		frame++;
    }
}
 