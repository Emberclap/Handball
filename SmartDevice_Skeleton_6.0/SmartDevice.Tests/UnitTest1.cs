namespace SmartDevice.Tests
{
    using NUnit.Framework;
    using System;
    using System.Text;

    public class Tests
    {
        private Device device;
        [SetUp]
        public void Setup()
        {
            this.device = new Device(2000);
        }

        [Test]
        public void DeviceConstructorShouldInitializeCorrectlyMemoryCapacity()
        {
            int expected = 2000;
            Assert.AreEqual(expected, device.MemoryCapacity);
        }
        [Test]
        public void DeviceConstructorShouldInitializeCorrectlyAvailableMemory()
        {
            int expectedAvailableMemory = 2000;
            Assert.AreEqual(expectedAvailableMemory, device.AvailableMemory);
        }
        [Test]
        public void DeviceConstructorShouldInitializeCorrectlyPhotos()
        {
            int expectedPhotos = 0;
            Assert.AreEqual(expectedPhotos, device.Photos);
        }
        [Test]
        public void DeviceConstructorShouldInitializeCorrectlyApplications()
        {
            int expectedPhotos = 0;
            Assert.AreEqual(expectedPhotos, device.Applications.Count);
        }

        [Test]
        public void TakePhotoShouldReturnFalseWhenPhotoSizeIsBiggerThanAvailableMemory()
        {
            int photoSize = 2001;
            Assert.False(device.TakePhoto(photoSize));
        }
        [Test]
        public void TakePhotoShouldReduceAvailableMemory()
        {
            int photoSize = 1000;
            int expectedAvailableMemory = 1000;
            device.TakePhoto(photoSize);
            Assert.AreEqual(expectedAvailableMemory, device.AvailableMemory);
        }
        [Test]
        public void TakePhotoShouldIncrasePhotosCount()
        {
            int photoSize = 222;
            int expectedphotos = 2;
            device.TakePhoto(photoSize);
            device.TakePhoto(photoSize);
            Assert.AreEqual(expectedphotos, device.Photos);
        }
        [Test]
        public void TakePhotoShouldReturnTrueWhenWorkingCorrectly()
        {
            int photoSize = 222;
            device.TakePhoto(photoSize);
            Assert.True(device.TakePhoto(photoSize));
        }
        [Test]
        public void InstallAppShouldReduceAvailableMemory()
        {
            int appSize = 1000;
            int expectedAvailableMemory = 1000;
            device.InstallApp("game" ,appSize);
            Assert.AreEqual(expectedAvailableMemory, device.AvailableMemory);
        }
        [Test]
        public void InstallAppShouldAddNameToApplications()
        {
            string appName = "game";
            int appSize = 1000;
            device.InstallApp(appName, appSize);
            Assert.That(device.Applications.Contains(appName));
        }
        [Test]
        public void InstallAppShouldReturnStringMessageWhenSuccessfull()
        {
            string appName = "game";
            int appSize = 1000;
            Assert.AreEqual($"{appName} is installed successfully. Run application?", device.InstallApp(appName, appSize));
        }
        [Test]
        public void InstallAppShouldThrowErrorWhenThereIsNotEnoughMemory()
        {
            string appName = "game";
            int appSize = 3000;
           
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => device.InstallApp(appName,appSize));

            Assert.That("Not enough available memory to install the app." == ex.Message);
        }
        [Test]
        public void FormatDeviceShouldEraseAllPhotos()
        {
            device.TakePhoto(222);
            device.TakePhoto(222);
            device.FormatDevice();
            Assert.AreEqual(0,device.Photos);
            Assert.That(device.Photos == 0);
        }
        [Test]
        public void FormatDeviceShouldRestartApplicationsList()
        {
            string appName = "game";
            int appSize = 222;
            device.InstallApp(appName, appSize);
            device.InstallApp(appName, appSize);
            device.FormatDevice();
            Assert.AreEqual(0, device.Applications.Count);
            Assert.That(device.Applications.Count == 0);
        }
        [Test]
        public void FormatDeviceShouldResetAvailableMemory()
        {
            string appName = "game";
            int appSize = 222;
            device.InstallApp(appName, appSize);
            device.InstallApp(appName, appSize);
            device.FormatDevice();
            Assert.AreEqual(2000, device.AvailableMemory);
            Assert.That(device.AvailableMemory == 2000);
        }
        [Test]
        public void DeviceGetDeviceStatusShouldReturnStatusString()
        {
            int memoryCapacity = 2048;
            Device device = new Device(memoryCapacity);
            int photoSize = 100;
            device.TakePhoto(photoSize);
            device.InstallApp("MyFirstApp", 500);
            device.InstallApp("MySecondApp", 300);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Memory Capacity: {memoryCapacity} MB, Available Memory: {memoryCapacity - photoSize - 500 - 300} MB");
            stringBuilder.AppendLine($"Photos Count: 1");
            stringBuilder.AppendLine($"Applications Installed: MyFirstApp, MySecondApp");

            string result = stringBuilder.ToString().TrimEnd();
            string status = device.GetDeviceStatus();

            Assert.AreEqual(result, status);
        }

    }
    
}