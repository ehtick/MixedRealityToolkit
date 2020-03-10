//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WaveEngine_MRTK_Demo.Effects
{
    using WaveEngine.Common.Graphics;
    using WaveEngine.Framework.Graphics;
    using WaveEngine.Framework.Graphics.Effects;
    using WaveEngine.Mathematics;
    
    
    public class HoloGraphic : WaveEngine.Framework.Graphics.MaterialDecorator
    {
        
        public HoloGraphic(WaveEngine.Framework.Graphics.Effects.Effect effect) : 
                base(new Material(effect))
        {
        }
        
        public HoloGraphic(WaveEngine.Framework.Graphics.Material material) : 
                base(material)
        {
        }
        
        public WaveEngine.Mathematics.Matrix4x4 Base_WorldViewProj
        {
            get
            {
                return this.material.CBuffers[0].GetBufferData<WaveEngine.Mathematics.Matrix4x4>(0);
            }
            set
            {
				this.material.CBuffers[0].SetBufferData(value, 0);
            }
        }
        
        public WaveEngine.Mathematics.Matrix4x4 Base_World
        {
            get
            {
                return this.material.CBuffers[0].GetBufferData<WaveEngine.Mathematics.Matrix4x4>(64);
            }
            set
            {
				this.material.CBuffers[0].SetBufferData(value, 64);
            }
        }
        
        public WaveEngine.Mathematics.Vector3 Matrices_Color
        {
            get
            {
                return this.material.CBuffers[1].GetBufferData<WaveEngine.Mathematics.Vector3>(0);
            }
            set
            {
				this.material.CBuffers[1].SetBufferData(value, 0);
            }
        }
        
        public float Matrices_Alpha
        {
            get
            {
                return this.material.CBuffers[1].GetBufferData<System.Single>(12);
            }
            set
            {
				this.material.CBuffers[1].SetBufferData(value, 12);
            }
        }
        
        public WaveEngine.Mathematics.Vector3 Matrices_InnerGlowColor
        {
            get
            {
                return this.material.CBuffers[1].GetBufferData<WaveEngine.Mathematics.Vector3>(16);
            }
            set
            {
				this.material.CBuffers[1].SetBufferData(value, 16);
            }
        }
        
        public float Matrices_InnerGlowAlpha
        {
            get
            {
                return this.material.CBuffers[1].GetBufferData<System.Single>(28);
            }
            set
            {
				this.material.CBuffers[1].SetBufferData(value, 28);
            }
        }
        
        public float Matrices_InnerGlowPower
        {
            get
            {
                return this.material.CBuffers[1].GetBufferData<System.Single>(44);
            }
            set
            {
				this.material.CBuffers[1].SetBufferData(value, 44);
            }
        }
        
        public float Matrices_MaxFingerDist
        {
            get
            {
                return this.material.CBuffers[1].GetBufferData<System.Single>(60);
            }
            set
            {
				this.material.CBuffers[1].SetBufferData(value, 60);
            }
        }
        
        public WaveEngine.Mathematics.Vector3 Matrices_FingerPosLeft
        {
            get
            {
                return this.material.CBuffers[1].GetBufferData<WaveEngine.Mathematics.Vector3>(32);
            }
            set
            {
				this.material.CBuffers[1].SetBufferData(value, 32);
            }
        }
        
        public WaveEngine.Mathematics.Vector3 Matrices_FingerPosRight
        {
            get
            {
                return this.material.CBuffers[1].GetBufferData<WaveEngine.Mathematics.Vector3>(48);
            }
            set
            {
				this.material.CBuffers[1].SetBufferData(value, 48);
            }
        }
    }
}
