using AirportTool.Application.Interfaces;
using AirportTool.Application.Services;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Aircraft;
using AutoMapper;
using Moq;
using Xunit;

namespace AirportTool.Tests.Services
{
    public class AircraftServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IAircraftRepository> aircraftRepository;
        private readonly Mock<IMapper> mapper;

        private readonly AircraftService service;

        public AircraftServiceTests()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            aircraftRepository = new Mock<IAircraftRepository>();
            mapper = new Mock<IMapper>();

            unitOfWork.Setup(u => u.Aircraft).Returns(aircraftRepository.Object);

            service = new AircraftService(unitOfWork.Object, mapper.Object);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotFound_ReturnsNull()
        {
            aircraftRepository
                .Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Aircraft?)null);

            var result = await service.GetByIdAsync(10);

            Assert.Null(result);
            mapper.Verify(m => m.Map<GetAircraftByIdDto>(It.IsAny<Aircraft>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_WhenFound_ReturnsMappedDto()
        {
            var entity = new Aircraft { Id = 10 };
            var dto = new GetAircraftByIdDto { Id = 10 };

            aircraftRepository
                .Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            mapper
                .Setup(m => m.Map<GetAircraftByIdDto>(entity))
                .Returns(dto);

            var result = await service.GetByIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal(10, result!.Id);
        }

        [Fact]
        public async Task CreateAsync_WhenSeatCapacityInvalid_ThrowsArgumentException()
        {
            var dto = new CreateAircraftDto
            {
                SeatCapacity = 0,
                TailNumber = "YR-ABC"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(dto));

            aircraftRepository.Verify(r => r.AddAsync(It.IsAny<Aircraft>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenTailNumberExists_ThrowsArgumentException()
        {
            var dto = new CreateAircraftDto
            {
                SeatCapacity = 150,
                TailNumber = "YR-ABC"
            };

            aircraftRepository
                .Setup(r => r.TailNumberExistsAsync(dto.TailNumber, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(dto));

            aircraftRepository.Verify(r => r.AddAsync(It.IsAny<Aircraft>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_AddsEntity_Saves_AndReturnsId()
        {
            var dto = new CreateAircraftDto
            {
                SeatCapacity = 150,
                TailNumber = "YR-ABC"
            };

            aircraftRepository
                .Setup(r => r.TailNumberExistsAsync(dto.TailNumber, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var entity = new Aircraft { Id = 123 };
            mapper
                .Setup(m => m.Map<Aircraft>(dto))
                .Returns(entity);

            aircraftRepository
                .Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // IMPORTANT: dacă SaveChangesAsync e Task<int>
            unitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await service.CreateAsync(dto);

            Assert.Equal(123, result);
            aircraftRepository.Verify(r => r.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenSeatCapacityInvalid_ThrowsArgumentException()
        {
            var dto = new UpdateAircraftDto
            {
                SeatCapacity = -1,
                TailNumber = "YR-NEW"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(1, dto));

            aircraftRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenEntityNotFound_ThrowsKeyNotFoundException()
        {
            var dto = new UpdateAircraftDto
            {
                SeatCapacity = 100,
                TailNumber = "YR-NEW"
            };

            aircraftRepository
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Aircraft?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateAsync(1, dto));

            aircraftRepository.Verify(r => r.UpdateAsync(It.IsAny<Aircraft>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenTailNumberExists_ThrowsArgumentException()
        {
            var dto = new UpdateAircraftDto
            {
                SeatCapacity = 100,
                TailNumber = "YR-DUP"
            };

            var entity = new Aircraft { Id = 1, TailNumber = "YR-OLD" };

            aircraftRepository
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            aircraftRepository
                .Setup(r => r.TailNumberExistsAsync(dto.TailNumber, 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(1, dto));

            mapper.Verify(m => m.Map(dto, entity), Times.Never);
            aircraftRepository.Verify(r => r.UpdateAsync(It.IsAny<Aircraft>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ValidRequest_Maps_Updates_AndSaves()
        {
            var dto = new UpdateAircraftDto
            {
                SeatCapacity = 120,
                TailNumber = "YR-NEW"
            };

            var entity = new Aircraft { Id = 1, TailNumber = "YR-OLD" };

            aircraftRepository
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            aircraftRepository
                .Setup(r => r.TailNumberExistsAsync(dto.TailNumber, 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            aircraftRepository
                .Setup(r => r.UpdateAsync(entity, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            unitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await service.UpdateAsync(1, dto);

            mapper.Verify(m => m.Map(dto, entity), Times.Once);
            aircraftRepository.Verify(r => r.UpdateAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Deletes_AndSaves()
        {
            aircraftRepository
                .Setup(r => r.DeleteAsync(5, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            unitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await service.DeleteAsync(5);

            aircraftRepository.Verify(r => r.DeleteAsync(5, It.IsAny<CancellationToken>()), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
