using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AlienBiomes
{
    public static class ABShaderPropertyIDs
    {
        private static readonly string HashOffsetName = "_HashOffset";

        public static int HashOffset = Shader.PropertyToID(HashOffsetName);
    }
}
