using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.IRepository
{
    public interface IEspecialidadRepository : IRepositoryGenerico<Especialidad>
    {   
        void Actualizar(Especialidad especialidad);
    }
}
