float4x4 world;
float4x4 view;
float4x4 projection;
float3 cameraPosition;

//light properties
float3 lightPosition;
float4x4 lightRotation;
float4 ambientLightColor;
float4 diffuseLightColor;
float4 specularLightColor;

float3 LightDirection = float3(0, 1, 0);
float ConeAngle = 10;
float LightFalloff = 1;

//material properties
float specularPower;
float specularIntensity;

//flashlight properties
float FlashLightOn = true;
float FullStrengthDistance;
float MaxDistance;

float FogStart = 5;
float FogEnd   = 10;
bool  FogEnabled = true;


//////////////////////////////////////////////////////////////
// Example 2.1                                              //
//                                                          //
// This structure will allow the vertex shader to output    //
// normal and position values in TEXCOORD registers.  Like  //
// colors, values in TexCoords are interpolated in screen   //
// space so that each pixel will have properly interpolated //
// normal and world coordinates.  This will enable us to    //
// do the Phong lighting calculation at each pixel for      //
// a much smoother result.                                  //
////////////////////////////////////////////////////////////// 
struct VertexShaderOutputPerVertexDiffuse
{
	float4 Position : POSITION;
	float3 WorldNormal : TEXCOORD0;
	float3 WorldPosition : TEXCOORD1;
	float4 Color : COLOR0;
};

struct VertexShaderOutputPerPixelDiffuse
{
	float4 Position : POSITION;
	float3 WorldNormal : TEXCOORD0;
	float3 WorldPosition : TEXCOORD1;
	float2 TextureCoordinate : TEXCOORD2;
};

struct PixelShaderInputPerVertexDiffuse
{
	float3 WorldNormal : TEXCOORD0;
	float3 WorldPosition : TEXCOORD1;
	float4 Color: COLOR0;
	float2 TextureCoordinate : TEXCOORD2;
};

struct PixelShaderInputPerPixelDiffuse
{
	float3 WorldNormal : TEXCOORD0;
	float3 WorldPosition : TEXCOORD1;
	float4 Color: COLOR0;
	float2 TextureCoordinate : TEXCOORD2;
};

texture ModelTexture;
sampler2D textureSampler = sampler_state {
	Texture = (ModelTexture);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};


VertexShaderOutputPerVertexDiffuse PerVertexDiffuseVS(
	float3 position : POSITION,
	float3 normal : NORMAL)
{
	VertexShaderOutputPerVertexDiffuse output;

	//generate the world-view-projection matrix
	float4x4 wvp = mul(mul(world, view), projection);

		//transform the input position to the output
		output.Position = mul(float4(position, 1.0), wvp);


	//////////////////////////////////////////////////////////////
	// Example 2.2                                              //
	//                                                          //
	// Here we calculate the world-space components for use     //
	// in the lighting calculations, though this time the       //
	// normal and position are being stored in shader outputs.  //
	//////////////////////////////////////////////////////////////
	output.WorldNormal = mul(normal, world);
	float4 worldPosition = mul(float4(position, 1.0), world);
		output.WorldPosition = worldPosition / worldPosition.w;

	//calculate diffuse component
	float3 directionToLight = normalize(lightPosition - output.WorldPosition);
		float diffuseIntensity = saturate(dot(directionToLight, output.WorldNormal));
	float4 diffuse = diffuseLightColor * diffuseIntensity;

		//////////////////////////////////////////////////////////////
		// Example 2.3                                              //
		//                                                          //
		// Specular light will be added in in the pixel shader.     //
		//////////////////////////////////////////////////////////////
		output.Color = diffuse + ambientLightColor;


	//return the output structure
	return output;
}

float4 PhongPS(PixelShaderInputPerVertexDiffuse input) : COLOR
{

	//////////////////////////////////////////////////////////////
	// Example 2.4                                              //
	//                                                          //
	// This is the same set of equations used in the            //
	// Phong vertex shader implementation.  However, the        //
	// inputs come from the interpolated values in the          //
	// TEXCOORD registers.  The calculations are applied        //
	// on each pixel, giving a much smoother, more natural      //
	// lighting effect.                                         //
	//////////////////////////////////////////////////////////////
	float3 directionToLight = normalize(lightPosition - input.WorldPosition);
	float3 reflectionVector = normalize(reflect(-directionToLight, input.WorldNormal));
	float3 directionToCamera = normalize(cameraPosition - input.WorldPosition);

	//calculate specular component
	float4 specular = specularLightColor * specularIntensity *
	pow(saturate(dot(reflectionVector, directionToCamera)),
	specularPower);


	//////////////////////////////////////////////////////////////
	// Example 2.5                                              //
	//                                                          //
	// The Diffuse and ambient components are already part of   //
	// the input color.  At this point, only the specular color //
	// needs to be addded.                                      //
	//////////////////////////////////////////////////////////////
	float4 color = input.Color + specular;
		color.a = 1.0;

	return color;
}

