namespace System
{
    using Omu.ValueInjecter;

    /// <summary>
    /// Extensores de Object
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// Converte um objeto para outro
        /// </summary>
        /// <remarks>Este método tem impacto alto de desempenho</remarks>
        /// <param name="destination"> O objeto no qual estamos chamando MapFrom </param>
        /// <param name="origin"> Objeto origem do mapeamento </param>
        public static void MapFrom(this object destination, object origin)
        {
            if (origin == null)
            {
                throw new ArgumentException("Erro de mapeamento, objeto nulo recebido.");
            }

            destination.InjectFrom<FlatLoopValueInjection>(origin);
        }

        /// <summary>
        /// Mapeia um objeto para outro tipo
        /// </summary>
        /// <param name="origin"> The source. </param>
        /// <typeparam name="T"> Tipo do objeto destino </typeparam>
        /// <returns>O objeto mapeado para o novo tipo</returns>
        public static T MapTo<T>(this object origin) where T : new()
        {
            if (origin == null)
            {
                throw new ArgumentException("Erro de mapeamento, objeto nulo recebido.");
            }

            var vo = new T();
            vo.InjectFrom<UnflatLoopValueInjection>(origin);
            return vo;
        }
    }
}
