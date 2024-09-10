using Moq;
using NUnit.Framework;
using validata_task.Entities;

namespace validata_task
{
    [TestFixture]
    public class CustomerServiceTests
    {
        private Mock<IRepository<Customer>> _customerRepository;
        private Mock<IUnitOfWork> _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _customerRepository = new Mock<IRepository<Customer>>();
            _unitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public async Task Should_Add_Customer()
        {
            var customer = new Customer { FirstName = "John", LastName = "Doe" };
            _unitOfWork.Setup(u => u.Customers.AddAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);
            //var result = await _unitOfWork.Object.Customers.AddAsync(customer);
            //Assert.NotNull(result);
        }
    }

}
