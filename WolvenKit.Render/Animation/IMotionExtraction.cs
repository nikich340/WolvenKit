using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolvenKit.Render.Animation
{
    public abstract class IMotionExtraction
    {

    }

    class CLineMotionExtraction : IMotionExtraction
    {
        //    <class Name="CLineMotionExtraction" BaseClass="IMotionExtraction">
        //    <property Name = "frames" Type="array:2,0,Vector" />
        //    <property Name = "times" Type="array:2,0,Float" />
    }

    class CLineMotionExtraction2 : IMotionExtraction
    {
        public float duration;
        public List<float> frames = new List<float>();
        public byte[] deltaTimes = new byte[] {0,0,0,0,0,0};
        public byte flags;
        //<class Name="CLineMotionExtraction2" BaseClass="IMotionExtraction">
        //    <property Name = "duration" Type="Float" />
        //    <property Name = "frames" Type="array:2,0,Float" />
        //    <property Name = "deltaTimes" Type="array:2,0,Uint8" />
        //    <property Name = "flags" Type="Uint8" />
    }
}
