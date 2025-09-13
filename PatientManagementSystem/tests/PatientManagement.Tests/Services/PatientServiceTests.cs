#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PatientManagement.API.DTOs;
using PatientManagement.API.Models;
using PatientManagement.API.Repositories;
using PatientManagement.API.Services;
using Xunit;
#endregion

namespace PatientManagement.Tests.Services
{
    public class PatientServiceTests
    {
        #region Fields & Constructor
        private readonly Mock<IPatientRepository> _repoMock;
        private readonly PatientService _svc;

        public PatientServiceTests()
        {
            _repoMock = new Mock<IPatientRepository>();
            _svc = new PatientService(_repoMock.Object);
        }
        #endregion

        #region CreateAsync
        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenDobInFuture()
        {
            var dto = new PatientCreateDto
            {
                FirstName = "Test",
                LastName = "User",
                DOB = DateTime.UtcNow.AddDays(1),
                Gender = 'M',
                Email = "a@b.com"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _svc.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnId_WhenValid()
        {
            var dto = new PatientCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                DOB = DateTime.UtcNow.AddYears(-30),
                Gender = 'M',
                City = "NY",
                Email = "johndoe@test.com",
                Phone = "1234567890"
            };

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Patient>())).ReturnsAsync(1);

            var result = await _svc.CreateAsync(dto);

            result.Should().Be(1);
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Patient>()), Times.Once);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenDobInFuture()
        {
            var dto = new PatientUpdateDto
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                DOB = DateTime.UtcNow.AddDays(1),
                Gender = 'F',
                Email = "jane@test.com"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _svc.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnId_WhenValid()
        {
            var dto = new PatientUpdateDto
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                DOB = DateTime.UtcNow.AddYears(-25),
                Gender = 'F',
                City = "LA",
                Email = "jane@test.com",
                Phone = "9876543210"
            };

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Patient>())).ReturnsAsync(1);

            var result = await _svc.UpdateAsync(dto);

            result.Should().Be(1);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Patient>()), Times.Once);
        }
        #endregion

        #region DeleteAsync
        [Fact]
        public async Task DeleteAsync_ShouldCallRepoDelete()
        {
            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

            var result = await _svc.DeleteAsync(1);

            result.Should().Be(1);
            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GetByIdAsync_ShouldReturnPatient_WhenExists()
        {
            var patient = new Patient { Id = 1, FirstName = "John" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(patient);

            var result = await _svc.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.FirstName.Should().Be("John");
        }
        #endregion

        #region SearchAsync
        [Fact]
        public async Task SearchAsync_ShouldReturnPatients_WhenFound()
        {
            var patients = new List<Patient>
            {
                new Patient { Id = 1, FirstName = "Alice" },
                new Patient { Id = 2, FirstName = "Bob" }
            };

            _repoMock.Setup(r => r.SearchAsync(20, 40, 'F', "NY", "Diabetes"))
                     .ReturnsAsync(patients);

            var result = await _svc.SearchAsync(20, 40, 'F', "NY", "Diabetes");

            result.Should().HaveCount(2);
            result.First().FirstName.Should().Be("Alice");
        }
        #endregion
    }
}
