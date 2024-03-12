# Lasse's differential compression algorithm

Target data:
- Multiple large high-entropy files that share data in common.
- Multiple raw disk images created from the same machine but at different times.

Doesn't work well with:
- Small files

Advantages over LZMA and RAR:
- No restriction on dictionary size.
- Speed.
- No increase in entropy, so compressed files can be extra compressed by being fed through RAR or LZMA to combine the efficiency.

Requirements:
- Files need to be the same size.
- More than one file is needed to compress (the base file left untouched and is used as reference or dictionary to rebuild the other files)

What will be implemented later?
1. Support for files smaller than the base file. (Like incomplete disk images)
2. Tree-like layering instead of linear layering. Can be used to archive multiple files on the same base. (This can already be done but only manually, the stack needs to be recalculated for each file on the same layer)
3. Read and write stream support for compressed files to seek data in files without needing to decompress the entire file. This is simple to implement but takes time.
4. Different methods of header notation. Offsets can be represented in 32bit signed integers for files smaller than 4GB to save space. Difference size can be limited to 32bit signed integers to have max difference sequence set to 4GB but potentially save header space.
5. Skipping of single matching bytes to avoid adding 16 bytes to the header for a single match. Requires a second buffer read with a counter that counts the amount of bytes where the data matches again.
