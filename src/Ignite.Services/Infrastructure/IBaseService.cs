namespace Ignite.Services.Infrastructure
{
    using Entities;
    using System.Linq;

    public interface IBaseService<T> where T : BaseEntity
    {
        /// <summary> Searches for an item based on an Id. </summary>
        /// <param name="id"> The identifier. </param>
        /// <returns> The found identifier. </returns>
        T FindById(int id);

        /// <summary> Searches for all items. </summary>
        /// <returns> The found all. </returns>
        IQueryable<T> FindAll();

        /// <summary> Exists. </summary>
        /// <param name="id"> The identifier. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        bool Exists(int id);

        /// <summary> Creates a new entity. </summary>
        /// <param name="entity"> The entity. </param>
        /// <returns> A T. </returns>
        T Create(T entity);

        /// <summary> Deletes the given records from the database. </summary>
        /// <param name="id"> The identifier. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        bool Delete(int id);

        /// <summary> Updates this object. </summary>        
        /// <param name="entity"> The entity. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        bool Update(T entity);
    }
}
