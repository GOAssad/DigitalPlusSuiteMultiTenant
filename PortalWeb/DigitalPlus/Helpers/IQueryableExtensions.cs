using DigitalPlus.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlus.Helpers
{
    public static class IQueryableExtensions
    {
        public async static Task<int> CalcularTotalPaginas<T>(this IQueryable<T> queryable,
            int cantidadRegistrosAMostrar)
        {
            double conteo = queryable.Count();
            int totalPaginas = (int)Math.Ceiling(conteo / cantidadRegistrosAMostrar);
            return totalPaginas;
        }

        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, Paginacion paginacion)
        {
            return queryable
                .Skip((paginacion.Pagina - 1) * paginacion.CantidadRegistros)
                .Take(paginacion.CantidadRegistros);
        }
    }
}
