using Dynamics.Crm.Http.Connector.Core.Business.Authentication;
using Dynamics.Crm.Http.Connector.Core.Domains.Builder;
using Dynamics.Crm.Http.Connector.Core.Domains.Enums;
using Dynamics.Crm.Http.Connector.Core.Extensions;
using Dynamics.Crm.Http.Connector.Core.Extensions.DependencyInjections;
using Dynamics.Crm.Http.Connector.Core.Extensions.Utilities;
using Dynamics.Crm.Http.Connector.Core.Infrastructure.Builder;
using Dynamics.Crm.Http.Connector.Core.Infrastructure.Builder.Options;
using Dynamics.Crm.Http.Connector.Core.Persistence;
using Dynamics.Crm.Http.Connector.Core.UT.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dynamics.Crm.Http.Connector.Core.UT
{
    [TestClass]
    public class EntityDbSetIdentityUser
    {
        private IServiceProvider _provider;

        private IServiceCollection _services;

        private IDynamicsContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _services = new ServiceCollection();
            _services.AddDynamicsContext<DynamicsContext>(builder =>
            {
                builder.SetDefaultConnection(Dynamics.Connection); // Podemos user Bind desde IConfiguration para no crear nueva instancia directamente.
                builder.SetThrowExceptions(true);
                builder.AddEntityDeffinition<Contacts>();
                builder.AddEntityDeffinition<IdentityUser>();
            });
            _provider = _services.BuildServiceProvider();
            _context = _provider.GetService<IDynamicsContext>()!;
        }

        [TestMethod]
        public async Task SuccessFirstOrDefaultAsync()
        {
            try
            {
                var user = await _context.Set<IdentityUser>()
                    .FilterAnd(conditions => conditions.Equal(x => x.IdentityUserId, new Guid("d818df18-d600-ee11-8f6e-0022482dbd7a")))
                    .Distinct(true)
                    .FirstOrDefaultAsync();

                Assert.IsTrue(user != null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task FailFirstOrDefaultAsync()
        {
            try
            {
                var user = await _context.Set<IdentityUser>()
                    .FilterAnd(conditions => conditions.Equal(x => x.IdentityUserId, Guid.NewGuid()))
                    .Distinct(true)
                    .FirstOrDefaultAsync();

                Assert.IsTrue(user is null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task SuccessToListAsync()
        {
            try
            {
                var users = await _context.Set<IdentityUser>()
                    .FilterAnd(conditions =>
                    {
                        conditions.In(x => x.IdentityUserId, new Guid("07ef9235-de00-ee11-8f6e-0022482db4d8"), new Guid("42ee683c-de00-ee11-8f6e-0022482db4d8"));
                    })
                    .Distinct(true)
                    .ToListAsync();

                users = await _context.Set<IdentityUser>()
                    .FilterAnd(conditions =>
                    {
                        conditions.NotIn(x => x.IdentityUserId, new Guid("07ef9235-de00-ee11-8f6e-0022482db4d8"), new Guid("42ee683c-de00-ee11-8f6e-0022482db4d8"));
                    })
                    .Distinct(true)
                    .ToListAsync();

                users = await _context.Set<IdentityUser>()
                    .FilterAnd(conditions =>
                    {
                        conditions.Between(x => x.Age, 20, 80);
                    })
                    .Distinct(true)
                    .ToListAsync();

                users = await _context.Set<IdentityUser>()
                    .FilterAnd(conditions =>
                    {
                        conditions.NotBetween(x => x.Age, 20, 80);
                    })
                    .Distinct(true)
                    .ToListAsync();

                Assert.IsTrue(users.Count > 0);
            } 
            catch(Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task SuccessCountAsync()
        {
            try
            {
                var count = await _context.Set<IdentityUser>()
                    .Distinct(true)
                    .CountAsync();

                Assert.IsTrue(count > 0);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task SuccessToPagedListAsync()
        {
            try
            {
                var response = await _context.Set<IdentityUser>()
                    .Distinct(true)
                    .ToPagedListAsync(1, 1);

                Assert.IsTrue(response != null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task SuccessAddAsync()
        {
            try
            {
                var identityUser = new IdentityUser()
                {
                    IdentityUserId = Guid.Empty,
                    Name = "Test",
                    Age = 20,
                    CreatedOn = DateTime.Now,
                    StateCode = 1,
                    Status = 224050000,
                    OwnerId = new Guid("ef4269c5-a4f2-ec11-bb3d-00224820d6d5")
                };
                await _context.Set<IdentityUser>().AddAsync(identityUser);

                Assert.IsTrue(identityUser != null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task SuccessUpdateAsync()
        {
            try
            {
                var identityUser = new IdentityUser()
                {
                    IdentityUserId = new("48218898-5d05-ee11-8f6e-0022482db4d8"),
                    Name = "Test #2",
                    Age = 40,
                    CreatedOn = DateTime.Now,
                    StateCode = 1,
                    Status = 224050001,
                    OwnerId = new Guid("ef4269c5-a4f2-ec11-bb3d-00224820d6d5")
                };
                await _context.Set<IdentityUser>().UpdateAsync(identityUser);

                Assert.IsTrue(identityUser != null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task SuccessDeleteAsync()
        {
            try
            {
                var identityUser = new IdentityUser()
                {
                    IdentityUserId = new("48218898-5d05-ee11-8f6e-0022482db4d8"),
                    Name = "Test #2",
                    Age = 40,
                    CreatedOn = DateTime.Now,
                    StateCode = 1,
                    Status = 224050001,
                    OwnerId = new Guid("ef4269c5-a4f2-ec11-bb3d-00224820d6d5")
                };
                await _context.Set<IdentityUser>().DeleteAsync(identityUser);

                Assert.IsTrue(identityUser != null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }
    }
}
