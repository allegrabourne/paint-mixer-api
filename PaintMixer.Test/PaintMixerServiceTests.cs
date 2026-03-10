using Moq;
using PaintMixer.Domain;
using PaintMixer.Domain.Enums;
using PaintMixer.Service;
using PaintMixer.Service.Interfaces;

namespace PaintMixer.Test
{
    public sealed class PaintMixerServiceTests
    {
        private readonly Mock<IPaintMixerDevice> _paintMixerDeviceMock;
        private readonly PaintMixerService _mixerService;

        public PaintMixerServiceTests()
        {
            _paintMixerDeviceMock = new Mock<IPaintMixerDevice>();
            _mixerService = new PaintMixerService(_paintMixerDeviceMock.Object);
        }

        [Fact]
        public async Task GivenAValidPaintMix_WhenSubmitJobAsyncIsInvoked_ThenReturnsJobCode()
        {
            var mix = new PaintMix(red: 10, blue: 20, yellow: 30, white: 10, black: 5, green: 5);

            const int expectedJobCode = 123;

            _paintMixerDeviceMock
                .Setup(device => device.SubmitJob(
                    mix.Red,
                    mix.Black,
                    mix.White,
                    mix.Yellow,
                    mix.Blue,
                    mix.Green))
                .Returns(expectedJobCode);

            var result = await _mixerService.SubmitJobAsync(mix);

            Assert.Equal((short)expectedJobCode, result);

            _paintMixerDeviceMock.Verify(device => device.SubmitJob(
                mix.Red,
                mix.Black,
                mix.White,
                mix.Yellow,
                mix.Blue,
                mix.Green), Times.Once);
        }

        [Fact]
        public async Task GivenAnEmptyPaintMix_WhenSubmitJobAsyncIsInvoked_ThenThrowsArgumentNullException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _mixerService.SubmitJobAsync(null!));

            Assert.Equal("mix", exception.ParamName);
        }

        [Fact]
        public async Task GivenDeviceRejection_WhenSubmitJobAsyncIsInvoked_ThenThrowsInvalidOperationException()
        {
            var mix = new PaintMix(red: 20, blue: 20, yellow: 20, white: 20, black: 10, green: 5);

            _paintMixerDeviceMock
                .Setup(device => device.SubmitJob(
                    mix.Red,
                    mix.Black,
                    mix.White,
                    mix.Yellow,
                    mix.Blue,
                    mix.Green))
                .Returns(-1);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _mixerService.SubmitJobAsync(mix));
        }

        [Fact]
        public async Task GivenANegativeJobCode_WhenGetJobStatusAsyncIsInvoked_ThenThrowsArgumentOutOfRangeException()
        {
            const short invalidJobCode = -1;

            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _mixerService.GetJobStatusAsync(invalidJobCode));

            Assert.Equal("jobCode", exception.ParamName);
        }

        [Fact]
        public async Task GivenADeviceReturnsMinusOne_WhenGetJobStatusAsyncIsInvoked_ThenReturnsUnknownStatus()
        {
            const short jobCode = 42;

            _paintMixerDeviceMock
                .Setup(device => device.QueryJobState(jobCode))
                .Returns(-1);

            var result = await _mixerService.GetJobStatusAsync(jobCode);

            Assert.Equal(PaintJobStatus.Unknown, result);
        }

        [Fact]
        public async Task GivenADeviceReturnsZero_WhenGetJobStatusAsyncIsInvoked_ThenReturnsRunningStatus()
        {
            const short jobCode = 42;

            _paintMixerDeviceMock
                .Setup(device => device.QueryJobState(jobCode))
                .Returns(0);

            var result = await _mixerService.GetJobStatusAsync(jobCode);

            Assert.Equal(PaintJobStatus.Running, result);
        }

        [Fact]
        public async Task GivenADeviceReturnsOne_WhenGetJobStatusAsyncIsIvoked_ThenReturnsCompletedStatus()
        {
            const short jobCode = 42;

            _paintMixerDeviceMock
                .Setup(device => device.QueryJobState(jobCode))
                .Returns(1);

            var result = await _mixerService.GetJobStatusAsync(jobCode);

            Assert.Equal(PaintJobStatus.Completed, result);
        }

        [Fact]
        public async Task GivenDeviceReturnsUnexpectedStatusCode_WhenGetJobStatusAsyncIsInvoked_ThenThrowsInvalidOperationException()
        {
            const short jobCode = 42;

            _paintMixerDeviceMock
                .Setup(device => device.QueryJobState(jobCode))
                .Returns(99);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _mixerService.GetJobStatusAsync(jobCode));
        }

        [Fact]
        public async Task GivenNegativeJobCode_WhenCancelJobAsyncIsInvoked_ThenThrowsArgumentOutOfRangeException()
        {
            const short invalidJobCode = -1;

            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _mixerService.CancelJobAsync(invalidJobCode));

            Assert.Equal("jobCode", exception.ParamName);
        }

        [Fact]
        public async Task GivenDeviceCancelsJobSuccessfully_WhenCancelJobAsyncIsInvoked_ThenCallsCancelOnce()
        {
            const short jobCode = 42;

            _paintMixerDeviceMock
                .Setup(device => device.CancelJob(jobCode))
                .Returns(0);

            await _mixerService.CancelJobAsync(jobCode);

            _paintMixerDeviceMock.Verify(device => device.CancelJob(jobCode), Times.Once);
        }

        [Fact]
        public async Task GivenDeviceFailsToCancelJob_WhenCancelJobAsyncIsInvoked_ThenThrowsInvalidOperationException()
        {
            const short jobCode = 42;

            _paintMixerDeviceMock
                .Setup(device => device.CancelJob(jobCode))
                .Returns(-1);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _mixerService.CancelJobAsync(jobCode));
        }
    }
}
