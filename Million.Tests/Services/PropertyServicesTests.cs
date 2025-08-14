using AutoMapper;
using Million.Application.DTOs;
using Million.Application.MapperProfile;
using Million.Application.Service;
using Million.Domain.Entities;
using Million.Domain.Ports.Interfaces;
using Moq;

namespace Million.Tests.Services
{
    public class PropertyServiceTests
    {
        private IMapper _mapper = default!;
        private Mock<IPropertyRepository> _repo = default!;
        private PropertyService _svc = default!;

        [SetUp]
        public void Setup()
        {
            var cfg = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
            _mapper = cfg.CreateMapper();
            _repo = new Mock<IPropertyRepository>(MockBehavior.Strict);
            _svc = new PropertyService(_repo.Object, _mapper);
        }

        [Test]
        public async Task Create_ShouldReturnDto()
        {
            var req = new CreatePropertyRequest("Modern Condo", "123 Ocean Dr", 450000, "ML-1001", 2016, 2);
            _repo.Setup(r => r.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Property p, CancellationToken _) => { p.IdProperty = 10; return p; });

            var dto = await _svc.CreateAsync(req, default);

            Assert.That(dto.IdProperty, Is.EqualTo(10));
            Assert.That(dto.CodeInternal, Is.EqualTo("ML-1001"));
            _repo.VerifyAll();
        }

        [Test]
        public async Task ChangePrice_ShouldReturnFalse_WhenNotFound()
        {
            _repo.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Property?)null);
            var ok = await _svc.ChangePriceAsync(999, new ChangePriceRequest(100), default);
            Assert.False(ok);
            _repo.VerifyAll();
        }

        [Test]
        public async Task ChangePrice_ShouldUpdate_WhenFound()
        {
            var entity = new Property { IdProperty = 1, Name = "X", Address = "A", Price = 10, CodeInternal = "C" };
            _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _repo.Setup(r => r.UpdateAsync(entity, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var ok = await _svc.ChangePriceAsync(1, new ChangePriceRequest(200), default);
            Assert.True(ok);
            Assert.That(entity.Price, Is.EqualTo(200));
            _repo.VerifyAll();
        }
    }

}