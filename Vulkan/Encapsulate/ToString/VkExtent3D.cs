﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Vulkan {
    public unsafe partial struct VkExtent3D {
        public override string ToString() {
            return $"w: {width}, h: {height}, d: {depth}";
        }

    }
}