//////////////////////////////////////////////////////////////
// Example 2.6                                              //
//                                                          //
// This vertex shader simply sets the output registers so   // 
// that all lighting will be calculated per pixel.          //
//////////////////////////////////////////////////////////////
VertexShaderOutputPerPixelDiffuse PerPixelDiffuseVS(
	float3 position : POSITION,
	float3 normal : NORMAL,
	float3 texcoord : TEXCOORD)
{
	VertexShaderOutputPerPixelDiffuse output;

	//generate the world-view-projection matrix
	float4x4 wvp = mul(mul(world, view), projection);

		//transform the input position to the output
		output.Position = mul(float4(position, 1.0), wvp);

	output.WorldNormal = mul(normal, world);
	float4 worldPosition = mul(float4(position, 1.0), world);
		output.WorldPosition = worldPosition / worldPosition.w;

	output.TextureCoordinate = texcoord;
	//return the output structure
	return output;
}

//////////////////////////////////////////////////////////////
// Example 2.8                                              //
//                                                          //
// The diffuse-only pixel shader calculates diffuse light   //
// per-pxiel which reduces certain kinds of color artifacts //
// that appear on smooth curved surfaces.                   //
//////////////////////////////////////////////////////////////
float4 DiffuseOnlyPS(PixelShaderInputPerPixelDiffuse input) : COLOR
{
	//calculate per-pixel diffuse
	float3 directionToLight = normalize(lightPosition - input.WorldPosition);
	float diffuseIntensity = saturate(dot(directionToLight, input.WorldNormal));
	float4 diffuse = diffuseLightColor;

		float4 color = diffuse + ambientLightColor;
		color.a = 1.0;

	return color;
}

//////////////////////////////////////////////////////////////
// Example 2.9                                              //
//                                                          //
// Diffuse + phong in the pixel shader gives the best-      //
// looking results, but is the most GPU intensive of the    //
// techniques in this sample, since more pixels are being   //
// calculated than vertices for our very simple scene.      //
//////////////////////////////////////////////////////////////
float4 DiffuseAndPhongPS(PixelShaderInputPerPixelDiffuse input) : COLOR
{
	//calculate per-pixel diffuse
	float3 directionToLight = normalize(lightPosition - input.WorldPosition);
	float diffuseIntensity = 0;
	float distanceToLight = distance(lightPosition, input.WorldPosition);
	float4 diffuse = 0;

	float3 spotCenterVec = normalize(mul(LightDirection, -lightRotation));
	//calculate the angle between the spot center and the direction to light
	float3 dirLight = normalize(lightPosition - input.WorldPosition);
	//Check if the angle is within the cone's angle
	float angle = degrees(acos(dot(dirLight, spotCenterVec)));
	//If it is set the diffuse to 1
	if (angle < ConeAngle && FlashLightOn)
	{
		diffuseIntensity = 1 - (angle / ConeAngle);
		diffuse = diffuseLightColor * diffuseIntensity;

	}
	//float4 diffuse = diffuseLightColor * saturate(dot(directionToLight, normalize(input.WorldNormal))) * att;

	//calculate Phong components per-pixel
	float3 reflectionVector = normalize(reflect(-directionToLight, input.WorldNormal));
	float3 directionToCamera = normalize(cameraPosition - input.WorldPosition);

	//calculate specular component
	float4 specular = specularLightColor * specularIntensity *
		pow(saturate(dot(reflectionVector, directionToCamera)),
		specularPower);

		//calculate texture color
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
	textureColor.a = 1;

	

	float4 texColor = textureColor;
	
	float fog = 1;
	if (FogEnabled)
	{
			fog = 1 - (distanceToLight / FogEnd);
	}

	//texColor *= diffuseIntensity;
	//texColor.a = ambience.a;
	//all color components are summed in the pixel shader
	float4 color = (texColor * ambientLightColor + diffuse) * fog;//saturate(texColor + diffuse); // + ambientLightColor);// +specular);//specular + diffuse + ambientLightColor;
	color.a = 1.0;
	
	return color;
}



technique PerPixelDiffuse
{

	pass P0
	{
		//set the VertexShader state to the basic
		//vertex shader that will set up inputs for
		//the pixel shader
		VertexShader = compile vs_2_0 PerPixelDiffuseVS();

		//set the PixelShader state to the diffuse only pixel shader          
		PixelShader = compile ps_2_0 DiffuseOnlyPS();
	}
}

technique PerVertexDiffuseAndPerPixelPhong
{

	pass P0
	{
		//Per-vertex diffuse calculation and preparation of inputs
		//for the phong pixel shader
		VertexShader = compile vs_2_0 PerVertexDiffuseVS();

		//set the pixel shader to the per-pixel phong function      
		PixelShader = compile ps_2_0 PhongPS();
	}
}

technique PerPixelDiffuseAndPhong
{

	pass P0
	{
		//set the VertexShader state to the basic
		//vertex shader that will set up inputs for
		//the pixel shader
		VertexShader = compile vs_2_0 PerPixelDiffuseVS();

		//set the PixelShader state to the complete Phong Shading
		//implementation of the pixel shader       
		PixelShader = compile ps_2_0 DiffuseAndPhongPS();
	}
}