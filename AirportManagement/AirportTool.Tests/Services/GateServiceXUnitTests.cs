using AirportTool.Application.Interfaces;
using AirportTool.Application.Services;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Gates;
using AutoMapper;
using Moq;
using Xunit;

namespace AirportTool.Tests.Services
{
    public class GateServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IGateRepository> gateRepository;
        private readonly Mock<IMapper> mapper;

        private readonly GateService service;

        public GateServiceTests()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            gateRepository = new Mock<IGateRepository>();
            mapper = new Mock<IMapper>();

            unitOfWork.Setup(u => u.Gates).Returns(gateRepository.Object);

            service = new GateService(unitOfWork.Object, mapper.Object);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotFound_ReturnsNull()
        {
            gateRepository
                .Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Gate?)null);

            var result = await service.GetByIdAsync(10);

            Assert.Null(result);
            mapper.Verify(m => m.Map<GetGateByIdDto>(It.IsAny<Gate>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_WhenFound_ReturnsMappedDto()
        {
            var entity = new Gate { Id = 10, AirportId = 1, Code = "A1" };
            var dto = new GetGateByIdDto { Id = 10 };

            gateRepository
                .Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            mapper
                .Setup(m => m.Map<GetGateByIdDto>(entity))
                .Returns(dto);

            var result = await service.GetByIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal(10, result!.Id);
        }

        [Fact]
        public async Task CreateAsync_WhenGateCodeExists_ThrowsArgumentException()
        {
            var dto = new CreateGateDto
            {
                AirportId = 1,
                Code = "A1"
            };

            gateRepository
                .Setup(r => r.GateCodeExistsAsync(dto.AirportId, dto.Code, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(dto));

            gateRepository.Verify(r => r.AddAsync(It.IsAny<Gate>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_AddsEntity_Saves_AndReturnsId()
        {
            var dto = new CreateGateDto
            {
                AirportId = 1,
                Code = "A1"
            };

            gateRepository
                .Setup(r => r.GateCodeExistsAsync(dto.AirportId, dto.Code, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var entity = new Gate { Id = 77, AirportId = 1, Code = "A1" };
            mapper
                .Setup(m => m.Map<Gate>(dto))
                .Returns(entity);

            gateRepository
                .Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            unitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await service.CreateAsync(dto);

            Assert.Equal(77, result);
            gateRepository.Verify(r => r.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenGateNotFound_ThrowsKeyNotFoundException()
        {
            var dto = new UpdateGateDto { Code = "B2" };

            gateRepository
                .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Gate?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateAsync(5, dto));

            gateRepository.Verify(r => r.UpdateAsync(It.IsAny<Gate>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenGateCodeExists_ThrowsArgumentException()
        {
            var dto = new UpdateGateDto { Code = "B2" };
            var gate = new Gate { Id = 5, AirportId = 1, Code = "A1" };

            gateRepository
                .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(gate);

            gateRepository
                .Setup(r => r.GateCodeExistsAsync(gate.AirportId, dto.Code, 5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(5, dto));

            gateRepository.Verify(r => r.UpdateAsync(It.IsAny<Gate>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ValidRequest_TrimsCode_Updates_AndSaves()
        {
            var dto = new UpdateGateDto { Code = "  B2  " };
            var gate = new Gate { Id = 5, AirportId = 1, Code = "A1" };

            gateRepository
                .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(gate);

            gateRepository
                .Setup(r => r.GateCodeExistsAsync(gate.AirportId, dto.Code, 5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            gateRepository
                .Setup(r => r.UpdateAsync(gate, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            unitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await service.UpdateAsync(5, dto);

            Assert.Equal("B2", gate.Code);
            gateRepository.Verify(r => r.UpdateAsync(gate, It.IsAny<CancellationToken>()), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenUsedInSchedules_ThrowsInvalidOperationException()
        {
            gateRepository
                .Setup(r => r.IsUsedInSchedulesAsync(9, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(9));

            gateRepository.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WhenNotUsed_Deletes_AndSaves()
        {
            gateRepository
                .Setup(r => r.IsUsedInSchedulesAsync(9, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            gateRepository
                .Setup(r => r.DeleteAsync(9, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            unitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await service.DeleteAsync(9);

            gateRepository.Verify(r => r.DeleteAsync(9, It.IsAny<CancellationToken>()), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
