﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace Service
{
    public class EquipoRepositorio : IEquipoRepositorioBD
    {
        public FutbolDbContext context;
        public EquipoRepositorio(FutbolDbContext context)
        {
            this.context = context;
        }

        public Equipo Add(Equipo equipoNuevo)
        {
            context.Database.ExecuteSqlRaw("insertarEquipo {0}, {1}, {2}, {3}, {4}, {5}",
                                            equipoNuevo.nomEquipo,
                                           equipoNuevo.ciudad,
                                           equipoNuevo.estadio,
                                           equipoNuevo.escudo,
                                           equipoNuevo.anoFundacion,
                                           equipoNuevo.categoria);
            return equipoNuevo;
        }

        public Equipo Delete(int id)
        {
            Equipo equipoBorrar = context.equipo.Find(id);
            if (equipoBorrar != null)
            {
                context.equipo.Remove(equipoBorrar);
                context.SaveChanges();
            } return equipoBorrar;
        }

        public IEnumerable<CategoriaCuantos> EquiposPorCategoria()
        {
            return context.equipo.GroupBy(a => a.categoria).Select(g => new CategoriaCuantos()
            {
                division = g.Key.Value,
                numEquipos = g.Count()
            }) ;
        }

        public Equipo GetEquipo(int id)
        {
            SqlParameter parametro = new SqlParameter("@Id", id);
            return context.equipo.FromSqlRaw("GetEquipoById {0}", id).ToList().FirstOrDefault();
        }

        public IEnumerable<Equipo> GetEquipos()
        {
            return context.equipo.FromSqlRaw<Equipo>("select * from equipo").ToList();
        }

        public void Update(Equipo equipoActualizado)
        {
            var equipo = context.equipo.Attach(equipoActualizado);
            equipo.State = Microsoft.EntityFrameworkCore.EntityState.Modified; 
        }
    }
}