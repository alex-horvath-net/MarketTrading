using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiringManager.ReadJobRoles.Business;
using Shared;
using Shared.Business;

namespace HiringManager.ReadJobRoles.Adapters
{
    public class Test
    {

        public RepositoryMock GetRepositoryMock()
        {
            var repository = new RepositoryMock();
            return repository;
        }
        public class RepositoryMock : IRepository
        {
            public int Counter { get; private set; }
            public Task<List<JobRole>> Add(Request request)
            {
                Counter++;
                return new List<JobRole>() { new("Aladar") }.ToTask();
            }
        }
    }
}
