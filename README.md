# Lasse's differential compression algorithm

Target data:
- Multiple large high-entropy files that share data in common.
- Multiple raw disk images created from the same machine but at different times.

Doesn't work well with:
- Small files

Requirements:
- Files need to be the same size.
- More than one file is needed to compress (the base file left untouched and is used as reference or dictionary to rebuild the other files)

What will be implemented later?
1. Support for files smaller than the base file. (Like incomplete disk images)
2. Tree-like layering instead of linear layering. Can be used to archive multiple files on the same base. (This can already be done but only manually, the stack needs to be recalculated for each file on the same layer)
3. Read and write stream support for compressed files to seek data in files without needing to decompress the entire file. This is simple to implement but takes time.
