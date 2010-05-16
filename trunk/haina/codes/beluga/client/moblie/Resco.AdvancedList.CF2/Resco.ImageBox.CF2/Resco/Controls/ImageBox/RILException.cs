namespace Resco.Controls.ImageBox
{
    using System;

    public class RILException : Exception
    {
        private ErrorCode _code;

        public RILException() : this(ErrorCode.Unexpected)
        {
        }

        private RILException(ErrorCode code) : base(GetMessage(code))
        {
            this._code = code;
        }

        public RILException(int code) : this(GetCode(code))
        {
        }

        private static ErrorCode GetCode(int code)
        {
            switch (((NativeCodes) code))
            {
                case NativeCodes.RIMGERR_OUT_OF_MEMORY:
                case NativeCodes.RIMGERR_SAVE_OUT_OF_MEMORY:
                    return ErrorCode.OutOfMemory;

                case NativeCodes.RIMGERR_WRONG_PATH:
                    return ErrorCode.WrongPath;

                case NativeCodes.RIMGERR_OPEN_FILE:
                    return ErrorCode.OpenFile;

                case NativeCodes.RIMGERR_CREATE_FILE:
                    return ErrorCode.CreateFile;

                case NativeCodes.RIMGERR_READ_FILE:
                    return ErrorCode.ReadFile;

                case NativeCodes.RIMGERR_WRITE_FILE:
                    return ErrorCode.WriteFile;

                case NativeCodes.RIMGERR_BAD_IMAGE_DATA:
                    return ErrorCode.BadImageData;

                case NativeCodes.RIMGERR_FORMAT_NOTSUPPORTED:
                case NativeCodes.RIMGERR_SAVE_NOT_SUPPORTED:
                    return ErrorCode.FormatUnsupported;

                case NativeCodes.RIMGERR_VERSION_NOTSUPPORTED:
                    return ErrorCode.VersionUnsupported;

                case NativeCodes.RIMGERR_COMP_NOTSUPPORTED:
                    return ErrorCode.CompressionUnsupported;

                case NativeCodes.RIMGERR_LZW_NOTSUPPORTED:
                    return ErrorCode.LzwUnsupported;

                case NativeCodes.RIMGERR_CMYK_LAB:
                    return ErrorCode.CMYKLabUnsupported;

                case NativeCodes.RIMGERR_BAD_COLOR_MODE:
                    return ErrorCode.ColorModeUnsupported;

                case NativeCodes.RIMGERR_SAVE_FAILED:
                    return ErrorCode.SaveFailed;
            }
            return ErrorCode.Unexpected;
        }

        private static string GetMessage(ErrorCode code)
        {
            switch (code)
            {
                case ErrorCode.OutOfMemory:
                    return "The application ran out of memory.";

                case ErrorCode.WrongPath:
                    return "Path to the file is invalid.";

                case ErrorCode.OpenFile:
                    return "An error occured while trying to open a file. Make sure that the file exists in the\r\n                specified location and that it is not locked by another process. If the file is on a removable\r\nstorage (such as a memory card), make sure the storage is properly installed.";

                case ErrorCode.CreateFile:
                    return "An error occured while trying to create a file. Make sure there is enough space in the \r\ntarget memory. If the target is a removable storage (such as a memory card), make sure the storage is properly installed.";

                case ErrorCode.ReadFile:
                    return "An error occured while trying to read the contents of a file. Make sure the file is not used\r\nby another process. If the file is on a removable storage (such as a memory card),\r\nmake sure the storage is properly installed.";

                case ErrorCode.WriteFile:
                    return "An error occured while trying to write the contents of a file.\r\nMake sure there is enough space in the target memory. If the target is a removable storage\r\n(such as a memory card), make sure the storage is properly installed.";

                case ErrorCode.SaveFailed:
                    return "An error occured while trying to save the contents of a file. Contact\r\ntechnical support at support_dev@resco.net.";

                case ErrorCode.BadImageData:
                    return "The image data is invalid. It may have become corrupted during an unreliable data transfer.";

                case ErrorCode.FormatUnsupported:
                    return "The specified file format is not supported.";

                case ErrorCode.ColorModeUnsupported:
                    return "The color mode of the image is not supported.";

                case ErrorCode.VersionUnsupported:
                    return "Version of the file format is not supported.";

                case ErrorCode.CompressionUnsupported:
                    return "Compression used in the image is not supported.";

                case ErrorCode.LzwUnsupported:
                    return "Lzw compression is not supported.";

                case ErrorCode.CMYKLabUnsupported:
                    return "Images with CMYK color palette are unsupported.";
            }
            return "Unexpected error occured.";
        }

        public ErrorCode Code
        {
            get
            {
                return this._code;
            }
        }

        public enum ErrorCode
        {
            Unexpected,
            OutOfMemory,
            WrongPath,
            OpenFile,
            CreateFile,
            ReadFile,
            WriteFile,
            SaveFailed,
            BadImageData,
            FormatUnsupported,
            ColorModeUnsupported,
            VersionUnsupported,
            CompressionUnsupported,
            LzwUnsupported,
            CMYKLabUnsupported
        }

        private enum NativeCodes
        {
            RIMGERR_32Bit_IMAGE = 0x2000000e,
            RIMGERR_ABORTED = 0x2000000f,
            RIMGERR_BAD_COLOR_MODE = 0x20000013,
            RIMGERR_BAD_IMAGE_DATA = 0x20000008,
            RIMGERR_BASE = 0x20000000,
            RIMGERR_CMYK_LAB = 0x2000000d,
            RIMGERR_COMP_NOTSUPPORTED = 0x2000000b,
            RIMGERR_CREATE_FILE = 0x20000005,
            RIMGERR_EXCEPTION = 0x20000002,
            RIMGERR_FORMAT_NOTSUPPORTED = 0x20000009,
            RIMGERR_INVALIDPARAMETER = 0x20000015,
            RIMGERR_LZW_NOTSUPPORTED = 0x2000000c,
            RIMGERR_NOMOREPAGES = 0x20000011,
            RIMGERR_NOTOPENED = 0x20000010,
            RIMGERR_OPEN_FILE = 0x20000004,
            RIMGERR_OUT_OF_MEMORY = 0x20000001,
            RIMGERR_READ_FILE = 0x20000006,
            RIMGERR_SAVE_FAILED = 0x20000016,
            RIMGERR_SAVE_NOT_SUPPORTED = 0x20000012,
            RIMGERR_SAVE_OUT_OF_MEMORY = 0x20000014,
            RIMGERR_VERSION_NOTSUPPORTED = 0x2000000a,
            RIMGERR_WRITE_FILE = 0x20000007,
            RIMGERR_WRONG_PATH = 0x20000003
        }
    }
}

