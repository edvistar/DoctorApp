using Data.Interfaces.IRepository;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class EspecialidadRepository : Repository<Especialidad>, IEspecialidadRepository
    {
        private readonly ApplicationDbContext _db;

        public EspecialidadRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Especialidad especialidad)
        {
            var especialidadDb = _db.Especialidad.FirstOrDefault(e => e.Id == especialidad.Id);
            if (especialidadDb == null)
            {
                especialidadDb.NombreEspecialidad = especialidad.NombreEspecialidad;
                especialidadDb.Descripcion = especialidad.Descripcion;
                especialidadDb.Estado = especialidad.Estado;
                especialidadDb.FechaActualizacion = especialidad.FechaActualizacion;
                _db.SaveChanges();
            }
        }
    }
}
