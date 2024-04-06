
// FROM: https://www.shadertoy.com/view/ttc3zr

//------------------------------------------------------------------------------

uint murmurHash12(int2 src) {
    const uint M = 0x5bd1e995u;
    uint h = 1190494759u;
    src *= M; src ^= src>>24u; src *= M;
    h *= M; h ^= src.x; h *= M; h ^= src.y;
    h ^= h>>13u; h *= M; h ^= h>>15u;
    return h;
}

// 1 output, 2 inputs
float hash12(int2 src) {
    uint h = murmurHash12((src));
    return asfloat(h & 0x007fffffu | 0x3f800000u) - 1.0;
}

//------------------------------------------------------------------------------

uint murmurHash13(int3 src) {
    const uint M = 0x5bd1e995u;
    uint h = 1190494759u;
    src *= M; src ^= src>>24u; src *= M;
    h *= M; h ^= src.x; h *= M; h ^= src.y; h *= M; h ^= src.z;
    h ^= h>>13u; h *= M; h ^= h>>15u;
    return h;
}

// 1 output, 3 inputs
float hash13(int3 src) {
    uint h = murmurHash13((src));
    return asfloat(h & 0x007fffffu | 0x3f800000u) - 1.0;
}

//------------------------------------------------------------------------------
