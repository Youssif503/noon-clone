namespace noon.Application.Repository.Contract;
public interface IGenericRepository<T> where T : class
{
    Task addAsync(T entity);
    Task<T> getByIdAsync(object id);
    Task<IEnumerable<T>> getAllAsync();
    void updateAsync(T entity);
    void deleteAsync(T entity);
}