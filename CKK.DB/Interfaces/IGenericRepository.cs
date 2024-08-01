namespace CKK.DB.Interfaces
{
    public interface IGenericRepository<T>
    {
        T GetById(int id);
        List<T> GetAll();
        int Add(T entity);
        int Update(T entity);
        int Delete(T entity);
    }
}
