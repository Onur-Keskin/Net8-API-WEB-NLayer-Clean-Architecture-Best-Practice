using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Contracts.Persistance
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync(); //etkilenen satır sayısını dönecek
    }
}
