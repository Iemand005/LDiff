using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ManagedLDiff
{
    public class LDiffTool
    {
        public IndexingMethods IndexingMethod { get; set; }

        private FileStream HeaderFileStream { get; set; }
        private FileStream BaseFileStream { get; set; }
        private Dictionary<FileStream, FileStream> DiffStack { get; set; }

        private List<long>[] DifferOffsets { get; set; }
        private List<long>[] DifferLengths { get; set; }

        public LDiffTool(string baseFileName)
        {
            try
            {
                IndexingMethod = IndexingMethods.Origin;
                DiffStack = new Dictionary<FileStream, FileStream>();
                BaseFileStream = File.Open(baseFileName, FileMode.Open, FileAccess.Read);
                HeaderFileStream = File.Open(Path.ChangeExtension(baseFileName, ".diffh"), FileMode.OpenOrCreate, FileAccess.Write);
            }
            catch { throw; }
        }

        private void InitializeHeader()
        {
            byte[] magicBytes = Encoding.ASCII.GetBytes("LDIH");
            byte version = 1;
            HeaderFileStream.Write(magicBytes, 0, magicBytes.Length);
            HeaderFileStream.WriteByte(version);
        }

        public void Stack(string fileName, string outFileName)
        {
            try
            {
                FileStream sourceFileStream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                if (sourceFileStream.Length != BaseFileStream.Length) throw new InvalidDataException("The stacked file size doesn't match the base file size.");
                DiffStack.Add(sourceFileStream, File.Open(outFileName, FileMode.Create, FileAccess.Write));
            }
            catch { throw; }
        }

        public void Stack(string fileName)
        {
            try { Stack(fileName, Path.ChangeExtension(fileName, ".diff" + DiffStack.Count)); }
            catch { throw; }
        }

        public void DiffLayered()
        {
            int chunkSize = 5;
            try
            {
                if (DiffStack.Count > 0)
                {
                    InitializeHeader();


                    DifferOffsets = new List<long>[DiffStack.Count];
                    DifferLengths = new List<long>[DiffStack.Count];

                    #region Build Header Map

                    byte[] magicBytes = Encoding.ASCII.GetBytes("LDIF");
                    byte version = 1;

                    long differOffset = 0;

                    int baseBytesRead;
                    int layerBytesRead;
                    byte[] baseBuffer = new byte[chunkSize];
                    byte[] layerBuffer = new byte[chunkSize];
                    bool[] diffing = new bool[DiffStack.Count];

                    byte[][] checksums = new byte[DiffStack.Count][];

                    for (byte i = 0; i < DiffStack.Count; i++)
                    {
                        DifferOffsets[i] = new List<long>();
                        DifferLengths[i] = new List<long>();
                        checksums[i] = new byte[4];
                        FileStream originalFileStrea = DiffStack.ElementAt(i).Key;
                        FileInfo fileInfo = new FileInfo(originalFileStrea.Name);
                        HeaderRegisterFile(fileInfo);
                        var diffFileStream = DiffStack.ElementAt(i).Value;
                        diffFileStream.Write(magicBytes, 0, 4);
                        diffFileStream.WriteByte(version);
                        diffFileStream.WriteByte(i);
                    }

                    long baseOffset = 0;

                    while ((baseBytesRead = BaseFileStream.Read(baseBuffer, 0, baseBuffer.Length)) > 0)
                    {
                        for (int stackLayer = 0; stackLayer < DiffStack.Count; stackLayer++)
                        {
                            var item = DiffStack.ElementAt(stackLayer);
                            FileStream layerFileStream = item.Key;
                            FileStream diffFileStream = item.Value;

                            layerBytesRead = layerFileStream.Read(layerBuffer, 0, layerBuffer.Length);

                            for (int bufferOffset = 0; bufferOffset < layerBytesRead; bufferOffset++)
                            {
                                long offset = baseOffset + bufferOffset;

                                // Here is where the byte comparison happens.
                                if (baseBuffer[bufferOffset] == layerBuffer[bufferOffset])
                                {
                                    if (diffing[stackLayer])
                                    {
                                        DifferLengths[stackLayer].Add(offset - differOffset);
                                        diffing[stackLayer] = false;
                                    }
                                }
                                //else if (offset == BaseFileStream.Length - 1) // End differ if end of stream.
                                //{
                                //    DifferLengths[stackLayer].Add(offset -differOffset);
                                //    diffFileStream.WriteByte(layerBuffer[bufferOffset]);
                                //}
                                else
                                {
                                    if (!diffing[stackLayer]) // The bytes differ and we're not diffing. Start diffing.
                                    {
                                        DifferOffsets[stackLayer].Add(offset);
                                        diffing[stackLayer] = true;
                                    }
                                    diffFileStream.WriteByte(layerBuffer[bufferOffset]);
                                    if (offset == BaseFileStream.Length - 1) // End differ if end of stream.
                                    {
                                        DifferLengths[stackLayer].Add(offset - differOffset +1);
                                    }
                                }
                                CheckSum(checksums[stackLayer], layerBuffer[bufferOffset], offset);
                            }

                            Buffer.BlockCopy(layerBuffer, 0, baseBuffer, 0, chunkSize);
                        }
                        baseOffset += baseBytesRead;
                    }
                    #endregion

                    #region Build Header
                    for (int stackLayer = 0; stackLayer < DiffStack.Count; stackLayer++)
                    {
                        var item = DiffStack.ElementAt(stackLayer);
                        var diffFileStream = item.Value;
                        if (DifferOffsets[stackLayer].Count != DifferLengths[stackLayer].Count) throw new Exception("The amount of offsets and lengths mismatch.");
                        for (int index = 0; index < DifferOffsets[stackLayer].Count; index++)
                        {
                            diffFileStream.Write(BitConverter.GetBytes(DifferOffsets[stackLayer][index]), 0, 8);
                            diffFileStream.Write(BitConverter.GetBytes(DifferLengths[stackLayer][index]), 0, 8);
                        }
                        diffFileStream.Write(BitConverter.GetBytes(DifferOffsets[stackLayer].Count), 0, 4);
                        diffFileStream.Write(checksums[stackLayer], 0, 4);
                    }
                    #endregion
                }
            }
            catch { throw; }

            CloseAll();
        }

        private void HeaderRegisterFile(FileInfo fileInfo)
        {
            try
            {
                byte[] fileNameBuffer = Encoding.Unicode.GetBytes(fileInfo.FullName);
                int fileNameLength = fileInfo.FullName.Length;
                if (fileNameLength >= 256) throw new ArgumentException("File path is too long.");
                HeaderFileStream.WriteByte((byte)fileNameLength);
                HeaderFileStream.Write(fileNameBuffer, 0, fileNameBuffer.Length);
                //throw new NotImplementedException();
            }
            catch { throw; }
        }
        
        /// <summary>
        /// Increments the chesksum with one byte.
        /// </summary>
        /// <param name="source">The checksum to increment.</param>
        /// <param name="next">The byte to perform the calculation with.</param>
        /// <param name="offset">The position of the current byte.</param>
        public static void CheckSum(byte[] source, byte next, long offset)
        {
            if (source == null) throw new Exception("The provided source is invalid.");
            long a = offset - 1;
            byte calc = (byte)((source[(a < 0 ? -a : a) % source.Length] ^ next) + next - (offset ^ next));
            source[offset % source.Length] = calc;
        }

        /// <summary>
        /// Closes all streams.
        /// </summary>
        private void CloseAll()
        {
            BaseFileStream.Close();
            HeaderFileStream.Close();
            foreach (var stream in DiffStack)
            {
                stream.Key.Close();
                stream.Value.Close();
            }
        }

        public void Close()
        {
            CloseAll();
        }

    }

    public class LUndiffTool
    {
        public IndexingMethods IndexingMethod { get; set; }

        public string BaseFileName { get; set; }

        public FileStream BaseFileStream { get; set; }
        public List<FileStream> UndiffStack { get; private set; }

        public List<long>[] DifferOffsets { get; private set; }
        public List<long>[] DifferLengths { get; private set; }

        public LUndiffTool(string baseFileName)
        {
            IndexingMethod = IndexingMethods.Origin;
            UndiffStack = new List<FileStream>();
            BaseFileName = baseFileName;
        }

        public void Stack(string outFileName, bool customName = false)
        {
            try
            {
                UndiffStack.Add(File.Open(customName ? outFileName : Path.ChangeExtension(outFileName, "diff" + UndiffStack.Count), FileMode.Open, FileAccess.Read));
            }
            catch { throw; }
        }

        public bool UndiffLayered(int layer, string outFileName)
        {
            bool success = false;
            try
            {
                byte[] validChecksum = new byte[4];
                byte[] checksum = new byte[4];
                BaseFileStream = File.Open(BaseFileName, FileMode.Open, FileAccess.Read);
                var outFileStream = File.Open(outFileName, FileMode.OpenOrCreate, FileAccess.Write);

                int stackSize = layer + 1;
                DifferOffsets = new List<long>[stackSize];
                DifferLengths = new List<long>[stackSize];

                for (int i = 0; i < stackSize; i++)
                {
                    DifferOffsets[i] = new List<long>();
                    DifferLengths[i] = new List<long>();

                    FileStream diffedFile = UndiffStack[i];
                    diffedFile.Seek(0, SeekOrigin.Begin);

                    byte[] magicBytes = new byte[4];
                    diffedFile.Read(magicBytes, 0, 4);
                    string magicString = Encoding.ASCII.GetString(magicBytes);

                    int ver = diffedFile.ReadByte();

                    if (!(magicString == "LDIF" && ver == 1)) throw new InvalidDataException("Invalid file format!");

                    diffedFile.Seek(-8, SeekOrigin.End);

                    byte[] footerBuffer = new byte[4];
                    diffedFile.Read(footerBuffer, 0, 4);
                    diffedFile.Read(validChecksum, 0, 4);
                    int headerSize = BitConverter.ToInt32(footerBuffer, 0);
                    if (headerSize == 0) throw new InvalidDataException("The file contains no data.");

                    diffedFile.Seek(-((headerSize * 16) + 8), SeekOrigin.End);

                    byte[] differDataBuffer = new byte[16];
                    for (int headerChunkIndex = 0; headerChunkIndex < headerSize; headerChunkIndex++)
                    {
                        diffedFile.Read(differDataBuffer, 0, 16);
                        DifferOffsets[i].Add(BitConverter.ToInt64(differDataBuffer, 0));
                        DifferLengths[i].Add(BitConverter.ToInt64(differDataBuffer, 8));
                    }

                    diffedFile.Seek(6, SeekOrigin.Begin);
                }

                int chunkSize = 5;

                int baseBytesRead;
                byte[] baseBuffer = new byte[chunkSize];
                byte[] layerBuffer = new byte[chunkSize];
                long kankerOffset = 0;


                long nextDifferOffset = DifferOffsets[layer][0];
                while ((baseBytesRead = BaseFileStream.Read(baseBuffer, 0, baseBuffer.Length)) > 0)
                {
                    long offsenewt = kankerOffset + baseBytesRead;

                    // If not hit the next indexed offset, just copy the base bytes. If hit the next offset, remember its length and the next offset.
                    // If on top of the next offset and layer of the next offset is higher, switch to that and remember its length
                    // If 
                    bool found = false;
                    for (int currentOffset = 0; currentOffset < baseBuffer.Length; currentOffset++)
                    {
                        // Byte by byte loop!

                        byte current = baseBuffer[currentOffset];
                        long offset = currentOffset + kankerOffset;
                        if (offset >= BaseFileStream.Length) break;
                        for (int layerIndex = layer; layerIndex >= 0; layerIndex--)
                        {
                            //var a = UndiffStack[layerIndex];
                            for (int offsetIndex = 0; offsetIndex < DifferOffsets[layerIndex].Count; offsetIndex++)
                            {
                                long differOffset = DifferOffsets[layerIndex][offsetIndex];
                                long differLength = DifferLengths[layerIndex][offsetIndex];

                                long differSize = differLength - differOffset;

                                if (offset >= differOffset && offset < differLength)
                                {
                                    current = (byte)UndiffStack[layerIndex].ReadByte();
                                    if (!found)
                                    {
                                        outFileStream.WriteByte(current);
                                        LDiffTool.CheckSum(checksum, current, offset);
                                    }
                                    found = true;
                                }
                            }
                        }
                        if (!found)
                        {
                            outFileStream.WriteByte(current);
                            LDiffTool.CheckSum(checksum, current, offset);
                        }
                        found = false;
                    }
                    kankerOffset = offsenewt;
                }
                outFileStream.Close();
                BaseFileStream.Close();

                success = checksum.SequenceEqual(validChecksum);
            }
            catch { throw; }
            return success;
        }

    }

    public enum IndexingMethods
    {
        Origin,
        LastStart,
        LastEnd
    }

    public enum IndexingSize
    {
        None = 0,
        Byte = 1,
        Short = 2,
        Normal = 4,
        Long = 8
    }
}
