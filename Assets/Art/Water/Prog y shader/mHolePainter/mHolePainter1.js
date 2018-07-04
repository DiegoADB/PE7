// mHolePainter v1.0 - mgear - http://unitycoder.com/blog
// shader is from: http://www.unifycommunity.com/wiki/index.php?title=Dissolve_With_Texture

private var texture:Texture2D;

function Start () 
{
	// Create a new texture and assign it to the renderer's material
	texture = new Texture2D(64, 32);
	// set texture in the inspector slot
	GetComponent.<Renderer>().material.SetTexture("_SliceGuide", texture);
	
	// Fill the texture with white (you could also paint it black, then draw with white)
	for (var y : int = 0; y < texture.height; ++y) 
	{
		for (var x : int = 0; x < texture.width; ++x) 
		{
			texture.SetPixel (x, y, Color.white);
		}
	}
	// Apply all SetPixel calls
	texture.Apply();
}


function Update () 
{
	// rotate ball with A/D keys
	if (Input.GetKey ("a"))
	{
		transform.Rotate(Vector3(0,30,0) * Time.deltaTime);
	}
	if (Input.GetKey ("d"))
	{
		transform.Rotate(Vector3(0,-30,0) * Time.deltaTime);
	}

	// actual drawing to texture, taken from unity docs, Only works with objects with MESH collider
	// Only when we press the mouse
	if (!Input.GetMouseButton (0)) return;
	
	// Only if we hit something, do we continue
	var hit : RaycastHit;
	if (!Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), hit)) return;
	
	// Just in case, also make sure the collider also has a renderer
	// material and texture. Also we should ignore primitive colliders.
	var renderer : Renderer = hit.collider.GetComponent.<Renderer>();
	var meshCollider = hit.collider as MeshCollider;
	if (renderer == null || renderer.sharedMaterial == null ||texture == null || meshCollider == null) return;
	
	// Now draw a pixel where we hit the object
	var tex : Texture2D = texture;
	var pixelUV = hit.textureCoord;
	pixelUV.x *= tex.width;
	pixelUV.y *= tex.height;
	
	// add black spot, which is then transparent in the shader
	tex.SetPixel(pixelUV.x, pixelUV.y, Color.black);
	tex.Apply();

}