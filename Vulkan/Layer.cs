﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Vulkan {
    public unsafe partial class Layer {
        private Layer() { }

        public static VkResult EnumerateInstanceLayerProperties(out VkLayerProperties[] layerProperties) {
            UInt32 count;
            VkResult result = vkAPI.vkEnumerateInstanceLayerProperties(&count, null).Check();
            layerProperties = new VkLayerProperties[count];
            fixed (VkLayerProperties* pointer = layerProperties) {
                result = vkAPI.vkEnumerateInstanceLayerProperties(&count, pointer).Check();
            }

            return result;
        }
    }
}
