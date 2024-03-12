using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ManagedLDiff
{
    public class LDiffStream : Stream
    {
        public FileStream BaseFileStream { get; private set; }
        public FileStream HeaderFileStream { get; private set; }
        public FileStream OutFileStream { get; private set; }
        public List<FileStream> DiffStack { get; private set; }
        public List<FileStream> UndiffStack { get; private set; }

        private List<long>[] DifferOffsets { get; set; }
        private List<long>[] DifferLengths { get; set; }

        private void AllocateDifferBuffers(int stackSize)
        {
            try
            {
                DifferOffsets = new List<long>[stackSize];
                DifferLengths = new List<long>[stackSize];
                for (int i = 0; i < stackSize; i++)
                {
                    DifferOffsets[i] = new List<long>();
                    DifferLengths[i] = new List<long>();
                }
            }
            catch { throw; }
        }

        public LDiffStream(string baseFileName, string outFileName)
        {
            string headerExt = ".diffh";
            string outExt = ".diff" + DiffStack.Count;
            BaseFileStream = File.Open(baseFileName, FileMode.Open, FileAccess.Read);
            HeaderFileStream = File.Open(Path.ChangeExtension(outFileName, headerExt), FileMode.Open, FileAccess.ReadWrite);
            OutFileStream = File.Open(Path.ChangeExtension(outFileName, outExt), FileMode.Open, FileAccess.ReadWrite);
        }

        public override bool CanRead
        {
            get
            {
                return BaseFileStream.CanRead &&
                    UndiffStack.Count(file => file.CanRead == false) == 0 &&
                    DiffStack.Count(file => file.CanRead == false) == 0;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return BaseFileStream.CanSeek &&
                    UndiffStack.Count(file => file.CanSeek == false) == 0 &&
                    DiffStack.Count(file => file.CanSeek == false) == 0; ;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return HeaderFileStream.CanWrite && OutFileStream.CanWrite &&
                    UndiffStack.Count(file => file.CanRead == false) == 0; // NEed to read the undiffed of underlying layers.
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            byte[][] readBuffer = new byte[UndiffStack.Count][];
            for (int layer = UndiffStack.Count - 1; layer >= 0; layer--)
            {
                readBuffer[layer] = new byte[buffer.Length];
                //byte[] layerBuffer = ;
                UndiffStack[layer].Read(readBuffer[layer], offset, count);
            }

            for (int i = offset; i < count; i++)
            {
                //if(
                //for (int layer = UndiffStack.Count - 1; layer >= 0; layer--)
                //{
                //    if()
                //}
            }
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            try
            {
                UndiffStack.ForEach(file => file.Seek(offset, origin));
                return BaseFileStream.Seek(offset, origin);
            }
            catch { throw; }
        }

        public override void SetLength(long value)
        {
            try
            {
                UndiffStack.ForEach(file => file.SetLength(value));
                BaseFileStream.SetLength(value);
            }
            catch { throw; }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // If data not exist in stream at top of stack, check lower and lower until base. If not in base, write to new stream, record in database and perhaps write to header.
            throw new NotImplementedException();
        }
    }

    public class LUndiffStream : Stream
    {
        public FileStream BaseFileStream { get; private set; }
        public FileStream HeaderFileStream { get; private set; }
        public FileStream OutFileStream { get; private set; }
        public List<FileStream> DiffStack { get; private set; }
        public List<FileStream> UndiffStack { get; private set; }

        public List<long>[] DifferOffsets { get; private set; }
        public List<long>[] DifferLengths { get; private set; }

        private void AllocateDifferBuffers(int stackSize)
        {
            try
            {
                DifferOffsets = new List<long>[stackSize];
                DifferLengths = new List<long>[stackSize];
                for (int i = 0; i < stackSize; i++)
                {
                    DifferOffsets[i] = new List<long>();
                    DifferLengths[i] = new List<long>();
                }
            }
            catch { throw; }
        }

        public LUndiffStream(string baseFileName, string outFileName)
        {
            string headerExt = ".diffh";
            string outExt = ".diff" + DiffStack.Count;
            BaseFileStream = File.Open(baseFileName, FileMode.Open, FileAccess.Read);
            HeaderFileStream = File.Open(Path.ChangeExtension(outFileName, headerExt), FileMode.Open, FileAccess.ReadWrite);
            OutFileStream = File.Open(Path.ChangeExtension(outFileName, outExt), FileMode.Open, FileAccess.ReadWrite);
        }

        public override bool CanRead
        {
            get
            {
                //bool canRead = false;
                return BaseFileStream.CanRead &&
                    UndiffStack.Count(file => file.CanRead == false) == 0 &&
                    DiffStack.Count(file => file.CanRead == false) == 0;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return BaseFileStream.CanSeek &&
                    UndiffStack.Count(file => file.CanSeek == false) == 0 &&
                    DiffStack.Count(file => file.CanSeek == false) == 0; ;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return HeaderFileStream.CanWrite && OutFileStream.CanWrite &&
                    UndiffStack.Count(file => file.CanRead == false) == 0; // NEed to read the undiffed of underlying layers.
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException("The provided buffer is null.");
            byte[][] readBuffer = new byte[UndiffStack.Count][];
            for (int layer = UndiffStack.Count - 1; layer >= 0; layer--)
            {
                readBuffer[layer] = new byte[buffer.Length];
                //byte[] layerBuffer = ;
                UndiffStack[layer].Read(readBuffer[layer], offset, count);
            }

            for (int i = offset; i < count; i++)
            {
                //if(
                //for (int layer = UndiffStack.Count - 1; layer >= 0; layer--)
                //{
                //    if()
                //}
            }
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            try
            {
                UndiffStack.ForEach(file => file.Seek(offset, origin));
                return BaseFileStream.Seek(offset, origin);
            }
            catch { throw; }
        }

        public override void SetLength(long value)
        {
            try
            {
                UndiffStack.ForEach(file => file.SetLength(value));
                BaseFileStream.SetLength(value);
            }
            catch { throw; }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // If data not exist in stream at top of stack, check lower and lower until base. If not in base, write to new stream, record in database and perhaps write to header.
            throw new NotImplementedException();
        }
    }
}
