namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Runtime.InteropServices;

    internal class HTCGSensor : IDisposable
    {
        private IntPtr myHandle = HTCSensorOpen(HTCSensor.GSensor);

        public HTCGSensor()
        {
            IntPtr handle = CreateEvent(IntPtr.Zero, true, false, "HTC_GSENSOR_SERVICESTART");
            SetEvent(handle);
            CloseHandle(handle);
        }

        [DllImport("coredll")]
        private static extern bool CloseHandle(IntPtr handle);
        [DllImport("coredll", SetLastError=true)]
        private static extern IntPtr CreateEvent(IntPtr eventAttributes, bool manualReset, bool intialState, string name);
        public void Dispose()
        {
            if (this.myHandle != IntPtr.Zero)
            {
                HTCSensorClose(this.myHandle);
                this.myHandle = IntPtr.Zero;
            }
            IntPtr handle = CreateEvent(IntPtr.Zero, true, false, "HTC_GSENSOR_SERVICESTOP");
            SetEvent(handle);
            CloseHandle(handle);
        }

        [DllImport("coredll", SetLastError=true)]
        private static extern bool EventModify(IntPtr handle, uint func);
        public HTCGSensorData GetRawSensorData()
        {
            HTCGSensorData data;
            HTCSensorGetDataOutput(this.myHandle, out data);
            return data;
        }

        [DllImport("HTCSensorSDK")]
        protected static extern void HTCSensorClose(IntPtr handle);
        [DllImport("HTCSensorSDK")]
        private static extern IntPtr HTCSensorGetDataOutput(IntPtr handle, out HTCGSensorData sensorData);
        [DllImport("HTCSensorSDK")]
        protected static extern IntPtr HTCSensorOpen(HTCSensor sensor);
        private static bool SetEvent(IntPtr handle)
        {
            return EventModify(handle, 3);
        }
    }
}

