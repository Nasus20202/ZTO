using TDDLab.Core.Infrastructure;
using TDDLab.Core.InvoiceMgmt;
using TDDLab.Core.Tests.Helpers;

namespace TDDLab.Core.Tests.Processing;

public class WorkerImplTests
{
    private readonly Mock<IConfigurationSettings> _configurationSettingsMock = new();
    private readonly Mock<IMessagingFacility<Invoice, ProcessingResult>> _messagingFacilityMock =
        new();
    private readonly Mock<IExceptionHandler> _exceptionHandlerMock = new();
    private readonly Mock<IInvoiceProcessor> _invoiceProcessorMock = new();
    private readonly WorkerImpl _workerImpl;

    public WorkerImplTests()
    {
        _workerImpl = new(
            _configurationSettingsMock.Object,
            _messagingFacilityMock.Object,
            _exceptionHandlerMock.Object,
            _invoiceProcessorMock.Object
        );
    }

    [Fact]
    public void Start_ShouldInitializeMessagingChannels_WhenCalled()
    {
        // Arrange
        const string inputQueue = "inputQueue";
        const string outputQueue = "outputQueue";

        _configurationSettingsMock.Setup(cs => cs.GetSettingsByKey(inputQueue)).Returns(inputQueue);
        _configurationSettingsMock
            .Setup(cs => cs.GetSettingsByKey(outputQueue))
            .Returns(outputQueue);

        // Act
        _workerImpl.Start();

        // Assert
        _messagingFacilityMock.Verify(mf => mf.InitializeInputChannel(inputQueue), Times.Once);
        _messagingFacilityMock.Verify(mf => mf.InitializeOutputChannel(outputQueue), Times.Once);
    }

    [Fact]
    // This behaviour could be adjusted to make Start idempotent if needed
    public void Start_ShouldInitializeMessagingChannelsAgain_WhenCalledMultipleTimes()
    {
        // Arrange
        const string inputQueue = "inputQueue";
        const string outputQueue = "outputQueue";

        _configurationSettingsMock.Setup(cs => cs.GetSettingsByKey(inputQueue)).Returns(inputQueue);
        _configurationSettingsMock
            .Setup(cs => cs.GetSettingsByKey(outputQueue))
            .Returns(outputQueue);

        // Act
        _workerImpl.Start();
        _workerImpl.Start();

        // Assert
        _messagingFacilityMock.Verify(
            mf => mf.InitializeInputChannel(inputQueue),
            Times.Exactly(2)
        );
        _messagingFacilityMock.Verify(
            mf => mf.InitializeOutputChannel(outputQueue),
            Times.Exactly(2)
        );
    }

    [Fact]
    public void Stop_ShouldDisposeMessagingFacility_WhenCalled()
    {
        // Act
        _workerImpl.Stop();

        // Assert
        _messagingFacilityMock.Verify(mf => mf.Dispose(), Times.Once);
    }

    [Fact]
    // This behaviour could be adjusted to make Stop idempotent if needed
    public void Stop_ShouldDisposeMessagingFacilityAgain_WhenCalledMultipleTimes()
    {
        // Act
        _workerImpl.Stop();
        _workerImpl.Stop();

        // Assert
        _messagingFacilityMock.Verify(mf => mf.Dispose(), Times.Exactly(2));
    }

    [Fact]
    public void DoJob_ShouldReadMessageProcessItAndWriteResult_WhenCalled()
    {
        // Arrange
        var invoice = new InvoiceBuilder().WithDefaultValues().Build();
        var processingResult = new ProcessingResult { Result = InvoiceResult.Succeeded };
        var messageMock = new Message<Invoice> { Data = invoice, Metadata = new Metadata() };

        _messagingFacilityMock.Setup(mf => mf.ReadMessage()).Returns(messageMock);

        _invoiceProcessorMock.Setup(ip => ip.Process(invoice)).Returns(processingResult);

        // Act
        _workerImpl.DoJob();

        // Assert
        _messagingFacilityMock.Verify(mf => mf.ReadMessage(), Times.Once);
        _invoiceProcessorMock.Verify(ip => ip.Process(invoice), Times.Once);
        _messagingFacilityMock.Verify(
            mf =>
                mf.WriteMessage(
                    It.Is<Message<ProcessingResult>>(msg =>
                        msg.Data == processingResult && msg.Metadata == messageMock.Metadata
                    )
                ),
            Times.Once
        );
    }

    [Fact]
    public void DoJob_ShouldHandleException_WhenReadMessageThrowsException()
    {
        // Arrange
        var exception = new Exception("Test exception");

        _messagingFacilityMock.Setup(mf => mf.ReadMessage()).Throws(exception);

        // Act
        _workerImpl.DoJob();

        // Assert
        _exceptionHandlerMock.Verify(eh => eh.HandleException(exception), Times.Once);
    }

    [Fact]
    public void DoJob_ShouldHandleException_WhenProcessThrowsException()
    {
        // Arrange
        var invoice = new InvoiceBuilder().WithDefaultValues().Build();
        var messageMock = new Message<Invoice> { Data = invoice, Metadata = new Metadata() };
        var exception = new Exception("Test exception");

        _messagingFacilityMock.Setup(mf => mf.ReadMessage()).Returns(messageMock);
        _invoiceProcessorMock.Setup(ip => ip.Process(invoice)).Throws(exception);

        // Act
        _workerImpl.DoJob();

        // Assert
        _exceptionHandlerMock.Verify(eh => eh.HandleException(exception), Times.Once);
    }

    [Fact]
    public void DoJob_ShouldHandleException_WhenWriteMessageThrowsException()
    {
        // Arrange
        var invoice = new InvoiceBuilder().WithDefaultValues().Build();
        var messageMock = new Message<Invoice> { Data = invoice, Metadata = new Metadata() };
        var processingResult = new ProcessingResult { Result = InvoiceResult.Succeeded };

        _messagingFacilityMock.Setup(mf => mf.ReadMessage()).Returns(messageMock);
        _invoiceProcessorMock.Setup(ip => ip.Process(invoice)).Returns(processingResult);
        _messagingFacilityMock
            .Setup(mf => mf.WriteMessage(It.IsAny<Message<ProcessingResult>>()))
            .Throws(new Exception("Test exception"));

        // Act
        _workerImpl.DoJob();

        // Assert
        _exceptionHandlerMock.Verify(eh => eh.HandleException(It.IsAny<Exception>()), Times.Once);
    }
}
